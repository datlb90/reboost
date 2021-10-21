using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;

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
        Task<List<RaterBalances>> GetAllRaterBalanceAsync(string userId);
        Task<List<RaterBalances>> GetRaterPayableBalanceAsync(string userId);
        Task<List<RaterBalances>> UpdatePaidBalancesAsync(string userId);
        Task<LearnerPaymentsHistory> CreatePaymentHistoryAsync(LearnerPaymentsHistory data);
        Task<LearnerSubscriptions> CreateUpdateSubscriptionAsync(LearnerSubscriptions data);
        Task<GetSubscriptionsModel> GetLearnerSubscriptions(string userId);
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

        public async Task<List<RaterBalances>> GetAllRaterBalanceAsync(string userId)
        {
            var rater = await ReboostDbContext.Raters.Where(r => r.UserId == userId).FirstOrDefaultAsync();

            var listBalances = await ReboostDbContext.RaterBalances.Where(b => b.RaterId == rater.Id).ToListAsync();

            return listBalances;
        }

        public async Task<List<RaterBalances>> GetRaterPayableBalanceAsync(string userId)
        {
            var rater = await ReboostDbContext.Raters.Where(r => r.UserId == userId).FirstOrDefaultAsync();

            var listBalances = await ReboostDbContext.RaterBalances.Where(b => b.RaterId == rater.Id && b.Status == RaterBalanceStatus.AVAILABLE).ToListAsync();

            return listBalances;
        }

        public async Task<List<RaterBalances>> UpdatePaidBalancesAsync(string userId)
        {
            var rater = await ReboostDbContext.Raters.Where(r => r.UserId == userId).FirstOrDefaultAsync();

            var listBalances = await ReboostDbContext.RaterBalances.Where(b => b.RaterId == rater.Id && b.Status == RaterBalanceStatus.AVAILABLE).ToListAsync();

            foreach (var b in listBalances)
            {
                    b.Status = RaterBalanceStatus.PAID;
            }
            await ReboostDbContext.SaveChangesAsync();

            return listBalances;
        }

        public async Task<LearnerPaymentsHistory> CreatePaymentHistoryAsync(LearnerPaymentsHistory data)
        {
            await ReboostDbContext.LearnerPaymentsHistory.AddAsync(data);
            await ReboostDbContext.SaveChangesAsync();
            return data;
        }
        public async Task<LearnerSubscriptions> CreateUpdateSubscriptionAsync(LearnerSubscriptions data)
        {
            var existed = await ReboostDbContext.LearnerSubscriptions.Where(s => s.UserId == data.UserId).FirstOrDefaultAsync();

            if (existed != null)
            {
                existed.MonthSubs = data.MonthSubs != null ? data.MonthSubs : existed.MonthSubs;
                existed.YearSubs = data.YearSubs != null ? data.YearSubs : existed.YearSubs;

                await ReboostDbContext.SaveChangesAsync();
                return existed;
            }

            LearnerSubscriptions newSubs = new LearnerSubscriptions
            {
                UserId = data.UserId,
                MonthSubs = data.MonthSubs,
                YearSubs = data.YearSubs,
            };

            await ReboostDbContext.LearnerSubscriptions.AddAsync(newSubs);
            await ReboostDbContext.SaveChangesAsync();
            return newSubs;
        }

        public async Task<GetSubscriptionsModel> GetLearnerSubscriptions(string userId)
        {
            var userSubs = await ReboostDbContext.LearnerSubscriptions.Where(s => s.UserId == userId).FirstOrDefaultAsync();


            GetSubscriptionsModel rs = new GetSubscriptionsModel();
            if (userSubs.YearSubs != null)
            {
                rs.YearSub = userSubs.YearSubs;
                rs.IsYearExpired = false;
            }
            
            if (userSubs.MonthSubs != null)
            {
                rs.MonthSub = userSubs.MonthSubs;
                rs.IsMonthExpired = false;
            }

            return rs;
        }
    }
}
