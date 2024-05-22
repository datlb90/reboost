using Hangfire.Dashboard;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI_API.Moderation;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X9;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.ZaloPay;
using Reboost.Shared;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using Hangfire;

namespace Reboost.Service.Services
{
    public interface IPaymentService
    {
        Task<VerifyPaymentModel> VerifyZaloPayStatus(ZaloPayVerifyResultModel model);
        Task<ZaloCallbackResultModel> ZaloPaymentCallback(CallbackRequest cbdata);
        Task<string> GetZaloPayUrl(ZaloPayRequestModel model);
        Task<VerifyPaymentModel> VerifyVnPayStatus(VNPayVerifyResultModel model);
        Task<VnPayCallbackResultModel> VnPayCallback(VNPayVerifyResultModel model);
        Task<string> GetVNPayUrl(VNPayRequestModel model);
        Task<VerifyPaymentModel> ProcessOrder(string paymentMethod, int orderId, string ipAddress);
        Task<IEnumerable<Payments>> GetAllAsync();
        Task<List<Payments>> GetAllPaymentByUserIdAsync(string id);
        Task<Payments> CreatePaymentAsync(Payments pm);
        Task<User> UpdateStripeId(User user, string id);
        Task<IEnumerable<Payments>> GetOutPaymentByUserId(string userId);
        Task<string> RaterPayout(string userId);
        Task<List<RaterBalances>> GetRaterPayableBalanceAsync(string userId);
        Task<List<RaterBalances>> GetAllRaterBalanceAsync(string userId);
        Task<string> LearnerRefund(string userId, string email);
        Task<LearnerPaymentsHistory> CreatePaymentHistoryAsync(LearnerPaymentsHistory data);
        Task<LearnerSubscriptions> CreateUpdateSubscriptionAsync(LearnerSubscriptions data);
        Task<List<string>> GetLearnerSubscriptions(string userId);
        string GetBasicTokenAsync();
    }
    public class PaymentService : BaseService, IPaymentService
    {
        private IStripeService mStripeService;
        private IRaterService mRaterService;
        private IOrderService _orderService;
        private IReviewService _reviewService;
        private IConfiguration _configuration;
        private IMailService _mailService;
        private IUserService _userService;
        private ISubscriptionService _subscriptionService;
        public PaymentService(IUnitOfWork unitOfWork, IStripeService stripeService, IRaterService raterService,
            IUserService userService, IMailService mailService, IOrderService orderService,
            IReviewService reviewService, ISubscriptionService subscriptionService, IConfiguration configuration)
            : base(unitOfWork)
        {
            mStripeService = stripeService;
            mRaterService = raterService;
            _configuration = configuration;
            _orderService = orderService;
            _reviewService = reviewService;
            _userService = userService;
            _mailService = mailService;
            _subscriptionService = subscriptionService;
        }

        public async Task<Orders> CreateNewOrder(string userId, int planId, string subscriptionType, string ipAddress)
        {
            Plans plan = await _subscriptionService.GetPlan(planId);
            int amount = plan.Price * plan.Duration;
            if(subscriptionType == "upgrade")
            {
                Subscriptions userSubscription = await _subscriptionService.GetUserActiveSubscription(userId);
                // Verify if this is actually an upgrade
                if (userSubscription != null && userSubscription.PlanId <= 3 && planId >= 4)
                {
                    // Recalculate the total based on prorated amount
                    int proratedAmount = await _subscriptionService.GetUserProratedAmount(userId);
                    amount = amount - proratedAmount;
                }
                
            }
            // use the amount for the plan if the type is 'new' or 'renew'
            Orders order = new Orders
            {
                UserId = userId,
                PlanId = planId,
                SubscriptionType = subscriptionType,
                Amount = amount,
                Status = PaymentStatus.PENDING,
                CreatedDate = DateTime.Now.AddHours(12),
                LastActivityDate = DateTime.Now,
                IpAddress = ipAddress
            };

            return await _orderService.Create(order);
        }

