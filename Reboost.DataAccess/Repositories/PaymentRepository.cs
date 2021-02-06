using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;

namespace Reboost.DataAccess.Repositories
{
    public interface IPaymentRepository : IRepository<Payments>
    {
        Task<Payments> GetDetailByPaymentId(string id);
        Task<List<Payments>> GetAllPaymentsByUserId(string id);
        Task<UserStripeAccounts> UpdateUserStripeAccounts(UserStripeAccounts acct);
        Task<UserStripeAccounts> GetAccount(string userId);
        Task<IEnumerable<Payments>> GetOutPaymentByUserId(string userId);
        //Task<IEnumerable<Payments>> GetPaymentByUser(string userId);
        //Task<List<PaymentHistory>> GetAllPaymentHistory();
        //Task<PaymentHistory> CreateNewPaymentHistory(PaymentHistory ph);
    }
    public class PaymentRepository : BaseRepository<Payments>, IPaymentRepository
    {
        ReboostDbContext _context;
        private ReboostDbContext ReboostDbContext => context as ReboostDbContext;
        public PaymentRepository(ReboostDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Payments> GetDetailByPaymentId(string id)
        {
            return await (from q in ReboostDbContext.Payments
                          where q.PaymentId == id
                          select q).FirstOrDefaultAsync();
        }
        public async Task<List<Payments>> GetAllPaymentsByUserId(string id)
        {
            return await (from q in ReboostDbContext.Payments
                          where q.UserId == id
                          select q).ToListAsync();
        }
        public async Task<UserStripeAccounts> UpdateUserStripeAccounts(UserStripeAccounts acct)
        {
            await ReboostDbContext.UserStripeAccounts.AddAsync(acct);
            await ReboostDbContext.SaveChangesAsync();
            return acct;
        }
        public async Task<UserStripeAccounts> GetAccount(string userId)
        {
            return await (from q in ReboostDbContext.UserStripeAccounts
                          where q.UserId == userId
                          select q).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Payments>> GetOutPaymentByUserId(string userId) {
            return await ReboostDbContext.Payments.AsNoTracking().Where(p => p.UserId == userId && p.Type == "OUT").ToListAsync();
        }
        //public async Task<IEnumerable<Payments>> GetPaymentByUser(string userId) {
        //    return await ReboostDbContext.Payments.AsNoTracking().Where(p => p.UserId == userId).ToListAsync();
        //}
        //public async Task<List<PaymentHistory>> GetAllPaymentHistory()
        //{
        //    return await (from p in ReboostDbContext.PaymentHistory
        //                  select p).ToListAsync();
        //}
        //public async Task<PaymentHistory> CreateNewPaymentHistory(PaymentHistory ph)
        //{
        //    await ReboostDbContext.PaymentHistory.AddAsync(ph);
        //    await ReboostDbContext.SaveChangesAsync();
        //    return ph;
        //}
    }
}
