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
    public interface IQuestionPartRepository : IRepository<QuestionParts>
    {
        Task<List<QuestionParts>> DeleteByQuestionIdAsync(int id);
    }

    public class QuestionPartRepository :  BaseRepository<QuestionParts>, IQuestionPartRepository
    {
        ReboostDbContext db;

        public QuestionPartRepository(ReboostDbContext context)
         : base(context)
        {
            db = context;
        }

        public async Task<List<QuestionParts>> DeleteByQuestionIdAsync(int id)
        {
            var rs = await db.QuestionParts.Where(p => p.QuestionId == id).ToListAsync();
            db.RemoveRange(rs);
            await db.SaveChangesAsync();

            return rs;
        }
    }
}