        public async Task<string> GetVNPayUrl(VNPayRequestModel model)
        {
            // Create a new order
            Orders newOrder = await CreateNewOrder(model.userId, model.planId, model.subscriptionType, model.ipAddress);
            if (newOrder != null)
            {
                string orderInfo = "";
                string planName = "phản hồi chi tiết";
                if (newOrder.PlanId >= 4)
                    planName = "phản hồi chuyên sâu";

                if (model.subscriptionType == "new")
                    orderInfo = "Thanh toán gói " + planName + " " + model.duration + " tháng. Mã đơn hàng " + newOrder.Id;
                else if(model.subscriptionType == "renew")
                    orderInfo = "Thanh toán gia hạn gói " + planName + " " + model.duration + " tháng. Mã đơn hàng " + newOrder.Id;
                else
                    orderInfo = "Thanh toán nâng cấp gói " + planName + " " + model.duration + " tháng. Mã đơn hàng " + newOrder.Id;

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
                vnpay.AddRequestData("vnp_Amount", (newOrder.Amount * 100).ToString());
                vnpay.AddRequestData("vnp_CreateDate", newOrder.CreatedDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", model.ipAddress);
                vnpay.AddRequestData("vnp_Locale", "vn");
                vnpay.AddRequestData("vnp_OrderInfo", orderInfo);
                //default value: other
                vnpay.AddRequestData("vnp_OrderType", "other");
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
                vnpay.AddRequestData("vnp_TxnRef", newOrder.Id.ToString());

                // Create schedule to check callback after 15 minute
                BackgroundJob.Schedule(
                    () => CheckVnPayStatusAfter15Minutes(newOrder.Id, model.ipAddress), TimeSpan.FromMinutes(15)
                );

                return vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            }

            return null;
        }

        public async Task CheckVnPayStatusAfter15Minutes(int orderId, string ipAddress)
        {
            var vnpData = _configuration.GetSection("PaymentGateway:VNPay");
            string vnp_HashSecret = vnpData["vnp_HashSecret"];

            // check if callback has been receive
            // If not, get order status and process the order accordingly;
            Orders order = await _orderService.GetById(orderId);
            if (order.Status == PaymentStatus.PENDING)
            {
                // Chưa nhận được callback
                // Gọi API truy vấn trạng thái thanh toán của đơn hàng để lấy kết quả cuối cùng
                var vnp_Api = vnpData["vnp_Api"];
                var vnp_TmnCode = vnpData["vnp_TmnCode"]; // Terminal Id
                var vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu truy vấn giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
                var vnp_Version = VnPayLibrary.VERSION; //2.1.0
                var vnp_Command = "querydr";
                var vnp_TxnRef = orderId.ToString(); // Mã giao dịch thanh toán tham chiếu
                var vnp_OrderInfo = "Truy van giao dich:" + orderId.ToString();
                var vnp_TransactionDate = order.CreatedDate.ToString("yyyyMMddHHmmss");
                var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                var vnp_IpAddr = ipAddress;

                var signData = vnp_RequestId + "|" + vnp_Version + "|" + vnp_Command + "|" + vnp_TmnCode + "|" + vnp_TxnRef + "|" + vnp_TransactionDate + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
                var secureHash = Utils.HmacSHA512(vnp_HashSecret, signData);

                var qdrData = new
                {
                    vnp_RequestId = vnp_RequestId,
                    vnp_Version = vnp_Version,
                    vnp_Command = vnp_Command,
                    vnp_TmnCode = vnp_TmnCode,
                    vnp_TxnRef = vnp_TxnRef,
                    vnp_OrderInfo = vnp_OrderInfo,
                    vnp_TransactionDate = vnp_TransactionDate,
                    vnp_CreateDate = vnp_CreateDate,
                    vnp_IpAddr = vnp_IpAddr,
                    vnp_SecureHash = secureHash
                };
                var jsonData = JsonConvert.SerializeObject(qdrData);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(vnp_Api);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    VnPayTransactionStatusModel rs = JsonConvert.DeserializeObject<VnPayTransactionStatusModel>(streamReader.ReadToEnd());
                    if (rs.vnp_TransactionStatus == "00")
                    {
                        // Update user subscription
                        if (order.SubscriptionType == "new")
                            await _subscriptionService.SubscribeUserToPlan(order.UserId, order.PlanId);
                        else if (order.SubscriptionType == "renew")
                            await _subscriptionService.RenewUserSubscription(order.UserId, order.PlanId);
                        else
                            await _subscriptionService.UpgradeUserSubscription(order.UserId, order.PlanId);
                        // Payment success, update order status
                        order.Status = PaymentStatus.PROCESSED;
                        order.TransactionCode = rs.vnp_TransactionNo;
                        order.LastActivityDate = DateTime.UtcNow;
                        await _orderService.Update(order);
                    }
                    else
                    {
                        // Payment failure
                        order.Status = PaymentStatus.ERROR;
                        order.TransactionCode = rs.vnp_TransactionNo;
                        order.LastActivityDate = DateTime.UtcNow;
                        await _orderService.Update(order);
                    }
                }
            }
        }

