using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface IReviewRepository
    {
        Task<AnnotationModel> GetAnnotationsAsync(int docId, int reviewId);
        Task<IEnumerable<Annotations>> SaveAnnotationsAsync(int docId, int reviewId, IEnumerable<Annotations> annotations);
        Task<Annotations> AddAnnotationAsync( Annotations annotations);
        Task<IEnumerable<InTextComments>> SaveCommentsAsync(IEnumerable<InTextComments> comments);
        Task<String> SaveFeedback(int reviewId, List<ReviewData> data);
        Task<List<ReviewData>> LoadFeedBack(int reviewId);
        Task<InTextComments> AddInTextCommentAsync(InTextComments cmt);
        Task<InTextComments> DeleteInTextCommentAsync(int id);
        Task<int> DeleteAnnotationAsync(int id);
        Task<Annotations> EditAnnotationAsync(Annotations anno);
        Task<InTextComments> EditInTextComment(InTextComments cmt);
        Task<string> CreateNewSampleReviewDocumentAsync(string type, User user);
        Task<List<Reviews>> GetReviewsAsync();
        Task<List<Reviews>> GetReviewsByUserIdAsync(string userId);
        Task<Reviews> ChangeStatusAsync(int id, string newStatus);
        Task<ReviewRequests> CreateRequestAsync(ReviewRequests requests);
        Task<List<GetReviewsModel>> GetReviewRequestsByIdAsync(String userId);
        Task<GetReviewsModel> GetOrCreateReviewByReviewRequestAsync(int requestId, string userId);
    }

    public class ReviewRepository : IReviewRepository
    {
        ReboostDbContext db;
        public ReviewRepository(ReboostDbContext context)
        {
            db = context;
        }

        public async Task<AnnotationModel> GetAnnotationsAsync(int docId, int reviewId)
        {
            var annotations = await db.Annotations.AsNoTracking().Where(a => a.DocumentId == docId && a.ReviewId == reviewId).ToListAsync();
            var comments = db.InTextComments.AsNoTracking().Where(c => annotations.Select(a => a.Id).Contains(c.AnnotationId));
            return new AnnotationModel { 
                Annotations = annotations,
                Comments = comments
            };
        }

        public async Task<IEnumerable<Annotations>> SaveAnnotationsAsync(int docId, int reviewId, IEnumerable<Annotations> annotations)
        {
            var currentAnots = db.Annotations.Where(a => a.DocumentId == docId && a.ReviewId == reviewId);
            var annotIds = currentAnots.Select(c => c.Id);
            var currentComments = db.InTextComments.Where(c => annotIds.Contains(c.AnnotationId));
            db.InTextComments.RemoveRange(currentComments);
            //await db.SaveChangesAsync();

            db.Annotations.RemoveRange(currentAnots);

            await db.Annotations.AddRangeAsync(annotations);
            await db.SaveChangesAsync();

            return await Task.FromResult(annotations);
        }
        public async Task<IEnumerable<InTextComments>> SaveCommentsAsync(IEnumerable<InTextComments> comments) {
            var annotIds = comments.Select(c => c.AnnotationId);
            var currentComments = db.InTextComments.Where(c => annotIds.Contains(c.AnnotationId));

            db.RemoveRange(currentComments);
            await db.AddRangeAsync(comments);
            await db.SaveChangesAsync();

            return await Task.FromResult(comments);
        }
        public async Task<String> SaveFeedback(int reviewId,List<ReviewData> data)
        {
            Reviews review = await db.Reviews.FindAsync(reviewId);
            if (review != null)
            {
                ReviewRequests requests = await db.ReviewRequests.Where(rq => rq.Id == review.RequestId).FirstOrDefaultAsync();
                if (requests != null)
                {
                    requests.Status = "Completed";
                    requests.CompletedDateTime = DateTime.Now;
                }
                db.ReviewData.AddRange(data);
                await db.SaveChangesAsync();
                return null;
            }
            else
            {
                return "Current Review not existed";
            }
        }
        public async Task<List<ReviewData>> LoadFeedBack(int reviewId)
        {
            return await db.ReviewData.Where(rds => rds.ReviewId == reviewId).ToListAsync();
        }
        public async Task<Annotations> AddAnnotationAsync(Annotations annotation)
        {
            await db.Annotations.AddAsync(annotation);
            await db.SaveChangesAsync();

            return await Task.FromResult(annotation);
        }
        public async Task<InTextComments> AddInTextCommentAsync(InTextComments cmt)
        {
            await db.InTextComments.AddAsync(cmt);
            await db.SaveChangesAsync();
            return await Task.FromResult(cmt);
        }
        public async Task<InTextComments> DeleteInTextCommentAsync(int id)
        {
            InTextComments rs = await db.InTextComments.FindAsync(id);
            if (rs != null )
            {

                Annotations anno = await db.Annotations.FindAsync(rs.AnnotationId);
                db.InTextComments.Remove(rs);
                db.Annotations.Remove(anno);

                await db.SaveChangesAsync();
            }
            return await Task.FromResult(rs);
        }
        public async Task<Annotations> EditAnnotationAsync(Annotations anno)
        {
            Annotations annotations = await db.Annotations.FindAsync(anno.Id);
            db.Annotations.Remove(annotations);
            await db.Annotations.AddAsync(anno);
            await db.SaveChangesAsync();
            return await Task.FromResult(anno);
        }
        public async Task<int> DeleteAnnotationAsync(int id)
        {
            Annotations annotations = await db.Annotations.FindAsync(id);
            if (annotations!=null)
            {
                var comments = db.InTextComments.Where(c => c.AnnotationId == id);
                db.InTextComments.RemoveRange(comments);
                db.Annotations.Remove(annotations);
                await db.SaveChangesAsync();
            }
            return await Task.FromResult(id);
        }
        public async Task<InTextComments> EditInTextComment(InTextComments cmt)
        {
            InTextComments inTextComments = await db.InTextComments.FindAsync(cmt.Id);
            db.InTextComments.Remove(inTextComments);
            await db.InTextComments.AddAsync(cmt);
            await db.SaveChangesAsync();
            return await Task.FromResult(cmt);
        }
        public async Task<List<Reviews>> GetReviewsAsync()
        {
            List<Reviews> list = await db.Reviews.Include("ReviewData").ToListAsync();
            return await Task.FromResult(list);
        }
        public async Task<Reviews> ChangeStatusAsync(int id, string newStatus)
        {
            Reviews rv = await db.Reviews.FindAsync(id);
            rv.Status = newStatus;
            
            List<Reviews> rvs = await db.Reviews.Where(r => r.ReviewerId == rv.ReviewerId).ToListAsync();
            bool flag = true;

            Raters rater = await db.Raters.Where(r => r.UserId == rv.ReviewerId).FirstOrDefaultAsync();

            RaterRepository _raterRepository = new RaterRepository(db); 
            List<string> trainingCount = await _raterRepository.GetApplyTo(rater.Id);

            foreach (Reviews r in rvs)
            {
                if (!r.Status.Contains(RaterStatus.APPROVED))
                {
                    flag = false;
                    break;
                }
            }
            if (flag && newStatus.Contains(RaterStatus.APPROVED) )
            {
                if(trainingCount.Count == rvs.Count)
                {
                    rater.Status = RaterStatus.APPROVED;
                }
                else
                {
                    rater.Status = RaterStatus.TRANING;
                }
            }

            await db.SaveChangesAsync();
            return await Task.FromResult(rv);
        }
        public async Task<string> CreateNewSampleReviewDocumentAsync(string type, User user)
        {
            RaterRepository _raterRepository = new RaterRepository(db);
            Raters rater = await db.Raters.Where(r => r.UserId == user.Id).FirstOrDefaultAsync();
            List<String> applyTo = await _raterRepository.GetApplyTo(rater.Id);

            if (!applyTo.Contains(type.ToUpper()))
            {
                return await Task.FromResult("failed");
            }

            string status = type.ToUpper() + RaterStatus.TRANING;
            string approvedstatus = status + RaterStatus.APPROVED;
            Reviews existed = await db.Reviews.Where(rv => rv.ReviewerId == user.Id && (rv.Status == approvedstatus || rv.Status == status) ).FirstOrDefaultAsync();
            if(existed!= null)
            {
                return await Task.FromResult(existed.Id.ToString());
            };

            DateTime currentTime = DateTime.Now;

            Reviews rv = new Reviews { RequestId = 1, FinalScore = 0, ReviewerId = user.Id, RevieweeId = "1", Status = status, TimeSpentInSeconds = 0, LastActivityDate = currentTime };
            
            await db.Reviews.AddAsync(rv);
            await db.SaveChangesAsync();
      
            return await Task.FromResult(rv.Id.ToString());
        }
        public async Task<List<Reviews>> GetReviewsByUserIdAsync(string userId)
        {
            List<Reviews> list = await db.Reviews.Include("ReviewData").Where(rv => rv.ReviewerId == userId).ToListAsync();
            return await Task.FromResult(list);
        }

        public async Task<ReviewRequests> CreateRequestAsync(ReviewRequests request)
        {
            await db.ReviewRequests.AddAsync(request);
            await db.SaveChangesAsync();
            return await Task.FromResult(request);
        }

        public async Task<List<GetReviewsModel>> GetReviewRequestsByIdAsync(String userId)
        {
            var query = await (from rr in db.ReviewRequests
                               join q in db.Questions on rr.Submission.QuestionId equals q.Id
                               join ts in db.TestSections on q.Task.SectionId equals ts.Id

                               select new GetReviewsModel
                               {
                                   ReviewRequest = rr,
                                   QuestionName = q.Title,
                                   QuestionType = q.Type,
                                   TestSection = ts.Name
                               }).ToListAsync();

            return await Task.FromResult(query);
        }

        public async Task<GetReviewsModel> GetOrCreateReviewByReviewRequestAsync(int requestId, string userId)
        {
            GetReviewsModel rs = new GetReviewsModel();

            ReviewRequests reviewRequests = await db.ReviewRequests.Include("Submission").Where(r=>r.Id == requestId).FirstOrDefaultAsync();
            Reviews existed = await db.Reviews.Where(r => r.RequestId == requestId && r.ReviewerId == userId && r.RevieweeId == reviewRequests.UserId).FirstOrDefaultAsync();

            rs.ReviewRequest = reviewRequests;

            if (existed != null)
            {
                rs.ReviewId = existed.Id;
                return await Task.FromResult(rs);
            };

            DateTime currentTime = DateTime.Now;

            Reviews rv = new Reviews { RequestId = requestId, FinalScore = 0, ReviewerId = userId, RevieweeId = reviewRequests.UserId, Status = "In Progress", TimeSpentInSeconds = 0, LastActivityDate = currentTime };

            await db.Reviews.AddAsync(rv);
            await db.SaveChangesAsync();

            rs.ReviewId = rv.Id;

            return await Task.FromResult(rs);
        }
    }
}
