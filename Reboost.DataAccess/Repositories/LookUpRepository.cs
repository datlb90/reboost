using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface ILookUpRepository : IRepository<LookUp>
    {
        //Task<List<LookUp>> GetAllAsync();
        //Task<LookUp> GetByIdAsync(int id);
        Task<List<LookUp>> GetByType(string type);
    }
    public class LookUpRepository : Repository<LookUp>, ILookUpRepository
    {
        public LookUpRepository(ReboostDbContext context) : base(context)
        { }

        //public async Task<List<LookUp>> GetAllAsync()
        //{
        //    return await Context.LookUps.ToListAsync();
        //}

        //public async Task<LookUp> GetByIdAsync(int id)
        //{
        //    return await Context.LookUps.Where(r => r.Id == id).FirstAsync();
        //}
        public async Task<List<LookUp>> GetByType(string type)
        {
            return await ReboostDbContext.LookUps.Where(l => l.LookupType == type).ToListAsync();
        }
        private ReboostDbContext ReboostDbContext
        {
            get { return context as ReboostDbContext; }
        }
    }
}
