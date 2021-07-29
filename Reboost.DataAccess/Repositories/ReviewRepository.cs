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
        Task<Reviews> ChangeStatusAsync(int id, UpdateStatusModel model);
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
        Task<GetReviewsModel> GetReviewByIdAsync(int id);
        Task<IEnumerable<Reviews>> GetUnRatedReviewOfUser(string userId);
        Task<ReviewRatings> SubmitReviewRatingAsync(ReviewRatings data, string userId);
        Task<CreatedProRequestModel> CreateProRequestAsync(ReviewRequests request);
        Task<CreatedProRequestModel> ReRequestProRequestAsync(ReviewRequests request);
        Task<GetReviewsModel> GetOrCreateReviewByProRequestId(int requestId, string currentUserId);
        Task<int> IsProRequestCheckAsync(int reviewId);
        Task<List<RaterTraining>> GetRaterTrainingsAsync(int raterId);
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

            await db.ReviewData.AddRangeAsync(data);

            // Check if review is a training, then change rater status to 'Training Completed' if his/her have finished all training that applied to
            Raters rater = await db.Raters.Where(r => r.UserId == review.ReviewerId).FirstOrDefaultAsync();
            if(rater != null && rater.Status != RaterStatus.APPROVED)
            {
                RaterTraining training = await db.RaterTraining.Where(t => t.ReviewId == reviewId).FirstOrDefaultAsync();

                RaterRepository _raterRepository = new RaterRepository(db);
                Raters _rater = await _raterRepository.GetByUserIdAsync(review.ReviewerId);
                List<string> trainingCount = await _raterRepository.GetApplyTo(_rater.Id);

                if (training.Status == RaterTrainingStatus.REVISION_REQUEST)
                {
                    training.Status = RaterTrainingStatus.REVISION_COMPLETED;
                }
                else
                {
                    training.Status = RaterTrainingStatus.COMPLETED;
                }

                int completedTrainingCount = await db.RaterTraining.Where(t => t.RaterId == rater.Id && (t.Status.Contains(RaterTrainingStatus.COMPLETED) || t.Status==RaterTrainingStatus.APPROVED)).CountAsync();

                if (trainingCount.Count() == (completedTrainingCount+1))
                {
                    rater.Status = RaterStatus.TRAINING_COMPLETED;
                }

            }
            else
            {
                ReviewRequests request = await db.ReviewRequests.Where(rq => rq.Id == review.RequestId).FirstOrDefaultAsync();

                if (request != null)
                {
                    //Update submission status to 'Reviewed'
                    var submission = db.Submissions.Where(s => s.Id == request.SubmissionId).FirstOrDefault();
                    if (submission != null)
                    {
                        submission.UpdatedDate = DateTime.Now;
                        submission.Status = SubmissionStatus.REVIEWED;
                    }

                    request.Status = ReviewRequestStatus.COMPLETED;
                    request.CompletedDateTime = DateTime.Now;
                }
            }

            review.Status = ReviewStatus.COMPLETED;
            review.LastActivityDate = DateTime.Now;
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
        public async Task<Reviews> ChangeStatusAsync(int id, UpdateStatusModel model)
        {
            Reviews rv = await db.Reviews.FindAsync(id);
            RaterTraining training = await db.RaterTraining.Where(t => t.ReviewId == id).FirstOrDefaultAsync();

            if(rv== null || training == null)
            {
                return null;
            }

            if(training.Status == RaterTrainingStatus.APPROVED)
            {
                return null;
            }

            training.Note = model.note;
            training.Status = model.status;

            Raters rater = await db.Raters.Where(r => r.Id == training.RaterId).FirstOrDefaultAsync();

            if(model.status == RaterTrainingStatus.REVISION_REQUEST)
            {
                rv.Status = ReviewStatus.IN_PROGRESS;
                rater.Status = RaterStatus.REVISION_REQUESTED;
            }

            if(model.status == RaterTrainingStatus.APPROVED)
            {
                RaterRepository _raterRepository = new RaterRepository(db);
                List<string> trainingCount = await _raterRepository.GetApplyTo(rater.Id);

                int approvedTrainingCount = await db.RaterTraining.Where(t => t.RaterId == rater.Id && t.Status ==  RaterTrainingStatus.APPROVED).CountAsync();

                if (trainingCount.Count() == (approvedTrainingCount + 1))
                {
                    rater.Status = RaterStatus.APPROVED;
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
                return await Task.FromResult("Failed");
            }

            string status = type.ToUpper() + RaterStatus.TRAINING;

            RaterTraining exist = await db.RaterTraining.Where(r => r.RaterId == rater.Id && r.Test == type.ToUpper()).FirstOrDefaultAsync();
            if(exist!= null)
            {
                return await Task.FromResult(exist.ReviewId.ToString());
            }

            Reviews rv = new Reviews { RequestId = 1, FinalScore = 0, ReviewerId = user.Id, RevieweeId = "1", Status = ReviewStatus.IN_PROGRESS, TimeSpentInSeconds = 0, LastActivityDate = DateTime.Now };
            await db.Reviews.AddAsync(rv);
            await db.SaveChangesAsync();

            RaterTraining newTraining = new RaterTraining { RaterId = rater.Id, Note = null, ReviewId = rv.Id, Status = ReviewStatus.IN_PROGRESS, Test = type.ToUpper() };
            await db.RaterTraining.AddAsync(newTraining);
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

            Reviews rv = new Reviews { RequestId = requestId, FinalScore = 0, ReviewerId = userId, RevieweeId = reviewRequest.UserId, Status = ReviewStatus.IN_PROGRESS, TimeSpentInSeconds = 0, LastActivityDate = currentTime };

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
                                 where queue.MinimumRate <= rate && queue.Status == 0 && rq.Status != SubmissionStatus.PENDING && tests.Contains(test.Name) && rq.UserId != userID
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
        
        public async Task<GetReviewsModel> GetReviewByIdAsync(int id)
        {
            GetReviewsModel result = new GetReviewsModel();
            
            result.Review = await db.Reviews.FindAsync(id);

            result.ReviewId = id;
            result.ReviewRequest = await db.ReviewRequests.Where(rq => rq.Id == result.Review.RequestId).FirstOrDefaultAsync();
            if(result.ReviewRequest!= null)
            {
                result.Submission = await db.Submissions.Where(s => s.Id == result.ReviewRequest.SubmissionId).FirstOrDefaultAsync();
            }

            return result;
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

        public async Task<CreatedProRequestModel> CreateProRequestAsync(ReviewRequests request)
        {
            // Add request and queue
            request.FeedbackType = ReviewRequestType.PRO;
            request.RequestedDateTime = DateTime.Now;
            request.Status = ReviewRequestStatus.WAITING;
            await db.ReviewRequests.AddAsync(request);

            // Change submission status
            Submissions sub = await db.Submissions.FindAsync(request.SubmissionId);
            sub.Status = SubmissionStatus.REVIEW_REQUESTED;

            await db.SaveChangesAsync();

            await AddRequestQueue(new RequestQueue { RequestId = request.Id, RequestedDatetime = DateTime.Now }, request.UserId);

            var rater = await GetRaterForProRequestAsync();
            return new CreatedProRequestModel{Rater= rater, Request= request };
        }

        public async Task<CreatedProRequestModel> ReRequestProRequestAsync(ReviewRequests request)
        {
            // Update requested time on Queue
            var queue = await db.RequestQueue.Where(q => q.RequestId == request.Id).FirstOrDefaultAsync();
            queue.RequestedDatetime = DateTime.Now;
            await db.SaveChangesAsync();

            var rater = await GetRaterForProRequestAsync();
            return new CreatedProRequestModel { Rater = rater, Request = request };
        }

        // Get rater for pro request
        public async Task<Raters> GetRaterForProRequestAsync()
        {
            // Select all rater that having completed reviews
            var completedReviewsRaters = await (from reviews in db.Reviews
                                                where reviews.Status == ReviewRequestStatus.COMPLETED
                                                group reviews by new { reviews.ReviewerId, reviews.Status } into g
                                                orderby g.Key.ReviewerId descending
                                                select new
                                                {
                                                    RaterId = g.Key.ReviewerId,
                                                    CompletedCount = (from re in db.Reviews where re.Status == ReviewRequestStatus.COMPLETED && re.ReviewerId == g.Key.ReviewerId select re).Count()
                                                }
                                                ).ToListAsync();

            // Get the minimun value of completed reviews count
            var minCompletedCount = completedReviewsRaters.Min(r => r.CompletedCount);

            // Get all rater's ID with the minimun value of completed reviews count
            var minRatersList = completedReviewsRaters.Where(r => r.CompletedCount == minCompletedCount).ToList();

            // Get all rater's details
            List<Raters> minRatersDetailList = new List<Raters>();

            int i = 0;
            while (i < minRatersList.Count())
            {
                var rs = await db.Raters.Include("User").Where(r => r.UserId == minRatersList[i].RaterId).FirstOrDefaultAsync();
                if (rs != null)
                {
                    minRatersDetailList.Add(rs);
                }
                i++;
            }

            // Get first rater base on apply date 
            var rater = minRatersDetailList.OrderBy(r => r.AppliedDate).FirstOrDefault();



            //Query rater that have lowest assigned requests
            var selectedRater = (from r in db.Raters
                                 join a in (from a in db.RequestAsignments where a.Status == RequestAssigmentStatus.ASSIGNED select a) on r.Id equals a.RaterId into raterFull
                                 from a in raterFull.DefaultIfEmpty()
                                 orderby r.AppliedDate
                                 group r by r.Id into g
                                 select new
                                 {
                                     RaterId = g.Key,
                                     AssignedRequests = g.Count()
                                 }).OrderBy(r => r.AssignedRequests).FirstOrDefault();

            //Assign the pro request for selected rater
            // - Insert a record to RequestAssignment for selected rater
            // - Send email to selected rater

            return rater;
        }

        public async Task<GetReviewsModel> GetOrCreateReviewByProRequestId(int requestId, string currentUserId)
        {
            // Get Request and RequestQueue and check time-out
            var request = await db.ReviewRequests.FindAsync(requestId);
            var queue = await db.RequestQueue.Where(q => q.RequestId == requestId).FirstOrDefaultAsync();

            // Return null if not exist
            if (request == null || queue == null)
            {
                return null;
            }

            if (DateTime.Now.Subtract(queue.RequestedDatetime).TotalSeconds > 600)
            {
                return null;
            }

            // Get review if existed
            GetReviewsModel exist = new GetReviewsModel();
            exist.Review = await db.Reviews.Where(r => r.RequestId == requestId).FirstOrDefaultAsync();
            exist.ReviewId = exist.Review.Id;
            exist.ReviewRequest = request;
            exist.Submission = await db.Submissions.Where(s => s.Id == exist.ReviewRequest.SubmissionId).FirstOrDefaultAsync();

            if (exist?.Review != null)
            {
                return exist;
            }

            // Create new review if not existed
            Reviews rv = new Reviews { RevieweeId = request.UserId, RequestId = request.Id, ReviewerId = currentUserId, LastActivityDate = DateTime.Now, FinalScore = null, TimeSpentInSeconds = 0, Status = ReviewStatus.IN_PROGRESS };
            await db.Reviews.AddAsync(rv);

            // Change Request, RequestQueue status to 'In Progess'
            request.Status = ReviewStatus.IN_PROGRESS;
            queue.Status = 1;

            // Update requested time in queue for checking review timeout
            queue.RequestedDatetime = DateTime.Now;
            await db.SaveChangesAsync();

            GetReviewsModel result = new GetReviewsModel();
            result.Review = rv;
            result.ReviewRequest = request;
            result.Submission = await db.Submissions.Where(s => s.Id == result.ReviewRequest.SubmissionId).FirstOrDefaultAsync();

            
            


            return result;
        }

        public async Task<int> IsProRequestCheckAsync(int reviewId)
        {
            Reviews review = await db.Reviews.FindAsync(reviewId);

            // Check if is a pro review
            ReviewRequests isProRequest = await db.ReviewRequests.Where(rq => rq.Id == review.RequestId && rq.FeedbackType == ReviewRequestType.PRO).FirstOrDefaultAsync();

            // Check timeout if is a pro review
            if (isProRequest != null)
            {
                RequestQueue queue = await db.RequestQueue.Where(q => q.RequestId == isProRequest.Id).FirstOrDefaultAsync();
                if (DateTime.Now.Subtract(queue.RequestedDatetime).TotalSeconds > (3 * 60 * 60))
                {
                    // Return 0 if review's timeout end
                    return 0;
                }

                // Return 1 if it is a pro request
                return 1;
            }

            // Return -1 if it is NOT a pro request
            return -1;
        }

        public async Task<List<RaterTraining>> GetRaterTrainingsAsync(int raterId)
        {
            return await db.RaterTraining.Where(r => r.RaterId == raterId).ToListAsync();
        }
    }
}
