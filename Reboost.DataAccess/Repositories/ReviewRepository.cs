using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
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
        Task SaveFeedback(List<ReviewData> data);
        Task<List<ReviewData>> LoadFeedBack(int reviewId);
        Task<InTextComments> AddInTextCommentAsync(InTextComments cmt);
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
        public async Task SaveFeedback(List<ReviewData> data)
        {

            db.ReviewData.AddRange(data);
            await db.SaveChangesAsync();
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

    }
}
