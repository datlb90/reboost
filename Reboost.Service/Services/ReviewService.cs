using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
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
        Task SaveFeedback(List<ReviewData> data);
        Task<List<ReviewData>> LoadFeedback(int reviewId);
        Task<Annotations> AddAnnotationAsync(Annotations annotation);
        Task<InTextComments> AddInTextCommentAsync(int docId, int reviewId, InTextComments cmt, Annotations anno);
        Task<InTextComments> DeleteInTextCommentAsync(int id);
        Task<int> DeleteAnnotationAsync(int id);
        Task<Annotations> EditAnnotationAsync(Annotations anno);
        Task<InTextComments> EditInTextComment(InTextComments cmt);
        Task<string> CreateNewSampleReviewDocumentAsync(string type, User user);
        Task<List<Reviews>> GetReviewsAsync();
        Task<List<Reviews>> GetReviewsByIdAsync(string userId);
        Task<Reviews> ChangeStatusAsync(int id, string newStatus);
        Task<ReviewRequests> CreateRequestAsync(ReviewRequests requests);
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
        public async Task SaveFeedback(List<ReviewData> data)
        {
            await _unitOfWork.Review.SaveFeedback(data);
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
            if(anno.Type !="comment-area")
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
        public async Task<Reviews> ChangeStatusAsync(int id, string newStatus)
        {
            return await _unitOfWork.Review.ChangeStatusAsync(id, newStatus);
        }
        public async Task<List<Reviews>> GetReviewsByIdAsync(string userId)
        {
            return await _unitOfWork.Review.GetReviewsByIdAsync(userId);
        }
        public async Task<ReviewRequests> CreateRequestAsync(ReviewRequests requests)
        {
            DateTime now = DateTime.Now;
            requests.RequestedDateTime = now;
            return await _unitOfWork.Review.CreateRequestAsync(requests);
        }
    }
}
