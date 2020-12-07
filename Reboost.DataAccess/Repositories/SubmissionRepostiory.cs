using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Repositories
{
    public interface ISubmissionRepository : IRepository<Submissions>
    {
        
    }
    public class SubmissionRepository: BaseRepository<Submissions>, ISubmissionRepository
    {
        private ReboostDbContext mContext => context as ReboostDbContext;

        public SubmissionRepository(ReboostDbContext context) : base(context)
        {
            
        }
    }
}
