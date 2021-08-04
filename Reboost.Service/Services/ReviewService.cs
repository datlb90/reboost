using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IReviewService
    {
        Task<AnnotationModel> GetAnnotationsAsync(int docId, int reviewId);
        Task<IEnumerable<Annotations>> SaveAnnotationsAsync(int docId, int reviewId, IEnumerable<Annotations> annotations);
        Task<IEnumerable<InTextComments>> SaveCommentsAsync(IEnumerable<Annotations> annotations, IEnumerable<InTextComments> comments);
        Task<Reviews> SaveFeedback(int reviewId, List<ReviewData> data);
        Task<List<ReviewData>> LoadFeedback(int reviewId);
        Task<Annotations> AddAnnotationAsync(Annotations annotation);
        Task<InTextComments> AddInTextCommentAsync(int docId, int reviewId, InTextComments cmt, Annotations anno);
        Task<InTextComments> DeleteInTextCommentAsync(int id);
        Task<int> DeleteAnnotationAsync(int id);
        Task<Annotations> EditAnnotationAsync(Annotations anno);
        Task<InTextComments> EditInTextComment(InTextComments cmt);
        Task<string> CreateNewSampleReviewDocumentAsync(string type, User user);
        Task<List<Reviews>> GetReviewsAsync();
        Task<List<Reviews>> GetReviewsByUserIdAsync(string userId);
        Task<GetReviewsModel> GetReviewByIdAsync(int id);
        Task<Reviews> ChangeStatusAsync(int id, UpdateStatusModel model);
        Task<ReviewRequests> CreateRequestAsync(string userId, ReviewRequests requests);
        Task<List<GetReviewsModel>> GetRaterReviewsByIdAsync(String userId);
        Task<GetReviewsModel> GetOrCreateReviewByReviewRequestAsync(int submissionId, string userId);
        Task<int> CheckUserReviewValidationAsync(string role, User user, int reviewId);
        Task<int> CheckReviewValidationAsync(string role, User user, int reviewId);
        Task<ReviewRequests> GetReviewRequestBySubmissionId(int submissionId, string userId);
        Task<ReviewRatings> CreateReviewRatingAsync(ReviewRatings data, string raterId);
        Task<ReviewRatings> GetReviewRatingsByReviewIdAsync(int reviewId);
        Task<RequestQueue> AddRequestQueue(RequestQueue data, string userId);
        Task<GetReviewsModel> CreateReviewFromQueue(string userId);
        Task<IEnumerable<Reviews>> GetUnRatedReviewsAsync(string userId);
        Task<GetReviewsModel> GetPendingReviewAsync(string userId);
        Task<ReviewRatings> SubmitReviewRatingAsync(ReviewRatings data, string userId);
        Task<CreatedProRequestModel> CreateProRequestAsync(ReviewRequests request);
        Task<CreatedProRequestModel> ReRequestProRequestAsync(ReviewRequests request);
        Task<GetReviewsModel> GetOrCreateReviewByProRequestId(int requestId, string currentUserId);
        Task<int> IsProRequestCheckAsync(int reviewId);
        Task<List<RaterTraining>> GetRaterTrainingsAsync(int raterId);
    }

    public class ReviewService : BaseService, IReviewService
    {
        public ReviewService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<AnnotationModel> GetAnnotationsAsync(int docId, int reviewId)
        {
            return await _unitOfWork.Review.GetAnnotationsAsync(docId, reviewId);
        }
        public async Task<int> CheckUserReviewValidationAsync(string role, User user, int reviewId)
        {
            return await _unitOfWork.Review.CheckUserReviewValidationAsync(role, user, reviewId);
        }
        public async Task<IEnumerable<Annotations>> SaveAnnotationsAsync(int docId, int reviewId, IEnumerable<Annotations> annotations )
        {
            foreach (var anot in annotations)
            {
                anot.ReviewId = reviewId;
            }
            return await _unitOfWork.Review.SaveAnnotationsAsync(docId, reviewId, annotations);
        }
        public async Task<IEnumerable<InTextComments>> SaveCommentsAsync(IEnumerable<Annotations> annotations, IEnumerable<InTextComments> comments )
        {
            var _comments = new List<InTextComments>();
            foreach (var cmt in comments)
            {
                var annot = annotations.Where(a => a.Uuid == cmt.Uuid).FirstOrDefault();
                if(annot != null)
                {
                    cmt.Annotation = null;
                    cmt.AnnotationId = annot.Id;
                    _comments.Add(cmt);
                }
            }
            return await _unitOfWork.Review.SaveCommentsAsync(_comments);
        }
        public async Task<Reviews> SaveFeedback(int reviewId, List<ReviewData> data)
        { 
            return await _unitOfWork.Review.SaveFeedback(reviewId, data);
        }
        public async Task<List<ReviewData>> LoadFeedback(int reviewId)
        {
            return await _unitOfWork.Review.LoadFeedBack(reviewId);
        }
        public async Task<Annotations> AddAnnotationAsync(Annotations annotation)
        {
            return await _unitOfWork.Review.AddAnnotationAsync(annotation);
        }
        public async Task<InTextComments> AddInTextCommentAsync(int docId, int reviewId, InTextComments cmt, Annotations anno)
        {
            
            if(anno.Id == 0)
            {
                await _unitOfWork.Review.AddAnnotationAsync(anno);
            }

            cmt.AnnotationId = anno.Id;
            return await _unitOfWork.Review.AddInTextCommentAsync(cmt);
        }
        public async Task<InTextComments> DeleteInTextCommentAsync(int id)
        {
            return await _unitOfWork.Review.DeleteInTextCommentAsync(id);
        }
        public async Task<Annotations> EditAnnotationAsync(Annotations anno)
        {
            return await _unitOfWork.Review.EditAnnotationAsync(anno);
        }
        public async Task<int> DeleteAnnotationAsync(int id)
        {
            return await _unitOfWork.Review.DeleteAnnotationAsync(id);
        }
        public async Task<InTextComments> EditInTextComment(InTextComments cmt)
        {
            return await _unitOfWork.Review.EditInTextComment(cmt);
        }
        public async Task<string> CreateNewSampleReviewDocumentAsync(string type, User user)
        {
            return await _unitOfWork.Review.CreateNewSampleReviewDocumentAsync(type, user);
        }
        public async Task<List<Reviews>> GetReviewsAsync()
        {
            return await _unitOfWork.Review.GetReviewsAsync();
        }
        public async Task<Reviews> ChangeStatusAsync(int id, UpdateStatusModel model)
        {
            return await _unitOfWork.Review.ChangeStatusAsync(id, model);
        }
        public async Task<List<Reviews>> GetReviewsByUserIdAsync(string userId)
        {
            return await _unitOfWork.Review.GetReviewsByUserIdAsync(userId);
        }
        public async Task<ReviewRequests> CreateRequestAsync(string userId, ReviewRequests requests)
        {
            DateTime now = DateTime.Now;
            requests.RequestedDateTime = now;
            return await _unitOfWork.Review.CreateRequestAsync(userId, requests);
        }
        public async Task<RequestQueue> AddRequestQueue(RequestQueue requestQueue, string userId) {
            return await _unitOfWork.Review.AddRequestQueue(requestQueue, userId);
        }
        public async Task<List<GetReviewsModel>> GetRaterReviewsByIdAsync(String userId)
        {
            return await _unitOfWork.Review.GetRaterReviewsByIdAsync(userId);
        }
        public async Task<GetReviewsModel> GetOrCreateReviewByReviewRequestAsync(int requestId, string userId)
        {
            return await _unitOfWork.Review.GetOrCreateReviewByReviewRequestAsync(requestId, userId);
        }
        public async Task<ReviewRequests> GetReviewRequestBySubmissionId(int submissionId, string userId)
        {
            return await _unitOfWork.Review.GetReviewRequestBySubmissionId(submissionId, userId);
        }
        public async Task<int> CheckReviewValidationAsync(string role, User user, int reviewId)
        {
            return await _unitOfWork.Review.CheckReviewValidationAsync(role, user, reviewId);
        }
        public async Task<ReviewRatings> CreateReviewRatingAsync(ReviewRatings data, string raterId)
        {
            return await _unitOfWork.Review.CreateReviewRatingAsync(data, raterId);
        }
        public async Task<ReviewRatings> GetReviewRatingsByReviewIdAsync(int reviewId)
        {
            return await _unitOfWork.Review.GetReviewRatingsByReviewIdAsync(reviewId);
        }
        public async Task<GetReviewsModel> CreateReviewFromQueue(string userId) {
            return await _unitOfWork.Review.CreateReviewFromQueue(userId);
        }
        public async Task<IEnumerable<Reviews>> GetUnRatedReviewsAsync(string userId)
        {
            return await _unitOfWork.Review.GetUnRatedReviewOfUser(userId);
        }
        public async Task<GetReviewsModel> GetPendingReviewAsync(string userId)
        {
            return await _unitOfWork.Review.GetPendingReviewAsync(userId);
        }
        public async Task<GetReviewsModel> GetReviewByIdAsync(int id)
        {
            return await _unitOfWork.Review.GetReviewByIdAsync(id);
        }

        public async Task<ReviewRatings> SubmitReviewRatingAsync(ReviewRatings data, string userId)
        {
            return await _unitOfWork.Review.SubmitReviewRatingAsync(data, userId);
        }
        public async Task<CreatedProRequestModel> CreateProRequestAsync(ReviewRequests request)
        {
            return await _unitOfWork.Review.CreateProRequestAsync(request);
        }
        public async Task<CreatedProRequestModel> ReRequestProRequestAsync(ReviewRequests request)
        {
            return await _unitOfWork.Review.ReRequestProRequestAsync(request);
        }
        public async Task<GetReviewsModel> GetOrCreateReviewByProRequestId(int requestId, string currentUserId)
        {
            return await _unitOfWork.Review.GetOrCreateReviewByProRequestId(requestId, currentUserId);
        }
        public async Task<int> IsProRequestCheckAsync(int reviewId)
        {
            return await _unitOfWork.Review.IsProRequestCheckAsync(reviewId);
        }
        public async Task<List<RaterTraining>> GetRaterTrainingsAsync(int raterId)
        {
            return await _unitOfWork.Review.GetRaterTrainingsAsync(raterId);
        }
    }
}
