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
    }
}
