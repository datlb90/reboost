using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Repositories
{
    public abstract class BaseRepository
    {
        public ReboostDbContext Context; 
        public BaseRepository(ReboostDbContext context)
        {
            Context = context;
        }
    }
}
