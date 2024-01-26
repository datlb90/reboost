using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.X9;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IPaymentService
    {
        string GetVNPayUrl(VNPayRequestModel model);
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
        Task<List<string>> GetLearnerSubscriptions(string userId);
        string GetBasicTokenAsync();
    }
    public class PaymentService :BaseService, IPaymentService
    {
        private IStripeService mStripeService;
        private IRaterService mRaterService;
        private IConfiguration _configuration;
        public PaymentService(IUnitOfWork unitOfWork, IStripeService stripeService, IRaterService raterService, IConfiguration configuration) : base(unitOfWork)
        {
            mStripeService = stripeService;
            mRaterService = raterService;
            _configuration = configuration;
        }
        public string GetVNPayUrl(VNPayRequestModel model)
        {
            //Get configuration info
            var vnpData = _configuration.GetSection("PaymentGateway:VNPay");
            string vnp_Returnurl = vnpData["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = vnpData["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = vnpData["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = vnpData["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ.
            //Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_Amount", (model.amount * 10000).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.AddHours(12).ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", model.ipAddress);
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán dịch vụ chấm bài với mã nộp bài: " + model.submmissionId);
            //default value: other
            vnpay.AddRequestData("vnp_OrderType", "other"); 
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            vnpay.AddRequestData("vnp_TxnRef", model.submmissionId.ToString()); 
            return vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
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
            pm.PaymentDate = DateTime.UtcNow;
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

        public string GetBasicTokenAsync()
        {
            var rs = Base64Encode("AamTDpWxcBAOJ4twRr0E_XVy1z2uJ3AxTU9BejLB0sJCM3Om2RuApDwSZce6Lterg8aaNl-XWIyxw__F:EOyXbN3RRGLv6S5y9PwMIiIpIsMjsBhH2FcO1kxpZH8PT69O8JtEza67L43XO19os0mNpjvFCzqCxVYV");
            //var rs = Base64Encode("AamTDpWxcBAOJ4twRr0E_XVy1z2uJ3AxTU9BejLB0sJCM3Om2RuApDwSZce6Lterg8aaNl-XWIyxw__F");
            //rs += ":" + Base64Encode("EOyXbN3RRGLv6S5y9PwMIiIpIsMjsBhH2FcO1kxpZH8PT69O8JtEza67L43XO19os0mNpjvFCzqCxVYV");
            return rs;
        }

        public string GetPaypalBearerToken()
        {
            var client = new WebClient();

            // Encode client-id and secret to base64
            string encodedKey = GetBasicTokenAsync();

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

            if (rater == null || rater.PaypalAccount == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, rater == null ? "Rater not Exists!" : "Rater's Paypal Account not existed!");
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
            string currentToken = GetPaypalBearerToken();

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
			""sender_batch_id"":""Payout_" + DateTime.UtcNow.ToFileTime() + @""",
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

        public async Task<List<string>> GetLearnerSubscriptions(string userId)
        {
            GetSubscriptionsModel listSubs = await _unitOfWork.Payment.GetLearnerSubscriptions(userId);
            List<string> rs = new List<string>();

            if (listSubs.MonthSub != null)
            {
                listSubs.IsMonthExpired = await IsSubscriptionExpired(listSubs.MonthSub, "month");
                if (!listSubs.IsMonthExpired)
                {
                    rs.Add("month");
                }
            }
            if (listSubs.YearSub != null)
            {
                listSubs.IsYearExpired = await IsSubscriptionExpired(listSubs.YearSub, "year");
                if (!listSubs.IsYearExpired)
                {
                    rs.Add("year");
                }
            }

            return rs;
        }

        // Return FALSE if subscription has not been expired, TRUE if it is
        public async Task<Boolean> IsSubscriptionExpired(string subsId, string subType)
        {
            string bearerToken = GetPaypalBearerToken();

            if (bearerToken == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Token invalid!");
            }

            var url = "https://api-m.sandbox.paypal.com/v1/billing/subscriptions/"+subsId;

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.ContentType = "application/json";
            httpRequest.Headers["Authorization"] = "Bearer " + bearerToken;

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var responseJson = JObject.Parse(result);
                    var temp = responseJson["billing_info"];
                    DateTime next_billing_time = temp["next_billing_time"].Value<DateTime>();

                    if (next_billing_time >= DateTime.UtcNow) {
                        return false;
                    }                 
                }
            }

            Console.WriteLine(httpResponse.StatusCode);
            return true;
        }
    }
}
