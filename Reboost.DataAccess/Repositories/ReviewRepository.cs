using Microsoft.AspNetCore.Identity;
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
        Task<Reviews> SaveFeedback(int reviewId, List<ReviewData> data);
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
        Task<ReviewRequests> CreateRequestAsync(string userId, ReviewRequests requests);
        Task<List<GetReviewsModel>> GetRaterReviewsByIdAsync(String userId);
        Task<GetReviewsModel> GetOrCreateReviewByReviewRequestAsync(int requestId, string userId);
        Task<int> CheckUserReviewValidationAsync(string role, User user, int reviewId);
        Task<ReviewRequests> GetReviewRequestBySubmissionId(int submissionId, string userId);
        Task<int> CheckReviewValidationAsync(string role, User user, int reviewId);
        Task<ReviewRatings> CreateReviewRatingAsync(ReviewRatings data, string raterId);
        Task<ReviewRatings> GetReviewRatingsByReviewIdAsync(int reviewId);
        Task<RequestQueue> AddRequestQueue(RequestQueue data, string userID);
        Task<GetReviewsModel> CreateReviewFromQueue(string userID);
        Task<List<Reviews>> GetRatedReviewsAsync(string userId);
        Task<GetReviewsModel> GetPendingReviewAsync(string userId);
        Task<Reviews> GetReviewByIdAsync(int id);
        Task<IEnumerable<Reviews>> GetUnRatedReviewOfUser(string userId);
        Task<ReviewRatings> SubmitReviewRatingAsync(ReviewRatings data, string userId);
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
                Comments = comments,
                User = null
            };
        }

        public async Task<int> CheckUserReviewValidationAsync(string role, User user,int reviewId)
        {
            var review = await db.Reviews.FindAsync(reviewId);

            if (review== null)
            {
                return 0;
            }

            if (role == "Admin")
            {
                return 1;
            }

            if (review.ReviewerId == user.Id || review.RevieweeId == user.Id)
            {
                return 1;
            }

            return -1;
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
        public async Task<Reviews> SaveFeedback(int reviewId,List<ReviewData> data)
        {
            Reviews review = await db.Reviews.FindAsync(reviewId);
            if (review == null)
            {
                return null;
            }

            db.ReviewData.AddRange(data);

            if ((review.Status.Contains(TestsName.IELTS)|| review.Status.Contains(TestsName.TOEFL)) && !review.Status.Contains(RaterStatus.REVISION))
            {
                if (!review.Status.Contains(RaterReviewsTrainingStatus.Submitted))
                {
                    review.Status += RaterReviewsTrainingStatus.Submitted;

                    RaterRepository _raterRepository = new RaterRepository(db);
                    Raters rater = await _raterRepository.GetByUserIdAsync(review.ReviewerId);
                    List<string> trainingCount = await _raterRepository.GetApplyTo(rater.Id);

                    List<Reviews> rvs = await db.Reviews.Where(r => r.ReviewerId == review.ReviewerId && r.Status.Contains("Training")).ToListAsync();

                    if(rvs.Count() == trainingCount.Count())
                    {
                        rater.Status = RaterStatus.TRAININGCOMPLETED;
                    }

                }
                await db.SaveChangesAsync();
                return review;
            }

            ReviewRequests request = await db.ReviewRequests.Where(rq => rq.Id == review.RequestId).FirstOrDefaultAsync();

            if (review.Status.Contains(RaterStatus.REVISION))
            {
                var index = review.Status.IndexOf(RaterStatus.REVISION);
                review.Status = review.Status.Substring(0, index) + RaterStatus.REVISIONCOMPLETED;
                int completedCount = 0;

                Raters rater = await db.Raters.Where(r => r.UserId == review.ReviewerId).FirstOrDefaultAsync();

                RaterRepository _raterRepository = new RaterRepository(db);
                List<Reviews> listUserReview = await GetReviewsByUserIdAsync(rater.UserId);
                var trainingCount = listUserReview.Count(rater => rater.Status.Contains(RaterStatus.REVISION));

                foreach (Reviews r in listUserReview)
                {
                    if (r.Status.Contains(RaterStatus.REVISIONCOMPLETED))
                    {
                        completedCount += 1;
                    }
                    else
                    {
                        if(r.Id == reviewId)
                        {
                            completedCount += 1;
                        }
                    }
                }
                if(completedCount == trainingCount)
                {
                    rater.Status = RaterStatus.REVISIONCOMPLETED;
                }
            }
            else
            {
                review.Status = ReviewRequestStatus.COMPLETED;
                review.LastActivityDate = DateTime.Now;
            }

            if (request != null)
            {
                //Update submission status to Reviewed
                var submission = db.Submissions.Where(s => s.Id == request.SubmissionId).FirstOrDefault();
                if(submission != null)
                {
                    submission.UpdatedDate = DateTime.Now;
                    submission.Status = SubmissionStatus.REVIEWED;
                }

                request.Status = ReviewRequestStatus.COMPLETED;
                request.CompletedDateTime = DateTime.Now;
            }

            await db.SaveChangesAsync();
            return review;
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


            
            List<Reviews> rvs = await db.Reviews.Where(r => r.ReviewerId == rv.ReviewerId && r.Status.Contains("Training")).ToListAsync();
            bool approvedFlag = true;
            bool isRevision = false;

            Raters rater = await db.Raters.Where(r => r.UserId == rv.ReviewerId).FirstOrDefaultAsync();

            RaterRepository _raterRepository = new RaterRepository(db); 
            List<string> trainingCount = await _raterRepository.GetApplyTo(rater.Id);

            foreach (Reviews r in rvs)
            {
                if (!r.Status.Contains(RaterStatus.APPROVED))
                {
                    if (r.Status.Contains(RaterStatus.REVISION)&&!r.Status.Contains(RaterStatus.REVISIONCOMPLETED))
                    {
                        isRevision = true;
                    }
                    approvedFlag = false;
                    break;
                }
            }

            if (approvedFlag && newStatus.Contains(RaterStatus.APPROVED))
            {
                if(trainingCount.Count == rvs.Count)
                {
                    rater.Status = RaterStatus.APPROVED;
                }
                else
                {
                    rater.Status = isRevision ? RaterStatus.REVISIONREQUESTED : RaterStatus.TRAINING;
                }
            }

            if (newStatus.Contains(RaterStatus.REVISION))
            {
                rater.Status = RaterStatus.REVISIONREQUESTED;
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

            string status = type.ToUpper() + RaterStatus.TRAINING;

            Reviews existed = await db.Reviews.Where(rv => rv.ReviewerId == user.Id && (rv.Status.Contains(status))).FirstOrDefaultAsync();
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

        public async Task<ReviewRequests> CreateRequestAsync(string userId, ReviewRequests request)
        {
            Submissions sub = await db.Submissions.Where(s => s.Id == request.SubmissionId).FirstOrDefaultAsync();

            var unratedList = await GetUnRatedReviewOfUser(userId);

            sub.Status = unratedList.Count() > 0 ? SubmissionStatus.PENDING : ReviewRequestStatus.REVIEW_REQUETED;

            await db.ReviewRequests.AddAsync(request);
            await db.SaveChangesAsync();
            return await Task.FromResult(request);
        }

        public async Task<List<GetReviewsModel>> GetRaterReviewsByIdAsync(String userId)
        {
            var query = await (from rr in db.ReviewRequests
                               join rv in db.Reviews on rr.Id equals rv.RequestId
                               join q in db.Questions on rr.Submission.QuestionId equals q.Id
                               join s in db.Submissions on rr.SubmissionId equals s.Id
                               join ts in db.TestSections on q.Task.SectionId equals ts.Id
                               orderby rv.LastActivityDate descending
                               where rv.ReviewerId == userId
                               select new GetReviewsModel
                               {
                                   ReviewRequest = rr,
                                   Submission = s,
                                   Review =rv,
                                   ReviewId = rv.Id,
                                   QuestionName = q.Title,
                                   QuestionType = q.Type,
                                   TestSection = ts.Name
                               }).ToListAsync();

            return await Task.FromResult(query);
        }

        public async Task<GetReviewsModel> GetOrCreateReviewByReviewRequestAsync(int requestId, string userId)
        {
            GetReviewsModel rs = new GetReviewsModel();

            ReviewRequests reviewRequest = await db.ReviewRequests.Include("Submission").Where(r=>r.Id == requestId).FirstOrDefaultAsync();
            Reviews existed = await db.Reviews.Where(r => r.RequestId == requestId && r.RevieweeId == reviewRequest.UserId).FirstOrDefaultAsync();

            rs.ReviewRequest = reviewRequest;

            if (existed != null)
            {
                rs.ReviewId = existed.Id;
                return await Task.FromResult(rs);
            };

            DateTime currentTime = DateTime.Now;

            Reviews rv = new Reviews { RequestId = requestId, FinalScore = 0, ReviewerId = userId, RevieweeId = reviewRequest.UserId, Status = "In Progress", TimeSpentInSeconds = 0, LastActivityDate = currentTime };

            await db.Reviews.AddAsync(rv);
            await db.SaveChangesAsync();

            rs.ReviewId = rv.Id;

            return await Task.FromResult(rs);
        }

        public async Task<ReviewRequests> GetReviewRequestBySubmissionId(int submissionId, string userId)
        {
            return await db.ReviewRequests.Where(r => r.SubmissionId == submissionId && r.UserId == userId).FirstOrDefaultAsync();
        }
        public async Task<int> CheckReviewValidationAsync(string role, User user, int reviewId)
        {
            var exist = await (from r in db.Reviews
                               where ((r.RevieweeId == user.Id || r.ReviewerId == user.Id) && r.Id == reviewId)
                               select r).FirstOrDefaultAsync();
            if(role == "Admin")
            {
                return 2;
            }
            if (exist?.Id!=null)
            {
                if (exist.Status == ReviewRequestStatus.COMPLETED || exist.Status == ReviewRequestStatus.RATED)
                {
                    return 2;
                }
                return 1;
            }
            return 0;
        }
        public async Task<ReviewRatings> CreateReviewRatingAsync(ReviewRatings data, string raterId)
        {
            var review = db.Reviews.Where(r => r.Id == data.ReviewId).FirstOrDefault();
            review.Status = ReviewRequestStatus.COMPLETED;

            data.UserId = review.ReviewerId; //Assign ratee id

            await db.ReviewRatings.AddAsync(data);
            
            //Update user rate (UserRanks)
            decimal rate = 0;
            var userRates = db.ReviewRatings.Where(r => r.UserId == review.ReviewerId).ToList();

            //because record has just added has not been committed to DB, need to add them to the list to calculate average
            userRates.Add(new ReviewRatings { Rate = data.Rate });

            rate = userRates.Average(r => r.Rate);

            UserRanks rank = await db.UserRanks.Where(r => r.UserId == review.ReviewerId).FirstOrDefaultAsync();
            if(rank == null)
            {
                await db.UserRanks.AddAsync(new UserRanks { UserId = review.ReviewerId, AverageReviewRate = data.Rate });
            }
            else
            {
                rank.AverageReviewRate = rate;
            }

            //List<ReviewRequests> requests = await db.ReviewRequests.Where(r => r.UserId == data.UserId).ToListAsync();
            //List<Reviews> userRatedList = await this.GetRatedReviewsAsync(data.UserId);

            //bool pendingFlag = true;
            //if(requests.Count >0 && requests.Count == userRatedList.Count)
            //{
            //    foreach(ReviewRequests r in requests)
            //    {
            //        r.Status = "Submitted";
            //        Submissions submission = await db.Submissions.FindAsync(r.SubmissionId);
            //        submission.Type = "Completed";
            //        await db.SaveChangesAsync();
            //    }   
            //}

            //Update submission status as review has been rated
            var requestId = db.Reviews.Where(r => r.Id == data.ReviewId).FirstOrDefault()?.RequestId;

            if(requestId != null)
            {
                var request = db.ReviewRequests.Where(r => r.Id == requestId).FirstOrDefault();
                var submission = db.Submissions.Where(s => s.Id == request.SubmissionId).FirstOrDefault();

                submission.Status = SubmissionStatus.COMPLETED;
                request.Status = ReviewRequestStatus.RATED;
            }


            await db.SaveChangesAsync();

            IList<Submissions> _pendingSubmissions = null;

            //If there is no un-rated reviews left, change all Pending submission to Submitted
            var unratedReviews = await GetUnRatedReviewOfUser(raterId);
            if (unratedReviews.Count() == 0)
            {
                var pendingSubmissions = db.Submissions.Where(s => s.UserId == raterId && s.Status == SubmissionStatus.PENDING);
                foreach (var sub in pendingSubmissions)
                {
                    sub.Status = SubmissionStatus.SUBMITTED;
                    //await AddQueueAfterSubmittedPendingReview(sub.Id, raterId);
                }
                _pendingSubmissions = pendingSubmissions.ToList();
            }

            await db.SaveChangesAsync();

            if(_pendingSubmissions != null)
            {
                foreach (var sub in _pendingSubmissions)
                {
                    await AddQueueAfterSubmittedPendingReview(sub.Id, raterId);
                }
            }
            

            return data;
        }
        public async Task<ReviewRatings> GetReviewRatingsByReviewIdAsync(int reviewId)
        {
            var rs = await db.ReviewRatings.Where(r => r.ReviewId == reviewId).FirstOrDefaultAsync();
            return rs;
        }
        public async Task<RequestQueue> AddRequestQueue(RequestQueue data, string userID)
        {
            var reviews = await db.Reviews.Where(r => r.ReviewerId == userID).CountAsync();

            decimal rate = 0;
            var userRates = await db.ReviewRatings.Where(r => r.UserId == userID).ToListAsync();

            if (userRates.Count > 0)
            {
                rate = userRates.Average(r => r.Rate);
            }

            //var pendings = TODO: query pending

            var priority = rate * 5 + reviews; //-pendings

            data.Priority = (int)priority;
            data.MinimumRate = rate;
            data.Status = 0;

            await db.RequestQueue.AddAsync(data);
            await db.SaveChangesAsync();

            return data;
        }
        public async Task<List<string>> GetTestForCurrentUsers(string UserId)
        {
            return await (from us in db.UserScores
                          join ts in db.TestSections on us.SectionId equals ts.Id
                          join t in db.Tests on ts.TestId equals t.Id
                          where us.UserId == UserId
                          group t by t.Name into g
                          select g.Key).ToListAsync();
        }
        public async Task<GetReviewsModel> CreateReviewFromQueue(string userID)
        {
            decimal rate = 0;
            var userRate = await db.UserRanks.Where(r => r.UserId == userID).FirstOrDefaultAsync();

            if(userRate != null)
            {
                rate = userRate.AverageReviewRate;
            }

            var tests = await GetTestForCurrentUsers(userID);

            var request = await (from queue in db.RequestQueue 
                                 join rq in db.ReviewRequests on queue.RequestId equals rq.Id
                                 join question in db.Questions on rq.Submission.QuestionId equals question.Id
                                 join sec in db.TestSections on question.Task.SectionId equals sec.Id
                                 join test in db.Tests on sec.TestId equals test.Id
                                 where queue.MinimumRate <= rate && queue.Status == 0 && rq.Status != "Pending" && tests.Contains(test.Name) && rq.UserId != userID
                                 orderby queue.Priority descending, queue.RequestedDatetime ascending
                                 select queue).FirstOrDefaultAsync();

            //var request = db.RequestQueue
            //    .FromSqlRaw(@"UPDATE TOP(1) RequestQueue WITH (UPDLOCK, READPAST)
            //                SET Status = 1
            //                OUTPUT inserted.*
            //                FROM RequestQueue rq
            //                INNER JOIN ReviewRequests rr ON rq.RequestId = rr.Id 
            //                WHERE rq.Status = 0 AND minimumRate <= " + rate + " AND rr.Status != 'Pending'").AsEnumerable().FirstOrDefault();

            if (request != null)
            {
                request.Status = 1;
                await db.SaveChangesAsync();
                var review = await GetOrCreateReviewByReviewRequestAsync(request.RequestId, userID);
                return review;
            }

            return null;
        }

        public async Task<List<Reviews>> GetRatedReviewsAsync(string userId)
        {
            var rated = await (from r in db.Reviews
                                join request in db.ReviewRequests on r.RequestId equals request.Id
                                join rating in db.ReviewRatings on r.Id equals rating.ReviewId
                                where rating.UserId == userId
                                select r).ToListAsync();

            var allCompletedRequest = await (from r in db.Reviews
                                             join request in db.ReviewRequests on r.RequestId equals request.Id
                                             where request.UserId == userId && request.Status == ReviewRequestStatus.COMPLETED
                                             select r).ToListAsync();

            var unrated = new List<Reviews>();

            foreach(Reviews r in allCompletedRequest)
            {
                if (!rated.Contains(r))
                {
                    unrated.Add(r);
                }
            }

            return unrated;
        }

        public async Task<GetReviewsModel> GetPendingReviewAsync(string userId)
        {
            var pending = await (from r in db.Reviews
                                 join request in db.ReviewRequests on r.RequestId equals request.Id
                                 join queue in db.RequestQueue on request.Id equals queue.RequestId
                                 where r.ReviewerId == userId && request.Status != ReviewRequestStatus.COMPLETED && request.Status != ReviewRequestStatus.RATED && queue.Status==1
                                 select r).FirstOrDefaultAsync();
            if (pending != null)
            {
                var review = await GetOrCreateReviewByReviewRequestAsync(pending.RequestId, userId);
                return review;
            }

            return null;
        } 

        public async Task<IEnumerable<Reviews>> GetUnRatedReviewOfUser(string userId)
        {
            return await (from rv in db.Reviews
                          join rq in db.ReviewRequests on rv.RequestId equals rq.Id
                          join rt in db.ReviewRatings on rv.Id equals rt.ReviewId into completedReviews
                          from rated in completedReviews.DefaultIfEmpty()
                          where rv.RevieweeId == userId && rq.Status == ReviewRequestStatus.COMPLETED && rated == null
                          select rv).ToListAsync();
        }
        
        public async Task<Reviews> GetReviewByIdAsync(int id)
        {
            return await db.Reviews.FindAsync(id);
        }

        private async Task<RequestQueue> AddQueueAfterSubmittedPendingReview(int subId, string userId)
        {
            var requests = await db.ReviewRequests.Where(rr => rr.SubmissionId == subId).ToListAsync(); 

            foreach(var rr in requests)
            {
                await AddRequestQueue(new RequestQueue { RequestId = rr.Id, RequestedDatetime = DateTime.Now }, userId);
            }

            return null;
        }

        public async Task<ReviewRatings> SubmitReviewRatingAsync(ReviewRatings data,string userId)
        {
            var review = await db.Reviews.Where(rs => rs.Id == data.ReviewId && rs.ReviewerId == userId).FirstOrDefaultAsync();
            if (review!= null)
            {
                review.Status = SubmissionStatus.COMPLETED;
                await db.SaveChangesAsync();
                return data;
            }
            return null;
        }
    }
}
