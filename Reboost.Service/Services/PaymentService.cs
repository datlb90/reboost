using Microsoft.AspNetCore.Http;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    }
    public class PaymentService :BaseService, IPaymentService
    {
        private IStripeService mStripeService;
        public PaymentService(IUnitOfWork unitOfWork, IStripeService stripeService): base(unitOfWork)
        {
            mStripeService = stripeService;
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
    }
}
