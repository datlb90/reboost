using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;
using Stripe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reboost.Shared;
using Reboost.Service.ZaloPay;
using Newtonsoft.Json;
using OpenAI_API.Moderation;
using Microsoft.Extensions.Logging;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController<IPaymentService>
    {
        private readonly IMapper _mapper;
        private IUserService _userService;
        private IStripeService _stripeService;
        private IOrderService _orderService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService service, IMapper mapper, IUserService userService,
            IStripeService stripeService, IOrderService orderService, ILogger<PaymentController> logger) : base(service)
        {
            _mapper = mapper;
            _userService = userService;
            _stripeService = stripeService;
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost("vnpay/request")]
        public async Task<string> GetVNPayUrl(VNPayRequestModel model)
        {
            string ipnIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            model.ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return await _service.GetVNPayUrl(model);
        }

        [HttpGet("vnpay/callback")]
        public async Task<IActionResult> VnPayCallback()
        {
            VnPayCallbackResultModel rs = new VnPayCallbackResultModel();
            List<string> whiteListIP = new List<string>
            {
                "113.160.92.202",
                "113.52.45.78",
                "116.97.245.130",
                "42.118.107.252",
                "113.20.97.250",
                "203.171.19.146",
                "103.220.87.4",
                "103.220.86.4"
            };
            try
            {
                string ipnIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _logger.LogInformation("Địa chỉ IP gọi vào IPN: " + ipnIpAddress);
                if (whiteListIP.Contains(ipnIpAddress))
                {
                    if (Request.Query != null && Request.Query.Keys.Count != 0)
                    {
                        string queryString = "";
                        foreach (string key in Request.Query.Keys)
                        {
                            queryString += key + "=" + Request.Query[key] + "&";
                        }
                        _logger.LogInformation("Data nhận được từ IPN: " + queryString);
                        VNPayVerifyResultModel model = new VNPayVerifyResultModel
                        {
                            orderId = Request.Query["vnp_TxnRef"],
                            vnpAmount = Request.Query["vnp_Amount"],
                            vnpayTranId = Request.Query["vnp_TransactionNo"],
                            vnpResponseCode = Request.Query["vnp_ResponseCode"],
                            vnpTransactionStatus = Request.Query["vnp_TransactionStatus"],
                            vnpSecureHash = Request.Query["vnp_SecureHash"],
                            queryString = queryString
                        };
                        rs = await _service.VnPayCallback(model);
                        return Ok(rs);
                    }
                    else
                    {
                        rs.RspCode = "99";
                        rs.Message = "Input data required";
                        return Ok(rs);
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception e)
            {
                rs.RspCode = "99";
                rs.Message = "Unknow error";
                return Ok(rs);
            }
        }

        [HttpPost("vnpay/verify")]
        public VerifyPaymentModel VerifyVnPayStatus(VNPayVerifyResultModel model)
        {
            model.ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return _service.VerifyVnPayStatus(model);
        }

        [HttpPost("zalopay/request")]
        public async Task<string> GetZaloPayUrl(ZaloPayRequestModel model)
        {
            model.ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return await _service.GetZaloPayUrl(model);
        }

        [HttpPost("zalopay/callback")]
        public async Task<IActionResult> ZaloPaymentCallback(CallbackRequest cbdata)
        {
            string ipnIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            _logger.LogInformation("Địa chỉ IP gọi Zalopay callback: " + ipnIpAddress);

            ZaloCallbackResultModel rs = await _service.ZaloPaymentCallback(cbdata);
            if (rs != null)
                return Ok(rs);
            else
                return BadRequest();
        }

        [HttpPost("zalopay/verify")]
        public async Task<VerifyPaymentModel> VerifyZaloPayStatus(ZaloPayVerifyResultModel model)
        {
            return await _service.VerifyZaloPayStatus(model);
        }


        [HttpGet("process/order/{orderId}")]
        public async Task<VerifyPaymentModel> ProcessOrder(int orderId)
        {
            return await _service.ProcessOrder(orderId);
        }

        [Authorize]
        [HttpPost("order")]
        public async Task<IActionResult> AddNewOrder([FromBody] Orders order)
        {
            order.CreatedDate = DateTime.UtcNow;
            order.LastActivityDate = DateTime.UtcNow;
            var rs = await _orderService.Create(order);
            return Ok(rs);
        }

        private async Task<string> GetCustomerId()
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");

            var currentUser = await _userService.GetByEmailAsync(email.Value);
            var customerId = "";
            if (!string.IsNullOrEmpty(currentUser.StripeCustomerID))
            {
                customerId = currentUser.StripeCustomerID;
            }
            else
            {
                var customer = await _stripeService.CreateCustomerAsync(currentUser);
                await _userService.UpdateStripeIdAsync(currentUser, customer.Id);
                customerId = customer.Id;
            }
            return customerId;
        }

        [Authorize]
        [HttpPost("intent")]
        public async Task<IActionResult> GetNewIntent([FromBody] CreateIntentModel amount)
        {
            string customerId = await this.GetCustomerId();

            PaymentIntent paymentIntent = await _stripeService.CreateIntentAsync(customerId, amount.Amount);
            return Ok(paymentIntent);
        }
        [Authorize]
        [HttpPost("subscribe")]
        public async Task<IActionResult> CreateNewSubcription([FromBody] CreateSubscriptionModel model)
        {
            string customerId = await this.GetCustomerId();

            Subscription subscription = await _stripeService.CreateSubcription(customerId, model.priceId, model.methodId);
            return Ok(subscription);
        }
        [Authorize]
        [HttpGet("subscribe/{customerId}")]
        public async Task<IActionResult> GetUserSubscriptions([FromRoute] string customerId)
        {
            StripeList<Subscription> subscriptions = await _stripeService.GetSubscriptionAsync(customerId);
            return Ok(subscriptions);
        }
        [Authorize]
        [HttpGet("account/create/{UserId}")]
        public async Task<IActionResult> CreateBankAccount(string UserId)
        {

            AccountLink linkAccount = await _stripeService.CreateAccountAsync(UserId);

            return Ok(linkAccount);
        }
        [Authorize]
        [HttpGet("account/list")]
        public async Task<IActionResult> GetAllBankAccount([FromBody] BankAccountCreateModel model)
        {
            StripeList<Account> accountList = await _stripeService.GetAllBankAccountAsync(model.AccountId);

            return Ok(accountList);
        }
        [Authorize]
        [HttpGet("account/{userId}")]
        public async Task<IActionResult> GetAccountListById(string userId)
        {
            Account account = await _stripeService.GetAccount(userId);
            if(account == null)
            {
                return Ok("null");
            }
            return Ok(account);
        }

        //[Authorize]
        //[HttpPost("payout")]
        //public async Task<IActionResult> CreatePayout([FromBody] TransferModel model)
        //{
        //    Payout rs = await _stripeService.CreatePayoutAsync(model.Amount, model.Destination);
        //    return Ok(rs);
        //}

        [Authorize]
        [HttpPost("transfer")]
        public async Task<IActionResult> CreateTranfer([FromBody] TransferModel model)
        {
            Transfer tf = await _stripeService.CreateTransferAsync(model.Amount, model.Destination);
            
            await _service.CreatePaymentAsync(new Payments { 
                Amount = model.Amount,
                Currency = model.Currency,
                Description = "Pay for review",
                PaymentDate = DateTime.UtcNow,
                PaymentId = tf.Id,
                Status = "success",
                Type = "OUT",
                UserId = model.UserId
            });
            //PaymentHistory ph = new PaymentHistory()
            //{
            //    Amount = tf.Amount,
            //    CreatedDate = tf.Created.ToUniversalTime(),
            //    Name = "transfer"
            //};
            //await _service.CreateNewPaymentAsync(ph);
            return Ok(tf);
        }

        //[HttpGet("transfer")]
        //public async Task<IActionResult> GetAllTransferHistory()
        //{
        //    return Ok(await _service.GetAllPaymentHistory());
        //}
        [Authorize]
        [HttpPost("method/attach")]
        public async Task<IActionResult> AttachMethodToCustomer([FromBody] AttachMethodModel model)
        {
            string customerId = await this.GetCustomerId();

            PaymentMethod customer = await _stripeService.AttachMethodAsync(customerId, model.methodId);
            return Ok(customer);
        }
        [Authorize]
        [HttpGet("products/list")]
        public async Task<IActionResult> ListAllProucts()
        {
            var products = await _stripeService.ListAllProducts();
            return Ok(products.Data);
        }
        [Authorize]
        [HttpGet("prices/list")]
        public async Task<IActionResult> ListAllPrices()
        {
            var prices = await _stripeService.ListAllPrices();
            return Ok(prices.Data);
        }
        [Authorize]
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerInfo(string customerId)
        {
            return Ok(await _stripeService.GetCustomerAsync(customerId));
        }
        [Authorize]
        [HttpGet("method/list/{customerId}")]
        public async Task<IActionResult> GetAllMethods(string customerId)
        {
            var cards = await _stripeService.GetMethodsByCustomerIdAsync(customerId);
            return Ok(cards.Data);
        }
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Payments>> GetAll()
        {
            var rs = await _service.GetAllAsync();
            return rs;
        }
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IEnumerable<Payments>> GetByUserId([FromRoute] string userId)
        {
            var rs = await _service.GetAllPaymentByUserIdAsync(userId);
            return rs;
        }
        [Authorize]
        [HttpGet("out/{userId}")]
        public async Task<IEnumerable<Payments>> GetOutPaymentByUserId([FromRoute] string userId)
        {
            var rs = await _service.GetOutPaymentByUserId(userId);
            return rs;
        }
        [Authorize]
        [HttpPost("create")]
        public async Task<Payments> CreatePayment([FromBody] Payments pm)
        {
            return await _service.CreatePaymentAsync(pm);
        }
        [Authorize]
        [HttpGet("balance/{accountId}")]
        public async Task<Balance> GetBalanceAsync(string accountId)
        {
            return await _stripeService.GetBalanceAsync(accountId);
        }
        [Authorize]
        [HttpGet("loginLink/{accountId}")]
        public async Task<object> GetLoginLink(string accountId)
        {
            return await _stripeService.GetLoginLinkAsync(accountId);
        }
        [Authorize]
        [HttpGet("account/connected/{userId}")]
        public async Task<bool> CheckAccountConnected(string userId)
        {
            return await _stripeService.UserCompletedOnboarding(userId);
        }
        [Authorize]
        [HttpGet("defaultPaymentMethod/{customerId}")]
        public async Task<PaymentMethod> GetDefaultPaymentMethodAsync(string customerId)
        {
            return await _stripeService.GetDefaultPaymentMethodAsync(customerId);
        }
        [Authorize]
        [HttpGet("updateDefaultPaymentMethod/{customerId}/{defaultPaymentMethodId}")]
        public async Task<bool> UpdateDefaultPaymentMethod(string customerId, string defaultPaymentMethodId)
        {
            return await _stripeService.UpdateDefaultPaymentMethod(customerId, defaultPaymentMethodId);
        }
        [Authorize]
        [HttpGet("customerId")]
        public async Task<IActionResult> GetUserCustomerId()
        {
            var rs = await GetCustomerId();
            return Ok(rs);
        }

        [Authorize]
        [HttpPost("paypal/payout")]
        public async Task<IActionResult> HandlePaypalPayout()
        {
            var currentUserClaim = HttpContext.User;
            var userEmail = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(userEmail.Value);

            var rs = await _service.RaterPayout(currentUser.Id);
            return Ok(rs);
        }

        [Authorize]
        [HttpGet("balance/available")]
        public async Task<IActionResult> GetAvailableBalance()
        {
            var currentUserClaim = HttpContext.User;
            var userEmail = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(userEmail.Value);

            var rs = await _service.GetRaterPayableBalanceAsync(currentUser.Id);
            return Ok(rs);
        }

        [Authorize]
        [HttpGet("balance")]
        public async Task<IActionResult> GetAllRaterBalance()
        {
            var currentUserClaim = HttpContext.User;
            var userEmail = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(userEmail.Value);

            var rs = await _service.GetAllRaterBalanceAsync(currentUser.Id);
            return Ok(rs);
        }

        [Authorize]
        [HttpGet("paypal/connect/{mail}")]
        public async Task<IActionResult> ConnectToPaypalAccount([FromRoute] string mail)
        {
            var currentUserClaim = HttpContext.User;
            var userEmail = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(userEmail.Value);

            return Ok(currentUser);
        }

        [Authorize]
        [HttpPost("paypal/paymentHistory")]
        public async Task<IActionResult> CreatePaymentHistoryAsync([FromBody] LearnerPaymentsHistory data)
        {
            data.CreatedDate = DateTime.UtcNow;

            var rs = await _service.CreatePaymentHistoryAsync(data);
            return Ok(rs);
        }


        [Authorize]
        [HttpPost("paypal/subscribe")]
        public async Task<IActionResult> CreateUpdateSubscriptionAsync([FromBody] LearnerSubscriptions data)
        {
            var currentUserClaim = HttpContext.User;
            var userEmail = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(userEmail.Value);
            data.UserId = currentUser.Id;

            var rs = await _service.CreateUpdateSubscriptionAsync(data);
            return Ok(rs);
        }

        [Authorize]
        [HttpGet("paypal/subscribe")]
        public async Task<IActionResult> GetSubscriptionAsync()
        {
            var currentUserClaim = HttpContext.User;
            var userEmail = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(userEmail.Value);

            var rs = await _service.GetLearnerSubscriptions(currentUser.Id);
            return Ok(rs);
        }

        [Authorize]
        [HttpGet("paypal/auth/basicToken")]
        public async Task<IActionResult> GetPaypalBasicAuthToken()
        {
            var rs = _service.GetBasicTokenAsync();
            return Ok(rs);
        }
    }
}
