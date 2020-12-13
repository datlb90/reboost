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
    public interface IRubricRepository : IRepository<Rubrics>
    {
        Task<Rubrics> GetByIdAsync(int id);
        Task<Rubrics> UpdateWithCriteraAsync(Rubrics rubrics);
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
    }
}
