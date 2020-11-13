using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess.Repositories
{
    public interface IDocumentRepository : IRepository<Documents>
    {
        Task<IEnumerable<Documents>> GetByFileName(string fileName);
        Task<IEnumerable<Documents>> GetByStatus(string status);
    }

    public class DocumentRespository : Repository<Documents>, IDocumentRepository
    {
        public DocumentRespository(ReboostDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Documents>> GetByFileName(string fileName)
        {
            //RawSqlString sql = new RawSqlString();
            var result = ReboostDbContext.Documents.FromSqlRaw(@"SELECT Top(1) Id, RequestId FROM RequestQueue
                WITH(UPDLOCK, HOLDLOCK) WHERE Status = 0 ORDER BY Priority DESC, RequestedDatetime ASC)")
                .FirstOrDefaultAsync();

            return await ReboostDbContext.Documents.Where(d => d.FileName == fileName).ToListAsync();
        }

        public async Task<IEnumerable<Documents>> GetByStatus(string status)
        {
            return await ReboostDbContext.Documents.Where(d => d.Status == status).ToListAsync();
        }

        private ReboostDbContext ReboostDbContext
        {
            get { return context as ReboostDbContext; }
        }
    }
}
