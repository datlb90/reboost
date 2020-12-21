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
    public interface IRubricRepository : IRepository<Rubrics>
    {
        Task<Rubrics> GetByIdAsync(int id);
        Task<Rubrics> UpdateWithCriteraAsync(Rubrics rubrics);
        Task<List<RubricsModel>> getByQuestionId(int id);
    }
    public class RubricRepository : BaseRepository<Rubrics>, IRubricRepository
    {
        private ReboostDbContext ReboostDbContext => context as ReboostDbContext;
        public RubricRepository(ReboostDbContext context) : base(context)
        { }
        public async Task<Rubrics> GetByIdAsync(int id)
        {
            return await ReboostDbContext.Rubrics
                .Include(r => r.RubricCriteria)
                .Include(r => r.RubricCriteria).ThenInclude(u => u.RubricMilestone)
                .AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Rubrics> UpdateWithCriteraAsync(Rubrics rubrics)
        {
            var creterias = ReboostDbContext.RubricCriteria.Where(c => c.RubricId == rubrics.Id);
            ReboostDbContext.RemoveRange(creterias);
            await ReboostDbContext.SaveChangesAsync();
            return await Update(rubrics);
        }

        public async Task<List<RubricsModel>> getByQuestionId(int id)
        {
            var query = await (from q in ReboostDbContext.Questions
                               join t in ReboostDbContext.Tasks on q.TaskId equals t.Id
                               join r in ReboostDbContext.Rubrics on t.Id equals r.TaskId
                               join rc in ReboostDbContext.RubricCriteria on r.Id equals rc.RubricId
                               join rm in ReboostDbContext.RubricMilestones on rc.Id equals rm.CriteriaId
                               where q.Id == id
                               select new RubricsQuery
                               {
                                   Id = q.Id,
                                   Name = rc.Name,
                                   BandScore = rm.BandScore,
                                   Description = rm.Description
                               }).ToListAsync();

            var group = query.GroupBy(q => q.Name).Select(g => new RubricsModel
            {
                Name = g.Key,
                BandScoreDescriptions = g.Select(d => new BandScoreDescription
                {
                    BandScore = d.BandScore,
                    Description = d.Description
                }).ToList()
            }).ToList();

            return group;
        }
    }
}
