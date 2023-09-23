using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;

namespace Reboost.DataAccess.Repositories
{
    public interface IDocumentRepository : IRepository<Documents>
    {
        Task<IEnumerable<Documents>> GetByFileName(string fileName);
        Task<IEnumerable<Documents>> GetByStatus(string status);
        Task<IEnumerable<Documents>> SearchByUser(string userId, int questionId);
        Task<Documents> GetSubmissionByIdAsync(int id);
        Task<Documents> UpdateDocumentBySubmissionId(int id, DocumentRequestModel data);
        Task<Documents> GetSavedDocument(string userId, int questionId);
    }

    public class DocumentRespository : BaseRepository<Documents>, IDocumentRepository
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

            return await ReboostDbContext.Documents.Where(d => d.Filename == fileName).ToListAsync();
        }

        public async Task<IEnumerable<Documents>> GetByStatus(string status)
        {
            return await ReboostDbContext.Documents.Where(d => d.Status == status).ToListAsync();
        }
        public async Task<IEnumerable<Documents>> SearchByUser(string userId, int questionId)
        {
            var submission = await ReboostDbContext.Submissions.Where(s => s.UserId == userId && s.QuestionId == questionId).FirstOrDefaultAsync();

            var rs = await (from doc in ReboostDbContext.Documents
                         join sub in ReboostDbContext.Submissions on doc.Id equals sub.DocId
                         where sub.UserId == userId && sub.QuestionId == questionId
                         orderby sub.SubmittedDate descending
                         select doc).ToListAsync();
            foreach(Documents d in rs)
            {
                d.Submissions.Add(submission);
            }
            return rs;
        }
        public async Task<Documents> GetSubmissionByIdAsync(int id)
        {
            var submission = await ReboostDbContext.Submissions.FindAsync(id);

            var rs = await ReboostDbContext.Documents.FindAsync(submission.DocId);
            return rs;

        }
        public async Task<Documents> UpdateDocumentBySubmissionId(int id, DocumentRequestModel data)
        {
            var submission = await ReboostDbContext.Submissions.FindAsync(id);
            var doc = await ReboostDbContext.Documents.FindAsync(submission.DocId);

            submission.TimeSpentInSeconds = data.TimeSpentInSeconds;
            submission.Status = data.Status;
            submission.UpdatedDate = DateTime.Now;

            doc.Text = data.Text;
            doc.Filename = data.Filename;
            doc.Data = data.Data;

            await ReboostDbContext.SaveChangesAsync();

            return data;
        }
        public async Task<Documents> GetSavedDocument(string userId, int questionId)
        {
            var submission = await ReboostDbContext.Submissions.Where(s => s.UserId == userId && s.QuestionId == questionId && s.Status == "Saved").FirstOrDefaultAsync();
            if(submission != null)
            {
                var doc = await ReboostDbContext.Documents.FindAsync(submission.DocId);
                doc.Submissions.Add(submission);
                return doc;
            }
            return null;
        }
        private ReboostDbContext ReboostDbContext
        {
            get { return context as ReboostDbContext; }
        }
        
    }
}
