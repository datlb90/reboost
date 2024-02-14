using System;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface IOrderRepository : IRepository<Orders>
    {

    }
    public class OrderRepository : BaseRepository<Orders>, IOrderRepository
    {
        ReboostDbContext _context;
        private ReboostDbContext ReboostDbContext => context as ReboostDbContext;
        public OrderRepository(ReboostDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

