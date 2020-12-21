using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface IRaterCredentialRepository : IRepository<RaterCredentials>
    {
        Task<int> UpdateManyByRaterAync(int raterId, List<RaterCredentials> credentials);
    }
    public class RaterCredentialRepository: BaseRepository<RaterCredentials>, IRaterCredentialRepository
    {
        private ReboostDbContext db => context as ReboostDbContext;

        public RaterCredentialRepository(ReboostDbContext context) : base(context)
        { }

        public async Task<int> UpdateManyByRaterAync(int raterId, List<RaterCredentials> credentials) {
            var currentCredentials = db.RaterCredentials.AsNoTracking().Where(c => c.RaterId == raterId);
            db.RaterCredentials.RemoveRange(currentCredentials);

            db.RaterCredentials.AddRange(credentials);
            return await db.SaveChangesAsync();
        }
    }
}
