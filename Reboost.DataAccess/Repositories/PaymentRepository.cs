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
    public interface IPaymentRepository : IRepository<Payments>
    {
        Task<Payments> GetDetailByPaymentId(string id);
        Task<List<Payments>> GetAllPaymentsByUserId(string id);
    }
    public class PaymentRepository : BaseRepository<Payments>, IPaymentRepository
    {
        ReboostDbContext _context;
        private ReboostDbContext ReboostDbContext => context as ReboostDbContext;
        public PaymentRepository(ReboostDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Payments> GetDetailByPaymentId(string id)
        {
            return await (from q in ReboostDbContext.Payments
                          where q.PaymentId == id
                          select q).FirstOrDefaultAsync();
        }
        public async Task<List<Payments>> GetAllPaymentsByUserId(string id)
        {
            return await (from q in ReboostDbContext.Payments
                          where q.UserId == id
                          select q).ToListAsync();
        }
    }
}
