using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess.Repositories
{
    public interface IRequestQueueRepository : IRepository<RequestQueue>
    {
        Task<RequestQueue> GetTopPriorityRequest();
    }

    public class RequestQueueRepository : Repository<RequestQueue>, IRequestQueueRepository
    {
        public RequestQueueRepository(ReboostDbContext context)
           : base(context)
        { }

        public async Task<RequestQueue> GetTopPriorityRequest()
        {
            //RawSqlString sql = new RawSqlString();
            var request = await ReboostDbContext.RequestQueues
                .FromSqlRaw(@"UPDATE TOP(1) RequestQueue WITH (UPDLOCK, READPAST)
                            SET Status = 1
                            OUTPUT inserted.*
                            FROM RequestQueue
                            WHERE Status = 0").ToListAsync();
            return request.FirstOrDefault();
        }

        private ReboostDbContext ReboostDbContext
        {
            get { return context as ReboostDbContext; }
        }
    }
}
