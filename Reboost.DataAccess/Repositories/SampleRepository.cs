using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface ISampleRepository : IRepository<Samples>
    {
        Task<Samples> ApproveSampleByIdAsync(int id);
    }

    public class SampleRepository : BaseRepository<Samples>, ISampleRepository
    {
        ReboostDbContext db;
        public SampleRepository(ReboostDbContext context) : base(context)
        {
            db = context;
        }

        public async Task<Samples> ApproveSampleByIdAsync(int id)
        {
            var sample = await db.Samples.FindAsync(id);
            sample.Status = SampleStatus.APPROVED;

            await db.SaveChangesAsync();
            return sample;
        }
    }
}
