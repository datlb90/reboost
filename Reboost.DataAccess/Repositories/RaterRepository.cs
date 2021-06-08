using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess.Repositories
{
    public interface IRaterRepository: IRepository<Raters>
    {
        //Task<List<Tasks>> GetTaskAsync();
        //Task<Dictionary<string, int>> CountQuestByTaskAsync();
        //Task<List<QuestionModel>> GetAllExAsync();
        //Task<QuestionModel> GetByIdAsync(int id);
        //Task<Raters> GetByRaterIdCredentialsAsync(int id);
        Task<Raters> GetDetailByIdAsync(int id);
        Task<Raters> GetByUserIdAsync(string userId);
        Task<List<string>> GetApplyTo(int raterId);
        Task<Raters> UpdateWithCredentialAsync(Raters rater);
        Task<decimal> GetRaterRatingAsync(string UserID);
    }
    public class RaterRepository : BaseRepository<Raters>, IRaterRepository
    {
        private ReboostDbContext ReboostDbContext => context as ReboostDbContext;

        public RaterRepository(ReboostDbContext context) : base(context)
        { }

        public async Task<Raters> GetDetailByIdAsync(int id)
        {
            //return await ReboostDbContext.Raters
            //    .Include(r => r.RaterCredentials)
            //    .Include(r => r.User).ThenInclude(u => u.UserScores)
            //    .AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

            //var data = await (from r in ReboostDbContext.Raters
            //              join rc in ReboostDbContext.RaterCredentials on r.Id equals rc.RaterId
            //              join u in ReboostDbContext.Users on r.UserId equals u.Id
            //              join sc in ReboostDbContext.UserScores on u.Id equals sc.UserId
            //              where r.Id == id
            //              select new Raters { 
            //                  RaterCredentials = 
            //              })

            //             .FirstOrDefaultAsync();

            var rater = await (from r in ReboostDbContext.Raters
                            where r.Id == id
                            select r)
                            .FirstOrDefaultAsync();

            var credentials = ReboostDbContext.RaterCredentials.AsNoTracking().Where(c => c.RaterId == id).ToList();
            rater.RaterCredentials = credentials;

            var user = ReboostDbContext.Users.AsNoTracking().Where(u => u.Id == rater.UserId).FirstOrDefault();
            var scores = ReboostDbContext.UserScores.AsNoTracking().Where(s => s.UserId == user.Id).ToList();
            user.UserScores = scores;
            rater.User = user;

            return rater;
        }

        public async Task<List<string>> GetApplyTo(int raterId)
        {
            return await (from r in ReboostDbContext.Raters
                          join u in ReboostDbContext.Users on r.UserId equals u.Id
                          join sc in ReboostDbContext.UserScores on u.Id equals sc.UserId
                          join ss in ReboostDbContext.TestSections on sc.SectionId equals ss.Id
                          join t in ReboostDbContext.Tests on ss.TestId equals t.Id
                          where r.Id == raterId
                          group t by t.Name into g
                          select g.Key).ToListAsync();
        }

        public async Task<Raters> GetByUserIdAsync(string userId)
        {
            return await ReboostDbContext.Raters.FirstOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task<Raters> UpdateWithCredentialAsync(Raters rater)
        {
            var credentials = ReboostDbContext.RaterCredentials.Where(c => c.RaterId == rater.Id);
            ReboostDbContext.RemoveRange(credentials);
            await ReboostDbContext.SaveChangesAsync();

            rater.RaterCredentials = null;

            var rs = await Update(rater);

            return rs;
        }

        public async Task<decimal> GetRaterRatingAsync(string UserID)
        {
            var rs = await (from rr in ReboostDbContext.ReviewRatings
                            join r in ReboostDbContext.Reviews on rr.ReviewId equals r.Id
                            where r.ReviewerId == UserID
                            select rr).ToListAsync();
            if (rs.Count > 0)
            {
                decimal total = 0;
                foreach (ReviewRatings r in rs)
                {
                    total += r.Rate;
                }

                return (total / (rs.Count));
            }
            return 0;
        }
    }
}