        public async Task<VnPayCallbackResultModel> VnPayCallback(VNPayVerifyResultModel model)
        {
            VnPayCallbackResultModel result = new VnPayCallbackResultModel();
            try
            {
                var vnpData = _configuration.GetSection("PaymentGateway:VNPay");
                string vnp_HashSecret = vnpData["vnp_HashSecret"];
                int orderId = Convert.ToInt32(model.orderId);
                int vnp_Amount = Convert.ToInt32(model.vnpAmount) / 100;
                string vnp_ResponseCode = model.vnpResponseCode;
                string vnp_TransactionStatus = model.vnpTransactionStatus;
                String vnp_SecureHash = model.vnpSecureHash;

                VnPayLibrary vnpay = new VnPayLibrary();
                if (!String.IsNullOrEmpty(model.queryString))
                {
                    string[] queryStrings = model.queryString.Split("&");
                    if (queryStrings.Length > 0)
                    {
                        foreach (string s in queryStrings)
                        {
                            string[] sParameterName = s.Split("=");
                            //get all querystring data
                            if (!string.IsNullOrEmpty(sParameterName[0]) && sParameterName[0].StartsWith("vnp_"))
                            {
                                vnpay.AddResponseData(sParameterName[0], sParameterName[1]);
                            }
                        }
                        bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                        if (checkSignature)
                        {
                            // Cap nhat order
                            Orders order = await _orderService.GetById(orderId);
                            if (order != null)
                            {
                                if (order.Amount == vnp_Amount)
                                {
                                    if (order.Status == PaymentStatus.PENDING)
                                    {
                                        if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                                        {
                                            // Thanh toán thành công
                                            // Cập nhật trạng thái cho đơn hàng
                                            order.Status = PaymentStatus.COMPLETED;
                                            order.TransactionCode = model.vnpayTranId;
                                            order.LastActivityDate = DateTime.UtcNow;
                                            await _orderService.Update(order);
                                        }
                                        else
                                        {
                                            // Thanh toán không thành công
                                            // Cập nhật trạng thái cho đơn hàng
                                            order.Status = PaymentStatus.ERROR;
                                            order.TransactionCode = model.vnpayTranId;
                                            order.LastActivityDate = DateTime.UtcNow;
                                            await _orderService.Update(order);
                                        }
                                        result.RspCode = "00";
                                        result.Message = "Confirm Success";
                                    }
                                    else
                                    {
                                        result.RspCode = "02";
                                        result.Message = "Order already confirmed";
                                    }
                                }
                                else
                                {
                                    result.RspCode = "04";
                                    result.Message = "invalid amount";
                                }
                            }
                            else
                            {
                                result.RspCode = "01";
                                result.Message = "Order not found";
                            }
                        }
                        else
                        {
                            result.RspCode = "97";
                            result.Message = "Invalid signature";
                        }
                    }
                    else
                    {
                        result.RspCode = "99";
                        result.Message = "Input data required";
                    }
                }
                else
                {
                    result.RspCode = "99";
                    result.Message = "Input data required";
                }
                return result;
            }
            catch (Exception e)
            {
                result.RspCode = "99";
                result.Message = "Unknow error";
                return result;
            }
        }

