using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;
using Reboost.Shared;
using Hangfire;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : BaseController<IReviewService>
    {

        private IUserService _userService;
        private IRaterService _raterService;
        private IMailService _mailService;
        private ReboostDbContext db;

        public ReviewController(IReviewService service, IUserService userService, IRaterService raterService, IMailService mailService, ReboostDbContext ctx ) : base(service)
        {
            _userService = userService;
            _raterService = raterService;
            _mailService = mailService;
            db = ctx;
        }
        [Authorize]
        [HttpGet("getAnnotation/{docId}/{reviewId}")]
        public async Task<IActionResult> GetAnnotationsAsync([FromRoute] int docId, [FromRoute] int reviewId)
        {
            var rs = await _service.GetAnnotationsAsync(docId, reviewId);
            var review = await _service.GetReviewByIdAsync(reviewId);
            var user = await _userService.GetByIdAsync(review.Review.ReviewerId);

            rs.User = user;

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
        [HttpGet("reviewee/{reviewId}/auth")]
        public async Task<IActionResult> RevieweeReviewAuthentication([FromRoute] int reviewId)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var role = currentUserClaim.FindFirst("Role");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var check = await _service.CheckReviewValidationAsync(role.Value,currentUser, reviewId);
            if (check!=0)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(new { message = "Review not found!" });
            }
        }
        [Authorize]
        [HttpPost("rater/auth")]
        public async Task<IActionResult> RaterApprovedCheck()
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            if (currentUser != null)
            {
                var rater = await _raterService.GetByUserIdAsync(currentUser.Id);
                return Ok(rater);
            }
            return BadRequest(new { message = "User not found!" });
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
        [HttpPost("createReviewTraining/{type}")]
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
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            // Check if it is a pro request, return 1 if is a pro request, -1 if it not a pro request, 0 if review's timeout end
            var isProRequest = await _service.IsProRequestCheckAsync(id, currentUser.Id);

            // Check rater's permission
            if (isProRequest == 2)
            {
                return BadRequest(new { message = "You don't have permission to this review request!" });
            }

            // Check if pro review's timeout end
            if (isProRequest == 0)
            {
                return BadRequest(new { message = "Your review's timeout has ended!" });
            }

            var rs = await _service.SaveFeedback(id, data);

            // Send mail if review is not a training review
            if(rs != null && rs.RequestId!=0)
            {
                // Get reviewee's Id
                var reviewee = await _userService.GetByIdAsync(rs.RevieweeId);

                // Get review's info by reviewee's Id
                var result = await _service.GetOrCreateReviewByReviewRequestAsync(rs.RequestId, reviewee.Id);

                // Send mail to reviewee
                await _mailService.SendEmailAsync(reviewee.Email, "Review Update", "Your submission has just been updated! Follow the link at: localhost:3011/review/"+result.ReviewRequest.Submission.QuestionId+"/"+result.ReviewRequest.Submission.DocId+"/"+result.ReviewId);
            }

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
        [HttpPost("training/status/change/{id}")]
        public async Task<IActionResult> ChangeReviewStatusAsync([FromBody] UpdateStatusModel model, [FromRoute] int id)
        {
            var rs = await _service.ChangeStatusAsync(id, model);
            if (rs != null && rs.SendEmail != null)
            {
                await _mailService.SendEmailAsync(rs.RaterEmail, rs.EmailSubject, rs.EmailContent);
            }
            
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewByIdAsync(int id)
        {
            var rs = await _service.GetReviewByIdAsync(id);
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

            var exist = await _service.GetReviewRequestBySubmissionId(request.SubmissionId, currentUser.Id);
            if(exist!= null)
            {
                return Ok(exist);
            }

            var rs = await _service.CreateRequestAsync(currentUser.Id, request);
            
            var unrate = await _service.GetUnRatedReviewsAsync(currentUser.Id);
            
            if(unrate.Count() > 0)
            {
                return Ok(rs);
            }

            //Add new request to request queue
            if(request.FeedbackType == ReviewRequestType.FREE)
            {
                await _service.AddRequestQueue(new RequestQueue { RequestId = rs.Id, RequestedDatetime = DateTime.Now }, currentUser.Id);
            }
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

        [Authorize]
        [HttpGet("submission/{id}")]
        public async Task<IActionResult> GetOrCreateReviewBySubmissionId([FromRoute] int id)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var request = await _service.GetReviewRequestBySubmissionId(id, currentUser.Id);
            if (request == null)
            {
                request = await _service.CreateRequestAsync(currentUser.Id, new ReviewRequests() { UserId = currentUser.Id, SubmissionId = id } );
                if(request.FeedbackType == ReviewRequestType.FREE)
                {
                    await _service.AddRequestQueue(new RequestQueue { RequestId = request.Id, RequestedDatetime = DateTime.Now }, currentUser.Id);
                }
            }
            var rs = await _service.GetOrCreateReviewByReviewRequestAsync(request.Id, currentUser.Id);
            return Ok(rs);
        }

        [Authorize]
        [HttpPost("rate")]
        public async Task<IActionResult> CreateReviewRatingAsync([FromBody] ReviewRatings data)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);
            
            var rs = await _service.CreateReviewRatingAsync(data, currentUser.Id);
            return Ok(rs);
        }
        [Authorize]
        [HttpGet("rate/{reviewId}")]
        public async Task<IActionResult> GetReviewRatingByReviewIdAsync([FromRoute] int reviewId)
        {
            var rs = await _service.GetReviewRatingsByReviewIdAsync(reviewId);
            return Ok(rs);
        }
        [Authorize]
        [HttpGet("new-review")]
        public async Task<IActionResult> NewReview() {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);
            var review = await _service.CreateReviewFromQueue(currentUser.Id);
            return Ok(review);
        }

        [Authorize]
        [HttpGet("unrated")]
        public async Task<IActionResult> GetUnratedReviews()
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var unrated = await _service.GetUnRatedReviewsAsync(currentUser.Id);
            return Ok(unrated);
        }

        [Authorize]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingReview()
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);
            var pending = await _service.GetPendingReviewAsync(currentUser.Id);
            return Ok(pending);
        }

        //Just for testing
        [HttpGet("{reviewId}/rate/info")]
        public async Task<IActionResult> GetRatingInfo(int reviewId)
        {
            var rs = db.ReviewRatings.Where(r => r.ReviewId == reviewId).FirstOrDefault();
            if (rs == null)
            {
                Ok(await Task.FromResult(new { code = 400, messages = "Id not found" }));
            }
            return Ok(await Task.FromResult(rs));
        }

        [Authorize]
        [HttpGet("getRequest/{submissionId}")]
        public async Task<IActionResult> GetRequestBySubmissionId(int submissionId)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);
            
            var rs = await _service.GetReviewRequestBySubmissionId(submissionId, currentUser.Id);
            return Ok(rs);
        }


        [Authorize]
        [HttpPost("request/pro")]
        public async Task<IActionResult> CreateProRequestAsync([FromBody] ReviewRequests request)
        {
            // Get current user info
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            request.UserId = currentUser.Id;

            // Get request if exist
            var exist = await _service.GetReviewRequestBySubmissionId(request.SubmissionId, currentUser.Id);

            if (exist != null)
            {
                // Get rater and update requested time of queue
                var rs = await _service.ReRequestProRequestAsync(exist);

                return Ok(rs.Request);
            }
            else
            {
                // Add request then return the rater that applied to this request
                var rs = await _service.CreateProRequestAsync(request);

                if (rs == null)
                {
                    return BadRequest(new { message = "No rater available!" });
                }

                return Ok(rs.Request);
            }
        }

        [Authorize]
        [HttpGet("request/pro/{id}")]
        public async Task<IActionResult> GetOrCreateReviewByProRequest(int id)
        {
            // Get current user info
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var rs = await _service.GetOrCreateReviewByProRequestId(id, currentUser.Id);

            if(rs.Error?.Length > 0)
            {
                return BadRequest(new { message = rs.Error });
            }

            return Ok(rs);
        }

        [Authorize]
        [HttpGet("training/{id}")]
        public async Task<IActionResult> GetRaterTrainingAsync(int id)
        {
            var rs = await _service.GetRaterTrainingsAsync(id);
            return Ok(rs);
        }

        [Authorize]
        [HttpPost("dispute")]
        public async Task<IActionResult> CreateDisputeAsync([FromBody] Disputes dispute)
        {
            // Get current user info
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            dispute.UserId = currentUser.Id;
            dispute.Status = DisputeStatus.OPEN;

            var rs = await _service.CreateDisputeAsync(dispute);
            return Ok(rs);
        }

        [Authorize]
        [HttpGet("dispute")]
        public async Task<IActionResult> GetAllDisputesAsync()
        {
            var rs = await _service.GetAllDisputesAsync();
            return Ok(rs);
        }

        [Authorize]
        [HttpGet("dispute/{id}")]
        public async Task<IActionResult> GetByReviewIdAsync(int id)
        {
            var rs = await _service.GetDisputeByReviewIdAsync(id);
            return Ok(rs);
        }

        [Authorize]
        [HttpPost("dispute/update")]
        public async Task<IActionResult> UpdateDisputeAsync([FromBody] Disputes dispute)
        {
            var rs = await _service.UpdateDisputeAsync(dispute);
            return Ok(rs);
        }

        [Authorize]
        [HttpGet("dispute/learner")]
        public async Task<IActionResult> GetAllLearnerDisputes()
        {
            // Get current user info
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var rs = await _service.GetAllLearnerDisputesAsync(currentUser.Id);

            return Ok(rs);
        }
    }
}
