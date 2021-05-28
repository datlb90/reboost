using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;
using Reboost.Shared;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : BaseController<IReviewService>
    {

        private IUserService _userService;
        private IRaterService _raterService;
        public ReviewController(IReviewService service, IUserService userService, IRaterService raterService) : base(service)
        {
            _userService = userService;
            _raterService = raterService;
        }
        [Authorize]
        [HttpGet("getAnnotation/{docId}/{reviewId}")]
        public async Task<IActionResult> GetAnnotationsAsync([FromRoute] int docId, [FromRoute] int reviewId)
        {
            var rs = await _service.GetAnnotationsAsync(docId, reviewId);
            return Ok(rs);
        }
        [Authorize]
        [HttpGet("{reviewId}/auth")]
        public async Task<IActionResult> UserReviewAuthentication([FromRoute] int reviewId)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var role = currentUserClaim.FindFirst("Role");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var check = await _service.CheckUserReviewValidationAsync(role.Value, currentUser, reviewId);
            if (check == 1)
            {
                return Ok();
            }
            else if (check == 0)
            {
                return BadRequest(new { message = "Review not found!" });
            }
            return BadRequest(new { message = "Permission denined" });
        }
        [Authorize]
        [HttpPost("rater/auth")]
        public async Task<IActionResult> RaterApprovedCheck()
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var rater = await _raterService.GetByUserIdAsync(currentUser.Id);
            return Ok(rater);
        }
        [Authorize]
        [HttpPost("saveAnnotation/{docId}/{reviewId}")]
        public async Task<IActionResult> SaveAnnotationsAsync([FromRoute] int docId, [FromRoute] int reviewId, [FromBody] AnnotationModel data)
        {
            var savedAnnots = await _service.SaveAnnotationsAsync(docId, reviewId, data.Annotations.ToArray());
            var savedComments = await _service.SaveCommentsAsync(savedAnnots, data.Comments);

            return Ok(new { annotations = savedAnnots, comments = savedComments });
        }
        [Authorize]
        [HttpPost("createSampleReview/{type}")]
        public async Task<IActionResult> CreateNewSampleReviewDocumentAsync([FromRoute] string type)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var newReview = await _service.CreateNewSampleReviewDocumentAsync(type, currentUser);
            return Ok(newReview);
        }
        [Authorize]
        [HttpPost("feedback/{id}")]
        public async Task<IActionResult> ReviewFeedback([FromRoute] int id, [FromBody] List<ReviewData> data)
        {
            var rs = await _service.SaveFeedback(id, data);
            return Ok(rs);
        }
        [Authorize]
        [HttpGet("feedback/{reviewId}")]
        public async Task<IActionResult> GetFeedBack([FromRoute] int reviewId)
        {
            var rs = await _service.LoadFeedback(reviewId);
            return Ok(rs);
        }
        [Authorize]
        [HttpPost("annotation")]
        public async Task<IActionResult> AddAnnotationAsync([FromBody] Annotations data)
        {
            var rs = await _service.AddAnnotationAsync(data);
            return Ok(rs);
        }
        [Authorize]
        [HttpPost("inTextComment/{docId}/{reviewId}")]
        public async Task<IActionResult> AddInTextCommentAsync([FromRoute] int docId, [FromRoute] int reviewId, [FromBody] InsertCommentModel data)
        {
            var rs = await _service.AddInTextCommentAsync(docId, reviewId, data.Comment, data.Annotation);
            return Ok(rs);
        }
        [Authorize]
        [HttpPost("comment/delete")]
        public async Task<IActionResult> DeleteInTextCommentAsync([FromBody] DeleteCommentModel data)
        {
            var rs = await _service.DeleteInTextCommentAsync(data.id);
            return Ok(rs);
        }
        [Authorize]
        [HttpPost("annotation/delete")]
        public async Task<IActionResult> DeleteAnnotationAsync([FromBody] DeleteCommentModel data)
        {
            var rs = await _service.DeleteAnnotationAsync(data.id);
            return Ok(rs);
        }
        [Authorize]
        [HttpPost("edit")]
        public async Task<IActionResult> EditAnotationAsync([FromBody] Annotations data)
        {
            var rs = await _service.EditAnnotationAsync(data);
            return Ok(rs);
        }
        [Authorize]
        [HttpPost("comment/edit")]
        public async Task<IActionResult> EditInTextCommentAsync([FromBody] InTextComments comment)
        {
            var rs = await _service.EditInTextComment(comment);
            return Ok(rs);
        }
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllReviewsAsync()
        {
            var rs = await _service.GetReviewsAsync();
            return Ok(rs);
        }
        [Authorize]
        [HttpPost("status/change/{id}")]
        public async Task<IActionResult> ChangeReviewStatusAsync([FromBody] UpdateStatusModel status, [FromRoute] int id)
        {
            var rs = await _service.ChangeStatusAsync(id, status.status);
            return Ok(rs);
        }
        [Authorize]
        [HttpGet("getById")]
        public async Task<IActionResult> GetReviewsByRaterIdAsync()
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var rs = await _service.GetReviewsByUserIdAsync(currentUser.Id);
            
            return Ok(rs);
        }
        [Authorize]
        [HttpPost("request")]
        public async Task<IActionResult> CreateReviewRequestAsync([FromBody] ReviewRequests request)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            request.UserId = currentUser.Id;
            var rs = await _service.CreateRequestAsync(request);
            return Ok(rs);
        }
        [Authorize]
        [HttpGet("byRaterId")]
        public async Task<IActionResult> GetReviewRequestsByIdAsync()
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var rs = await _service.GetRaterReviewsByIdAsync(currentUser.Id);
            return Ok(rs);
        }
        [Authorize]
        [HttpGet("request/{id}")]
        public async Task<IActionResult> GetOrCreateReviewByRequestId([FromRoute] int id)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var rs = await _service.GetOrCreateReviewByReviewRequestAsync(id, currentUser.Id);
            return Ok(rs);
        }
    }
}