        public async Task<VerifyPaymentModel> VerifyVnPayStatus(VNPayVerifyResultModel model)
        {
            VerifyPaymentModel result = new VerifyPaymentModel();
            // Trả về trạng thái lỗi by default 
            result.status = OrderStatus.ERROR;

            var vnpData = _configuration.GetSection("PaymentGateway:VNPay");
            string vnp_HashSecret = vnpData["vnp_HashSecret"];
            int orderId = Convert.ToInt32(model.orderId);
            result.orderId = orderId;
            int vnp_Amount = Convert.ToInt32(model.vnpAmount) / 100;
            string vnp_ResponseCode = model.vnpResponseCode;
            string vnp_TransactionStatus = model.vnpTransactionStatus;
            String vnp_SecureHash = model.vnpSecureHash;
            VnPayLibrary vnpay = new VnPayLibrary();

            if (!String.IsNullOrEmpty(model.queryString))
            {
                string[] queryStrings = model.queryString.Split("&");
                if (queryStrings.Length > 0)
                {
                    foreach (string s in queryStrings)
                    {
                        string[] sParameterName = s.Split("=");
                        if (!string.IsNullOrEmpty(sParameterName[0]) && sParameterName[0].StartsWith("vnp_"))
                        {
                            vnpay.AddResponseData(sParameterName[0], sParameterName[1]);
                        }
                    }
                    bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                    if (checkSignature)
                    {
                        Orders order = await _orderService.GetById(orderId);
                        if(order != null && order.Amount == vnp_Amount)
                        {
                            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                            {
                                // Thanh toán thành công
                                result.status = OrderStatus.COMPLETED;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public async Task<string> GetZaloPayUrl(ZaloPayRequestModel model)
        {
            Orders newOrder = await CreateNewOrder(model.userId, model.planId, model.subscriptionType, model.ipAddress);
            if (newOrder != null)
            {
                string orderInfo = "";
                string planName = "phản hồi chi tiết";
                if (newOrder.PlanId >= 4)
                    planName = "phản hồi chuyên sâu";

                if (model.subscriptionType == "new")
                    orderInfo = "Thanh toán gói " + planName + " " + model.duration + " tháng. Mã đơn hàng " + newOrder.Id;
                else if (model.subscriptionType == "renew")
                    orderInfo = "Thanh toán gia hạn gói " + planName + " " + model.duration + " tháng. Mã đơn hàng " + newOrder.Id;
                else
                    orderInfo = "Thanh toán nâng cấp gói " + planName + " " + model.duration + " tháng. Mã đơn hàng " + newOrder.Id;

                var embed_data = new { preferred_payment_method = new string[] { } };
                var items = new[] { new { } };
                var app_trans_id = newOrder.Id;
                var param = new Dictionary<string, string>();

                param.Add("app_id", _configuration.GetSection("PaymentGateway:ZaloPay")["app_id"]);
                param.Add("app_user", model.userId);
                param.Add("app_time", Util.GetTimeStamp().ToString());
                param.Add("amount", newOrder.Amount.ToString());
                param.Add("app_trans_id", newOrder.CreatedDate.ToString("yyMMdd") + "_" + app_trans_id); // mã giao dich có định dạng yyMMdd_xxxx
                param.Add("embed_data", JsonConvert.SerializeObject(embed_data));
                param.Add("item", JsonConvert.SerializeObject(items));
                param.Add("description", orderInfo);
                param.Add("bank_code", "");
                param.Add("callback_url", _configuration["AppUrl"] + "/api/payment/zalopay/callback");
                param.Add("redirect_url", _configuration["ClientUrl"] + "/payment/redirect"); // Replace with the actual redirect_url

                var data = _configuration.GetSection("PaymentGateway:ZaloPay")["app_id"] + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                    + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
                param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _configuration.GetSection("PaymentGateway:ZaloPay")["key1"], data));

                var result = await HttpHelper.PostFormAsync(_configuration.GetSection("PaymentGateway:ZaloPay")["create_order_url"], param);

                // Send the order_url to the front end for redirection
                int return_code = Int32.Parse(result["return_code"].ToString());
                if (return_code == 1)
                {
                    // Create schedule to check callback after 15 minute?
                    BackgroundJob.Schedule(
                        () => CheckZaloPayStatusAfter15Minutes(newOrder.Id, newOrder.CreatedDate.ToString("yyMMdd") + "_" + app_trans_id), TimeSpan.FromMinutes(15)
                    );
                    return result["order_url"].ToString();
                }
                return null;
            }
            return null;
        }

        public async Task CheckZaloPayStatusAfter15Minutes(int orderId, string appTransId)
        {
            // check if callback has been receive
            // If not, get order status and process the order accordingly
            Orders order = await _orderService.GetById(orderId);
            if (order.Status == PaymentStatus.PENDING)
            {
                // Chưa nhận được callback
                // Gọi API truy vấn trạng thái thanh toán của đơn hàng để lấy kết quả cuối cùng
                var param = new Dictionary<string, string>();
                string appId = _configuration.GetSection("PaymentGateway:ZaloPay")["app_id"];
                string key1 = _configuration.GetSection("PaymentGateway:ZaloPay")["key1"];
                param.Add("app_id", appId);
                param.Add("app_trans_id", appTransId);
                var data = appId + "|" + appTransId + "|" + key1;

                param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));

                var orderStatusResult = await HttpHelper.PostFormAsync(_configuration.GetSection("PaymentGateway:ZaloPay")["query_order_url"], param);
                int orderStatus = Int32.Parse(orderStatusResult["return_code"].ToString());
                if (orderStatus == 1)
                {
                    // Update user subscription
                    if (order.SubscriptionType == "new")
                        await _subscriptionService.SubscribeUserToPlan(order.UserId, order.PlanId);
                    else if (order.SubscriptionType == "renew")
                        await _subscriptionService.RenewUserSubscription(order.UserId, order.PlanId);
                    else
                        await _subscriptionService.UpgradeUserSubscription(order.UserId, order.PlanId);

                    // Payment success, update order
                    order.Status = PaymentStatus.PROCESSED;
                    order.TransactionCode = orderStatusResult["zp_trans_id"].ToString();
                    order.LastActivityDate = DateTime.UtcNow;
                    order = await _orderService.Update(order);
                }
                else
                {
                    // Payment failure
                    order.Status = PaymentStatus.ERROR;
                    order.TransactionCode = orderStatusResult["zp_trans_id"].ToString();
                    order.LastActivityDate = DateTime.UtcNow;
                    await _orderService.Update(order);
                }
            }
        }

        public async Task<VerifyPaymentModel> VerifyZaloPayStatus(ZaloPayVerifyResultModel model)
        {
            VerifyPaymentModel result = new VerifyPaymentModel();
            // Trả về trạng thái lỗi by default 
            result.status = OrderStatus.ERROR;

            var checksumData = model.appid + "|" + model.apptransid + "|" + model.pmcid + "|" +
                model.bankcode + "|" + model.amount + "|" + model.discountamount + "|" + model.status;
            var checksum = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _configuration.GetSection("PaymentGateway:ZaloPay")["key2"], checksumData);

            if (checksum.Equals(model.checksum))
            {
                // Kiểm tra xem đã nhận được callback hay chưa,
                // nếu chưa thì tiến hành gọi API truy vấn trạng thái thanh toán của đơn hàng để lấy kết quả cuối cùng

                // Kiểm tra đã nhận được callback hay chưa bằng cách truy vấn trạng thái đơn hàng của Reboost
                // Nếu trạng thái đơn hàng là 1, và có transaction id, nghĩa là đã nhận được callback
                // Nếu trạng thái đơn hàng là 0, nghĩa là chưa nhận được callback hoặc có nhận được nhưng không xử lý đc
                string transactionId = model.apptransid;
                string[] transPlited = transactionId.Split('_');
                // Get the orderId from transaction id
                int orderId = Int32.Parse(transPlited[1]);

                Orders order = await _orderService.GetById(orderId);
                // Sanity check to make sure this is the right order
                if (order != null && order.UserId == model.userId && order.Amount == Int32.Parse(model.amount))
                {
                    if(order.Status == PaymentStatus.COMPLETED)
                    {
                        // Đã nhận được call back
                        result.status = OrderStatus.COMPLETED;
                    }
                    else // Chưa nhận được callback
                    {
                        // Gọi API truy vấn trạng thái thanh toán của đơn hàng để lấy kết quả cuối cùng
                        var param = new Dictionary<string, string>();
                        string appId = _configuration.GetSection("PaymentGateway:ZaloPay")["app_id"];
                        string key1 = _configuration.GetSection("PaymentGateway:ZaloPay")["key1"];
                        param.Add("app_id", appId);
                        param.Add("app_trans_id", model.apptransid);
                        var data = appId + "|" + model.apptransid + "|" + key1;

                        param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));

                        var orderStatusResult = await HttpHelper.PostFormAsync(_configuration.GetSection("PaymentGateway:ZaloPay")["query_order_url"], param);
                        int orderStatus = Int32.Parse(orderStatusResult["return_code"].ToString());
                        if (orderStatus == 1)
                        {
                            // Thanh toán thành công 
                            result.status = OrderStatus.COMPLETED;
                        }
                    }
                }
            }

