using Newtonsoft.Json.Linq;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payments>> GetAllAsync();
        Task<List<Payments>> GetAllPaymentByUserIdAsync(string id);
        Task<Payments> CreatePaymentAsync(Payments pm);
        Task<User> UpdateStripeId(User user, string id);
        Task<IEnumerable<Payments>> GetOutPaymentByUserId(string userId);
        //Task<List<PaymentHistory>> GetAllPaymentHistory();
        //Task<PaymentHistory> CreateNewPaymentAsync(PaymentHistory ph);
        Task<string> RaterPayout(string userId);
        Task<List<RaterBalances>> GetRaterPayableBalanceAsync(string userId);
        Task<List<RaterBalances>> GetAllRaterBalanceAsync(string userId);
        Task<string> LearnerRefund(string userId, string email);
        Task<LearnerPaymentsHistory> CreatePaymentHistoryAsync(LearnerPaymentsHistory data);
        Task<LearnerSubscriptions> CreateUpdateSubscriptionAsync(LearnerSubscriptions data);
        Task<LearnerSubscriptions> GetLearnerSubscriptions(string userId);

    }
    public class PaymentService :BaseService, IPaymentService
    {
        private IStripeService mStripeService;
        private IRaterService mRaterService;
        public PaymentService(IUnitOfWork unitOfWork, IStripeService stripeService, IRaterService raterService): base(unitOfWork)
        {
            mStripeService = stripeService;
            mRaterService = raterService;
        }

        public async Task<IEnumerable<Payments>> GetAllAsync()
        {
            return await _unitOfWork.Payment.GetAllAsync();
        }
        public async Task<List<Payments>> GetAllPaymentByUserIdAsync(string id)
        {
            return await _unitOfWork.Payment.GetAllPaymentsByUserId(id);
        }
        public async Task<Payments> CreatePaymentAsync(Payments pm)
        {
            pm.PaymentDate = DateTime.Now;
            return await _unitOfWork.Payment.Create(pm);
        }
        public async Task<User> UpdateStripeId(User user, string id)
        {
            user.StripeCustomerID = id;
            return await _unitOfWork.Users.UpdateAsync(user);
        }
        public async Task<IEnumerable<Payments>> GetOutPaymentByUserId(string userId)
        {
            return await _unitOfWork.Payment.GetOutPaymentByUserId(userId);
        }

        //public async Task<List<PaymentHistory>> GetAllPaymentHistory()
        //{
        //    return await _unitOfWork.Payment.GetAllPaymentHistory();
        //}
        //public async Task<PaymentHistory> CreateNewPaymentAsync(PaymentHistory ph)
        //{
        //    return await _unitOfWork.Payment.CreateNewPaymentHistory(ph);
        //}

        public async Task<List<RaterBalances>> GetAllRaterBalanceAsync(string userId)
        {
            return await _unitOfWork.Payment.GetAllRaterBalanceAsync(userId);
        }

        public async Task<List<RaterBalances>> GetRaterPayableBalanceAsync(string userId)
        {
            return await _unitOfWork.Payment.GetRaterPayableBalanceAsync(userId);
        }

        public async Task<List<RaterBalances>> UpdatePaidBalancesAsync(string userId)
        {
            return await _unitOfWork.Payment.UpdatePaidBalancesAsync(userId);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public string GetPaypalToken()
        {
            var client = new WebClient();

            // Encode client-id and secret to base64
            string encodedKey = Base64Encode("AamTDpWxcBAOJ4twRr0E_XVy1z2uJ3AxTU9BejLB0sJCM3Om2RuApDwSZce6Lterg8aaNl-XWIyxw__F:EOyXbN3RRGLv6S5y9PwMIiIpIsMjsBhH2FcO1kxpZH8PT69O8JtEza67L43XO19os0mNpjvFCzqCxVYV");

            client.Headers.Add("authorization", "basic " + encodedKey);
            client.Headers.Add("content-type", "application/x-www-form-urlencoded");
            client.Headers.Add("accept-language", "en_US");
            client.Headers.Add("accept", "application/json");

            var body = "grant_type=client_credentials";

            try
            {
                var response = client.UploadString("https://api-m.sandbox.paypal.com/v1/oauth2/token", "POST", body);
                var responseJson = JObject.Parse(response);
                var temp = responseJson["access_token"].Value<String>();
                return temp;
            }
            catch (WebException e)
            {
                var errorResponse = e.Response as HttpWebResponse;
                string responseText;

                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }

                return responseText;
            }
        }

        public async Task<string> RaterPayout(string userId)
        {
            var rater = await mRaterService.GetByUserIdAsync(userId);

            if (rater == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Rater not Exists!");
            }

            var balances = await GetRaterPayableBalanceAsync(userId);
            double totalBalances = 0;

            if (balances.Count > 0)
            {
                foreach (var b in balances)
                {
                    totalBalances += b.Total;
                }
            }


            if (totalBalances == 0)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Your available balance does not enough!");
            }

            string emailSubject = "You have a payout.";
            string emailMessage = "You have just received a new payout from Reboost! Thanks for using our service!";

            return await HandlePayout(userId, rater.PaypalAccount, totalBalances, emailSubject, emailMessage, false);
        }

        public async Task<string> LearnerRefund(string userId, string email)
        {
            double amount = PaidAmount.VALUE;

            string emailSubject = "You have a refund.";
            string emailMessage = "You have just received a refund from Reboost! Thanks for using our service!";

            return await HandlePayout(userId, email, amount, emailSubject, emailMessage, true);
        }

        public async Task<string> HandlePayout(string userId, string email, double paidAmount, string emailSub, string emailMess, bool isRefund)
        {
            string currentToken = GetPaypalToken();

            if (currentToken == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Token invalid!");
            }

            var client = new WebClient();
            client.Headers.Add("authorization", "Bearer " + currentToken);
            client.Headers.Add("content-type", "application/json");
            client.Headers.Add("accept-language", "en_US");
            client.Headers.Add("accept", "application/json");

            var body = @"{
			""sender_batch_header"":{
			""sender_batch_id"":""Payout_" + DateTime.Now.ToFileTime() + @""",
			" + @"""email_subject"":""" + emailSub + @""",
			" + @"""email_message"":""" + emailMess + @"""
			" + @"},
			  ""items"": [{
			    ""recipient_type"":""EMAIL"",
			    " + @"""amount"":{
					""value"":""" + paidAmount.ToString() + @""",
					""currency"":""USD""
					},
				""note"":""Thanks for your patronage!"",
				""sender_item_id"":""201403140001"",
				""receiver"":""" + email + @""",
                ""notification_language"":""en-US""
				}]
			}";

            try
            {
                var response = client.UploadString("https://api-m.sandbox.paypal.com/v1/payments/payouts", "POST", body);
                
                if(response!= null && !isRefund)
                {
                    await UpdatePaidBalancesAsync(userId);
                }

                return response;
            }
            catch (WebException e)
            {
                var errorResponse = e.Response as HttpWebResponse;
                string responseText;
                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }

                throw new AppException(ErrorCode.InvalidArgument, responseText);

            }
        }

        public async Task<LearnerPaymentsHistory> CreatePaymentHistoryAsync(LearnerPaymentsHistory data)
        {
            return await _unitOfWork.Payment.CreatePaymentHistoryAsync(data);
        }

        public async Task<LearnerSubscriptions> CreateUpdateSubscriptionAsync(LearnerSubscriptions data)
        {
            return await _unitOfWork.Payment.CreateUpdateSubscriptionAsync(data);
        }

        public async Task<LearnerSubscriptions> GetLearnerSubscriptions(string userId)
        {
            return await _unitOfWork.Payment.GetLearnerSubscriptions(userId);
        }
    }
}
