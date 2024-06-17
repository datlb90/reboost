using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface ISubmissionRepository : IRepository<Submissions>
    {
        Task<InitialSubmissionModel> GetInitialSubmission(string userId);
    }

    public class SubmissionRepository: BaseRepository<Submissions>, ISubmissionRepository
    {
        ReboostDbContext db;

        public SubmissionRepository(ReboostDbContext context) : base(context)
        {
            db = context;
        }

        public async Task<InitialSubmissionModel> GetInitialSubmission(string userId)
        {
            var submission =  await db.Submissions.Where(s => s.UserId == userId && (s.Type == "initial.vn" || s.Type == "initial.en")).FirstOrDefaultAsync();
            if(submission != null)
            {
                InitialSubmissionModel result = new InitialSubmissionModel
                {
                    id = submission.Id,
                    feedbackLanguage = submission.Type == "initial.vn" ? "vn" : "en"
                };
                return result;
            }
            return null;
        }

    }
}
