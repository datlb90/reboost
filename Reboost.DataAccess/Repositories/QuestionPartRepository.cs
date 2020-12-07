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
    }

    public class QuestionPartRepository :  BaseRepository<QuestionParts>, IQuestionPartRepository
    {
        public QuestionPartRepository(ReboostDbContext context)
         : base(context)
        { }
    }
}
