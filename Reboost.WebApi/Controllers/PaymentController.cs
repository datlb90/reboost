using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
using Stripe;
using Reboost.Service.Services;
using Reboost.WebApi.Identity;
using AutoMapper;
using Reboost.DataAccess.Models;

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
            string customerId = this.GetCustomerId().ToString();

            PaymentIntent paymentIntent = await _stripeService.CreateIntentAsync(customerId, amount.Amount);
            return Ok(paymentIntent);
        }
        
        [HttpPost("subscribe")]
        public async Task<IActionResult> CreateNewSubcription([FromBody] CreateSubscriptionModel model)
        {
            string customerId = this.GetCustomerId().ToString();

            Subscription subscription = await _stripeService.CreateSubcription(customerId, model.priceId, model.methodId);
            return Ok(subscription);
        }

        [HttpPost("account/create")]
        public async Task<IActionResult> CreateBankAccount([FromBody] BankAccountCreateModel model)
        {
            Token token = await _stripeService.CreateBankAccountTokenAsync();
            Account bankAccount = await _stripeService.CreateAccountAsync(model.UserId, token.Id);

            return Ok(bankAccount);
        }

        [HttpGet("account/list")]
        public async Task<IActionResult> GetAllBankAccount([FromBody] BankAccountCreateModel model)
        {
            StripeList<Account> accountList = await _stripeService.GetAllBankAccountAsync(model.UserId);

            return Ok(accountList);
        }

        [HttpPost("payout")]
        public async Task<IActionResult> CreatePayout([FromBody] CreateIntentModel amount)
        {
            //StripeList<BankAccount> accountList = await _stripeService.GetAllBankAccountAsync("cus_ImuGb5bF0SG7pp");
            Payout rs = await _stripeService.CreatePayoutAsync(amount.Amount, "ba_1ID5R9Ri9PbTN1XbCkx0G9li");
            return Ok(rs);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> CreateTranfer([FromBody] CreateIntentModel amount)
        {
            Transfer tf = await _stripeService.CreateTransferAsync(amount.Amount, "acct_1IDTlYDFjbd39QL3");
            return Ok(tf);
        }

        [HttpPost("method/attach")]
        public async  Task<IActionResult> AttachMethodToCustomer([FromBody] AttachMethodModel model)
        {
            string customerId = this.GetCustomerId().ToString();

            PaymentMethod customer = await _stripeService.AttachMethodAsync(customerId,model.methodId);
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
        [HttpPost("create")]
        public async Task<Payments> CreatePayment([FromBody] Payments pm)
        {
            return await _service.CreatePaymentAsync(pm);
        }
    }
}
