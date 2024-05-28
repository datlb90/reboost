using System;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace Reboost.DataAccess.Repositories
{
    public interface IReviewScoreRepository : IRepository<ReviewScores>
    {
        Task<ReviewScores> GetEssayScores(int reviewId);
    }

    public class ReviewScoreRepository : BaseRepository<ReviewScores>, IReviewScoreRepository
    {
        ReboostDbContext _context;

        private ReboostDbContext ReboostDbContext => context as ReboostDbContext;

        public ReviewScoreRepository(ReboostDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ReviewScores> GetEssayScores(int reviewId)
        {
            return await ReboostDbContext.ReviewScores.Where(s => s.ReviewId == reviewId).FirstOrDefaultAsync();
        }

    }
}

