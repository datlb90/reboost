using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;
using Stripe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController<IPaymentService>
    {
        private readonly IMapper _mapper;
        private IUserService _userService;
        private IStripeService _stripeService;

        public PaymentController(IPaymentService service, IMapper mapper, IUserService userService, IStripeService stripeService) : base(service)
        {
            _mapper = mapper;
            _userService = userService;
            _stripeService = stripeService;
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

        [HttpPost("intent")]
        public async Task<IActionResult> GetNewIntent([FromBody] CreateIntentModel amount)
        {
            string customerId = await this.GetCustomerId();

            PaymentIntent paymentIntent = await _stripeService.CreateIntentAsync(customerId, amount.Amount);
            return Ok(paymentIntent);
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> CreateNewSubcription([FromBody] CreateSubscriptionModel model)
        {
            string customerId = await this.GetCustomerId();

            Subscription subscription = await _stripeService.CreateSubcription(customerId, model.priceId, model.methodId);
            return Ok(subscription);
        }
        [HttpGet("subscribe/{customerId}")]
        public async Task<IActionResult> GetUserSubscriptions([FromRoute] string customerId)
        {
            StripeList<Subscription> subscriptions = await _stripeService.GetSubscriptionAsync(customerId);
            return Ok(subscriptions);
        }
        [HttpGet("account/create/{UserId}")]
        public async Task<IActionResult> CreateBankAccount(string UserId)
        {

            AccountLink linkAccount = await _stripeService.CreateAccountAsync(UserId);

            return Ok(linkAccount);
        }

        [HttpGet("account/list")]
        public async Task<IActionResult> GetAllBankAccount([FromBody] BankAccountCreateModel model)
        {
            StripeList<Account> accountList = await _stripeService.GetAllBankAccountAsync(model.AccountId);

            return Ok(accountList);
        }

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


        [HttpPost("payout")]
        public async Task<IActionResult> CreatePayout([FromBody] TransferModel model)
        {
            Payout rs = await _stripeService.CreatePayoutAsync(model.Amount, model.Destination);
            return Ok(rs);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> CreateTranfer([FromBody] TransferModel model)
        {
            Transfer tf = await _stripeService.CreateTransferAsync(model.Amount, model.Destination);
            
            await _service.CreatePaymentAsync(new Payments { 
                Amount = model.Amount,
                Currency = model.Currency,
                Description = "Pay for review",
                PaymentDate = DateTime.Now,
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

        [HttpPost("method/attach")]
        public async Task<IActionResult> AttachMethodToCustomer([FromBody] AttachMethodModel model)
        {
            string customerId = await this.GetCustomerId();

            PaymentMethod customer = await _stripeService.AttachMethodAsync(customerId, model.methodId);
            return Ok(customer);
        }

        [HttpGet("products/list")]
        public async Task<IActionResult> ListAllProucts()
        {
            var products = await _stripeService.ListAllProducts();
            return Ok(products.Data);
        }

        [HttpGet("prices/list")]
        public async Task<IActionResult> ListAllPrices()
        {
            var prices = await _stripeService.ListAllPrices();
            return Ok(prices.Data);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerInfo(string customerId)
        {
            return Ok(await _stripeService.GetCustomerAsync(customerId));
        }

        [HttpGet("method/list/{customerId}")]
        public async Task<IActionResult> GetAllMethods(string customerId)
        {
            var cards = await _stripeService.GetMethodsByCustomerIdAsync(customerId);
            return Ok(cards.Data);
        }

        [HttpGet]
        public async Task<IEnumerable<Payments>> GetAll()
        {
            var rs = await _service.GetAllAsync();
            return rs;
        }
        [HttpGet("{userId}")]
        public async Task<IEnumerable<Payments>> GetByUserId([FromRoute] string userId)
        {
            var rs = await _service.GetAllPaymentByUserIdAsync(userId);
            return rs;
        }
        [HttpGet("out/{userId}")]
        public async Task<IEnumerable<Payments>> GetOutPaymentByUserId([FromRoute] string userId)
        {
            var rs = await _service.GetOutPaymentByUserId(userId);
            return rs;
        }

        [HttpPost("create")]
        public async Task<Payments> CreatePayment([FromBody] Payments pm)
        {
            return await _service.CreatePaymentAsync(pm);
        }

        [HttpGet("balance/{accountId}")]
        public async Task<Balance> GetBalanceAsync(string accountId)
        {
            return await _stripeService.GetBalanceAsync(accountId);
        }

        [HttpGet("loginLink/{accountId}")]
        public async Task<object> GetLoginLink(string accountId)
        {
            return await _stripeService.GetLoginLinkAsync(accountId);
        }

        [HttpGet("account/connected/{userId}")]
        public async Task<bool> CheckAccountConnected(string userId)
        {
            return await _stripeService.UserCompletedOnboarding(userId);
        }
        [HttpGet("defaultPaymentMethod/{customerId}")]
        public async Task<PaymentMethod> GetDefaultPaymentMethodAsync(string customerId)
        {
            return await _stripeService.GetDefaultPaymentMethodAsync(customerId);
        }
        [HttpGet("updateDefaultPaymentMethod/{customerId}/{defaultPaymentMethodId}")]
        public async Task<bool> UpdateDefaultPaymentMethod(string customerId, string defaultPaymentMethodId)
        {
            return await _stripeService.UpdateDefaultPaymentMethod(customerId, defaultPaymentMethodId);
        }
    }
}