            return result;
        }

        public async Task<ZaloCallbackResultModel> ZaloPaymentCallback(CallbackRequest cbdata)
        {
            ZaloCallbackResultModel result = new ZaloCallbackResultModel();
            result.return_code = -1;
            result.return_message = "Error";
            try
            {
                var dataStr = Convert.ToString(cbdata.Data);
                var reqMac = Convert.ToString(cbdata.Mac);
                var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _configuration.GetSection("PaymentGateway:ZaloPay")["key2"], dataStr);
                Console.WriteLine("mac = {0}", mac);
                // kiểm tra callback hợp lệ (đến từ ZaloPay server)
                if (!reqMac.Equals(mac))
                {
                    // Callback không hợp lệ
                    result.return_code = -1;
                    result.return_message = "Mac not equal";
                }
                else
                {
                    // Thanh toán thành công
                    // Merchant cập nhật trạng thái cho đơn hàng
                    var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
                    string transactionId = dataJson["app_trans_id"].ToString();
                    string[] transPlited = transactionId.Split('_');
                    // Get the orderId from transaction id
                    int orderId = Int32.Parse(transPlited[1]);
                    int amount = Int32.Parse(dataJson["amount"].ToString());
                    Orders order = await _orderService.GetById(orderId);
                    if (order != null)
                    {
                        if (order.Amount == amount)
                        {
                            if (order.Status == PaymentStatus.PENDING)
                            {
                                // Cập nhật trạng thái cho đơn hàng
                                order.Status = PaymentStatus.COMPLETED;
                                order.TransactionCode = dataJson["zp_trans_id"].ToString();
                                order.LastActivityDate = DateTime.UtcNow;
                                await _orderService.Update(order);
                                result.return_code = 1;
                                result.return_message = "success";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ZaloPay server sẽ callback lại (tối đa 3 lần)
                result.return_code = 0; 
                result.return_message = ex.Message;
            }
            // thông báo kết quả cho ZaloPay server
            return result;
        }

        public async Task<VerifyPaymentModel> ProcessOrder(string paymentMethod, int orderId, string ipAddress)
        {
            VerifyPaymentModel result = new VerifyPaymentModel();
            result.status = OrderStatus.ERROR;
            // Wait for 1s to get callback from the payment providers
            await Task.Delay(1000);
            Orders order = await _orderService.GetById(orderId);
            result.order = order;
            if(order != null)
            {
                // The the status of the order is still pending, Gọi API truy vấn trạng thái thanh toán của đơn hàng để lấy kết quả cuối cùng
                if (order.Status == PaymentStatus.PENDING)
                {
                    if(paymentMethod == "vnpay")
                    {
                        // Truy cập trạng thái đơn hàng và updateu
                        var vnpData = _configuration.GetSection("PaymentGateway:VNPay");
                        string vnp_HashSecret = vnpData["vnp_HashSecret"];

                        // Gọi API truy vấn trạng thái thanh toán của đơn hàng để lấy kết quả cuối cùng
                        var vnp_Api = vnpData["vnp_Api"];
                        var vnp_TmnCode = vnpData["vnp_TmnCode"]; // Terminal Id
                        var vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu truy vấn giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
                        var vnp_Version = VnPayLibrary.VERSION; //2.1.0
                        var vnp_Command = "querydr";
                        var vnp_TxnRef = orderId.ToString(); // Mã giao dịch thanh toán tham chiếu
                        var vnp_OrderInfo = "Truy van giao dich:" + orderId.ToString();
                        var vnp_TransactionDate = order.CreatedDate.ToString("yyyyMMddHHmmss");
                        var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                        var vnp_IpAddr = ipAddress;

                        var signData = vnp_RequestId + "|" + vnp_Version + "|" + vnp_Command + "|" + vnp_TmnCode + "|" + vnp_TxnRef + "|" + vnp_TransactionDate + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
                        var secureHash = Utils.HmacSHA512(vnp_HashSecret, signData);

                        var qdrData = new
                        {
                            vnp_RequestId = vnp_RequestId,
                            vnp_Version = vnp_Version,
                            vnp_Command = vnp_Command,
                            vnp_TmnCode = vnp_TmnCode,
                            vnp_TxnRef = vnp_TxnRef,
                            vnp_OrderInfo = vnp_OrderInfo,
                            vnp_TransactionDate = vnp_TransactionDate,
                            vnp_CreateDate = vnp_CreateDate,
                            vnp_IpAddr = vnp_IpAddr,
                            vnp_SecureHash = secureHash
                        };
                        var jsonData = JsonConvert.SerializeObject(qdrData);

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(vnp_Api);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(jsonData);
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            VnPayTransactionStatusModel rs = JsonConvert.DeserializeObject<VnPayTransactionStatusModel>(streamReader.ReadToEnd());
                            if (rs.vnp_TransactionStatus == "00")
                            {
                                // Update user subscription
                                if (order.SubscriptionType == "new")
                                    await _subscriptionService.SubscribeUserToPlan(order.UserId, order.PlanId);
                                else if (order.SubscriptionType == "renew")
                                    await _subscriptionService.RenewUserSubscription(order.UserId, order.PlanId);
                                else
                                    await _subscriptionService.UpgradeUserSubscription(order.UserId, order.PlanId);
                                // Update status to processed
                                order.Status = PaymentStatus.PROCESSED;
                                order.LastActivityDate = DateTime.UtcNow;
                                await _orderService.Update(order);

                                result.status = OrderStatus.PROCESSED;
                                result.order = order;
                            }
                        }
                    }
                    else if(paymentMethod == "zalopay")
                    {
                        // Gọi API truy vấn trạng thái thanh toán của đơn hàng để lấy kết quả cuối cùng
                        var param = new Dictionary<string, string>();
                        string appId = _configuration.GetSection("PaymentGateway:ZaloPay")["app_id"];
                        string key1 = _configuration.GetSection("PaymentGateway:ZaloPay")["key1"];
                        string appTransId = order.CreatedDate.ToString("yyMMdd") + "_" + order.Id.ToString();
                        param.Add("app_id", appId);
                        param.Add("app_trans_id", appTransId);
                        var data = appId + "|" + appTransId + "|" + key1;

                        param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));

                        var orderStatusResult = await HttpHelper.PostFormAsync(_configuration.GetSection("PaymentGateway:ZaloPay")["query_order_url"], param);
                        int orderStatus = Int32.Parse(orderStatusResult["return_code"].ToString());
                        if (orderStatus == 1)
                        {
                            // Update user subscription
                            if (order.SubscriptionType == "new")
                                await _subscriptionService.SubscribeUserToPlan(order.UserId, order.PlanId);
                            else if (order.SubscriptionType == "renew")
                                await _subscriptionService.RenewUserSubscription(order.UserId, order.PlanId);
                            else
                                await _subscriptionService.UpgradeUserSubscription(order.UserId, order.PlanId);

                            // Update status to processed
                            order.Status = PaymentStatus.PROCESSED;
                            order.LastActivityDate = DateTime.UtcNow;
                            await _orderService.Update(order);

                            result.status = OrderStatus.PROCESSED;
                            result.order = order;
                        }
                    }
                }
                else if (order.Status == PaymentStatus.COMPLETED)
                {
                    // Update user subscription
                    if (order.SubscriptionType == "new")
                        await _subscriptionService.SubscribeUserToPlan(order.UserId, order.PlanId);
                    else if (order.SubscriptionType == "renew")
                        await _subscriptionService.RenewUserSubscription(order.UserId, order.PlanId);
                    else
                        await _subscriptionService.UpgradeUserSubscription(order.UserId, order.PlanId);

                    // Update status to processed
                    order.Status = PaymentStatus.PROCESSED;
                    order.LastActivityDate = DateTime.UtcNow;
                    await _orderService.Update(order);
                }
                else if(order.Status == PaymentStatus.PROCESSED)
                {
                    result.order = order;
                    result.status = OrderStatus.PROCESSED;
                }
                else
                {
                    // Payment failed, cannot process order
                    result.order = order;
                    result.status = OrderStatus.ERROR;
                }
            }
            return result;
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
