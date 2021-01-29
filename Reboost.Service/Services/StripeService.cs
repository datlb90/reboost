using Reboost.DataAccess.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        Task<Token> CreateBankAccountTokenAsync();
        Task<Account> CreateAccountAsync(string userId, string token);
        Task<Payout> CreatePayoutAsync(int amount, string customerId);
        Task<StripeList<Account>> GetAllBankAccountAsync(string customerId);
        Task<Transfer> CreateTransferAsync(int amount, string accountId);
    }
    public class StripeService: IStripeService
    {
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

        public async Task<Token> CreateBankAccountTokenAsync()
        {
            var options = new TokenCreateOptions
            {
                BankAccount = new TokenBankAccountOptions
                {
                    Country = "US",
                    Currency = "usd",
                    AccountHolderName = "To Nhan",
                    AccountHolderType = "individual",
                    RoutingNumber = "110000000",
                    AccountNumber = "000123456789",
                    
                },
            };
            return await new TokenService().CreateAsync(options);
           
        }

        public async Task<Account> CreateAccountAsync(string customerId, string token)
        {
            var opt = new AccountCreateOptions
            {
                Type = "express",
                Country = "US",
                Email = "nhanth191.3@gmail.com",
                BusinessType = "individual",
                BusinessProfile = new AccountBusinessProfileOptions
                {
                    Url = "customer.com",
                },
                //TosAcceptance = new AccountTosAcceptanceOptions
                //{
                //    Date = DateTime.Today,
                //    Ip = "27.65.61.146",
                //},
                Individual = new AccountIndividualOptions
                {
                    FirstName ="Quang",
                    LastName = "Duy",
                    SsnLast4 ="1233",
                    Phone = "+15448887665",
                    Address = new AddressOptions
                    {
                        City = "Dover",
                        Country = "US",
                        Line1 = "Deborah Lane",
                        Line2 = "Deborah Lane",
                        PostalCode = "03820",
                        State = "NH"
                    },
                    Email = "nhanth191.3@gmail.com",
                    Dob = new DobOptions
                    {
                        Day = 1,
                        Month = 1,
                        Year = 1990
                    }
                },
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions
                    {
                        Requested = true,
                    },
                    Transfers = new AccountCapabilitiesTransfersOptions
                    {
                        Requested = true,
                    }
                },
            };
            Account acc = await new AccountService().CreateAsync(opt);

            var options = new ExternalAccountCreateOptions
            {
                ExternalAccount = token,
            };
            var service = new ExternalAccountService();
            service.Create(acc.Id, options);

            return await new AccountService().GetAsync(acc.Id);
        }

        public async Task<StripeList<Account>> GetAllBankAccountAsync(string customerId)
        {
            var service = new AccountService();
            return await service.ListAsync();
        }

        public async Task<Payout> CreatePayoutAsync(int amount, string customerId)
        {
            var options = new PayoutCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                Destination = customerId
            };
            return await new PayoutService().CreateAsync(options);
        }

        public async Task<Transfer> CreateTransferAsync(int amount, string accountId)
        {
            var options = new TransferCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                Destination = accountId,
            };
            return await new TransferService().CreateAsync(options);
        }
        //public async Task<Account> CreateAccountAsync()
    }
}
