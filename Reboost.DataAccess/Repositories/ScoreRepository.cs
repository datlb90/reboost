using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface IScoreRepository: IRepository<UserScore>
    {
        //Task<List<UserScore>> GetAllAsync();
        //Task<UserScore> GetByIdAsync(int id);
        //Task<UserScore> CreateAsync(UserScore score);
        //Task<UserScore> UpdateAsync(UserScore score);
        //Task<UserScore> DeleteAsync(int id);
    }
    public class ScoreRepository : Repository<UserScore>, IScoreRepository
    {
        public ScoreRepository(ReboostDbContext context) : base(context)
        { }

        //public async Task<UserScore> CreateAsync(UserScore score)
        //{
        //    Context.Scores.Add(score);
        //    await Context.SaveChangesAsync();
        //    return score;
        //}

        //public async Task<UserScore> DeleteAsync(int id)
        //{
        //    UserScore _score = await Context.Scores.SingleOrDefaultAsync(r => r.Id == id);
        //    Context.Scores.Remove(_score);
        //    await Context.SaveChangesAsync();
        //    return _score;
        //}

        //public async Task<List<UserScore>> GetAllAsync()
        //{
        //    return await Context.Scores.ToListAsync();
        //}

        //public async Task<UserScore> GetByIdAsync(int id)
        //{
        //    return await Context.Scores.SingleOrDefaultAsync(r => r.Id == id);
        //}

        //public async Task<UserScore> UpdateAsync(UserScore score)
        //{
        //    UserScore _score = await Context.Scores.SingleOrDefaultAsync(r => r.Id == score.Id);
        //    _score.UserId = score.UserId;
        //    _score.SectionId = score.SectionId;
        //    _score.Score = score.Score;
        //    _score.Subject = score.Score;
        //    _score.Skill = 
        //    _score.UpdatedDate = score.UpdatedDate;
        //    await Context.SaveChangesAsync();
        //    return _score;
        //}
    }
}
