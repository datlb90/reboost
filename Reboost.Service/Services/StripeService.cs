using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PayPal.Api;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Newtonsoft.Json;

namespace Reboost.Service.Services
{
    public interface IStripeService {
        Task<Customer> CreateCustomerAsync(User user);
        Task<PaymentMethod> GetPaymentMethodAsync(string paymentMethodId);
        Task<PaymentIntent> CreateIntentAsync(string customerId, int amount);
        Task<StripeList<PaymentMethod>> GetMethodsByCustomerIdAsync(string customerId);
        Task<Customer> GetCustomerAsync(string id);
        Task<StripeList<Product>> ListAllProducts();
        Task<StripeList<Price>> ListAllPrices();
        Task<Subscription> CreateSubcription(string userId, string priceId, string methodId);
        Task<PaymentMethod> AttachMethodAsync(string userId, string methodId);
        Task<AccountLink> CreateAccountAsync(string UserId);
        //Task<Payout> CreatePayoutAsync(decimal amount, string customerId);
        Task<StripeList<Account>> GetAllBankAccountAsync(string customerId);
        Task<Transfer> CreateTransferAsync(decimal amount, string accountId);
        Task<Account> GetAccount(string userId);
        Task<Balance> GetBalanceAsync(string customerId);
        Task<object> GetLoginLinkAsync(string accountId);
        Task<StripeList<Subscription>> GetSubscriptionAsync(string customerId);
        Task<bool> UserCompletedOnboarding(string userId);
        Task<PaymentMethod> GetDefaultPaymentMethodAsync(string customerId);
        Task<bool> UpdateDefaultPaymentMethod(string customerId, string defaultPaymentMethodId);
    }
    public class StripeService : BaseService, IStripeService
    {
        public StripeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<Customer> CreateCustomerAsync(User user) {
            var options = new CustomerCreateOptions
            {
                Description = user.Id,
                Email = user.Email,
                Name = user.FirstName + " " + user.LastName
            };
            var service = new CustomerService();
            return await service.CreateAsync(options);
        }
        public async Task<Customer> GetCustomerAsync(string id)
        {
            return await new CustomerService().GetAsync(id);
        }
        public async Task<PaymentMethod> GetPaymentMethodAsync(string paymentMethodId)
        {
            return await new PaymentMethodService().GetAsync(paymentMethodId);
        }
        public async Task<StripeList<PaymentMethod>> GetMethodsByCustomerIdAsync(string customerId)
        {
            var options = new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = "card",
            };
            StripeList<PaymentMethod> paymentMethods = new PaymentMethodService().List(options);
            return paymentMethods;
        }
        public async Task<PaymentIntent> CreateIntentAsync(string customerId, int amount)
        {
            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                Customer = customerId,
                SetupFutureUsage = "off_session"
            });
            return paymentIntent;
        }
        public async Task<StripeList<Product>> ListAllProducts()
        {
            return await new ProductService().ListAsync();
        }
        public async Task<StripeList<Price>> ListAllPrices()
        {
            return await new PriceService().ListAsync();
        }
        public async Task<Subscription> CreateSubcription(string userId, string priceId, string methodId)
        {
            var options = new SubscriptionCreateOptions
            {
                Customer = userId,
                Items = new List<SubscriptionItemOptions>
                  {
                    new SubscriptionItemOptions
                    {
                      Price = priceId,
                    },
                  },
                DefaultPaymentMethod = methodId
            };
            var service = new SubscriptionService();
            return await service.CreateAsync(options);
        }
        public async Task<PaymentMethod> AttachMethodAsync(string userId, string methodId)
        {
            var options = new PaymentMethodAttachOptions
            {
                Customer = userId,
            };
            var service = new PaymentMethodService();
            return await service.AttachAsync(
              methodId,
              options
            );
        }
        public async Task<AccountLink> CreateAccountAsync(string userId)
        {
            var opt = new AccountCreateOptions
            {
                Type = "express",
            };
            var accountId = "";

            var userAccount = await _unitOfWork.Payment.GetAccount(userId);
            if(userAccount != null)
            {
                accountId = userAccount.StripeAccountId;
            }
            else
            {
                Account acc = await new AccountService().CreateAsync(opt);
                accountId = acc.Id;
                await _unitOfWork.Payment.UpdateUserStripeAccounts(new UserStripeAccounts { UserId = userId, StripeAccountId = accountId });
            }

            var options = new AccountLinkCreateOptions
            {
                Account = accountId,
                RefreshUrl = "http://localhost:3011/rater/StartRating",
                ReturnUrl = "http://localhost:3011/rater/StartRating/" + accountId,
                Type = "account_onboarding",
            };
            
            return await new AccountLinkService().CreateAsync(options);
        }
        public async Task<Account> GetAccount(string userId)
        {
            UserStripeAccounts acc = await _unitOfWork.Payment.GetAccount(userId);
            if(acc == null || string.IsNullOrEmpty(acc.StripeAccountId))
            {
                return await Task.FromResult<Account>(null);
            }
            return await new AccountService().GetAsync(acc.StripeAccountId);
        }

        public async Task<StripeList<Account>> GetAllBankAccountAsync(string customerId)
        {
            var service = new AccountService();
            return await service.ListAsync();
        }

        //public async Task<Payout> CreatePayoutAsync(decimal amount, string customerId)
        //{
        //    var options = new PayoutCreateOptions
        //    {
        //        Amount = (long)amount,
        //        Currency = "usd",
        //        //Method = "instant"
        //        StatementDescriptor = "Reboost"
        //    };
        //    var requestOptions = new RequestOptions();
        //    requestOptions.StripeAccount = customerId;
        //    return await new PayoutService().CreateAsync(options, requestOptions);
        //}

        public async Task<Transfer> CreateTransferAsync(decimal amount, string destination)
        {
            var options = new TransferCreateOptions
            {
                Amount = (long)amount,
                Currency = "usd",
                Destination = destination,
            };
            return await new TransferService().CreateAsync(options);
        }

        public async Task<Balance> GetBalanceAsync(string accountId)
        {
            var requestOptions = new RequestOptions();
            requestOptions.StripeAccount = accountId;
            var service = new BalanceService();
            Balance balance = await service.GetAsync(requestOptions);
            return balance;
        }

        public async Task<object> GetLoginLinkAsync(string accountId)
        {
            var service = new LoginLinkService();
            return await service.CreateAsync(accountId);
        }

        public async Task<StripeList<Subscription>> GetSubscriptionAsync(string customerId)
        {
            var options = new SubscriptionListOptions
            {
                Customer = customerId
            };
            var service = new SubscriptionService();
            StripeList<Subscription> subscriptions = await service.ListAsync(
              options
            );
            return subscriptions;
        }
        public async Task<bool> UserCompletedOnboarding(string userId) {
            var account = await GetAccount(userId);
            if(account == null)
            {
                return false;
            }

            var service = new AccountService();
            var accountDetail = await service.GetAsync(account.Id);

            return accountDetail.DetailsSubmitted;
        }
        public async Task<PaymentMethod> GetDefaultPaymentMethodAsync(string customerId)
        {
            var service = new CustomerService();
            var customer = await service.GetAsync(customerId);
            if (customer == null || customer.InvoiceSettings.DefaultPaymentMethodId == null)
            {
                return null;
            }
            var service1 = new PaymentMethodService();
            var defaultPaymentMethod = await service1.GetAsync(customer.InvoiceSettings.DefaultPaymentMethodId);
            return defaultPaymentMethod;
        }
        public async Task<bool> UpdateDefaultPaymentMethod(string customerId, string defaultPaymentMethodId)
        {
            var service = new CustomerService();
            var customer = await service.GetAsync(customerId);
            if(customer == null)
            {
                return false;
            }

            await AttachMethodAsync(customerId, defaultPaymentMethodId);

            var options = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions()
                {
                    DefaultPaymentMethod = defaultPaymentMethodId
                }
            };
            await service.UpdateAsync(customerId, options);
            return true;
        }
        
    }
}
