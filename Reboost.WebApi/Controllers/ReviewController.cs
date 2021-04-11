using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : BaseController<IReviewService>
    {

        private IUserService _userService;
        public ReviewController(IReviewService service, IUserService userService) : base(service)
        {
            _userService = userService;
        }

        [HttpGet("getAnnotation/{docId}/{reviewId}")]
        public async Task<IActionResult> GetAnnotationsAsync([FromRoute] int docId, [FromRoute] int reviewId)
        {
            var rs = await _service.GetAnnotationsAsync(docId, reviewId);
            return Ok(rs);
        }

        [HttpPost("saveAnnotation/{docId}/{reviewId}")]
        public async Task<IActionResult> SaveAnnotationsAsync([FromRoute] int docId, [FromRoute] int reviewId, [FromBody] AnnotationModel data)
        {
            var savedAnnots = await _service.SaveAnnotationsAsync(docId, reviewId, data.Annotations.ToArray());
            var savedComments = await _service.SaveCommentsAsync(savedAnnots, data.Comments);

            return Ok(new { annotations = savedAnnots, comments = savedComments });
        }
        [HttpPost("createSampleReview/{type}")]
        public async Task<IActionResult> CreateNewSampleReviewDocumentAsync([FromRoute] string type)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var newReview = await _service.CreateNewSampleReviewDocumentAsync(type, currentUser);
            return Ok(newReview);
        }
        [HttpPost("feedback")]
        public async Task<IActionResult> ReviewFeedback([FromBody] List<ReviewData> data)
        {
            await _service.SaveFeedback(data);
            return Ok();
        }
        [HttpGet("feedback/{reviewId}")]
        public async Task<IActionResult> GetFeedBack([FromRoute] int reviewId)
        {
            var rs = await _service.LoadFeedback(reviewId);
            return Ok(rs);
        }
        [HttpPost("annotation")]
        public async Task<IActionResult> AddAnnotationAsync([FromBody] Annotations data)
        {
            var rs = await _service.AddAnnotationAsync(data);
            return Ok(rs);
        }
        [HttpPost("inTextComment/{docId}/{reviewId}")]
        public async Task<IActionResult> AddInTextCommentAsync([FromRoute] int docId, [FromRoute] int reviewId, [FromBody] InsertCommentModel data)
        {
            var rs = await _service.AddInTextCommentAsync(docId, reviewId, data.Comment, data.Annotation);
            return Ok(rs);
        }
        [HttpPost("comment/delete")]
        public async Task<IActionResult> DeleteInTextCommentAsync([FromBody] DeleteCommentModel data)
        {
            var rs = await _service.DeleteInTextCommentAsync(data.id);
            return Ok(rs);
        }
        [HttpPost("annotation/delete")]
        public async Task<IActionResult> DeleteAnnotationAsync([FromBody] DeleteCommentModel data)
        {
            var rs = await _service.DeleteAnnotationAsync(data.id);
            return Ok(rs);
        }
        [HttpPost("edit")]
        public async Task<IActionResult> EditAnotationAsync([FromBody] Annotations data)
        {
            var rs = await _service.EditAnnotationAsync(data);
            return Ok(rs);
        }
        [HttpPost("comment/edit")]
        public async Task<IActionResult> EditInTextCommentAsync([FromBody] InTextComments comment)
        {
            var rs = await _service.EditInTextComment(comment);
            return Ok(rs);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllReviewsAsync()
        {
            var rs = await _service.GetReviewsAsync();
            return Ok(rs);
        }
        [HttpPost("status/change/{id}")]
        public async Task<IActionResult> ChangeReviewStatusAsync([FromBody] UpdateStatusModel status, [FromRoute] int id)
        {
            var rs = await _service.ChangeStatusAsync(id, status.status);
            return Ok(rs);
        }

    }
}
