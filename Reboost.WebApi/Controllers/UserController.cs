using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;
using Reboost.WebApi.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController<IUserService>
    {
        public UserController(IUserService service) : base(service)
        {

        }

        [HttpPost("support")]
        public async Task<IActionResult> SendSupportEmail([FromForm] ContactRequestModel model)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");

            var userName = "Quest";
            if (email != null)
            {
                var user = await _service.GetByEmailAsync(email.Value);
                userName = user.Username;
            }

            var rs = await _service.SendSupportEmail(userName, model);

            return Ok(rs);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get() {
            var rs = await _service.GetAllAsync();
            return Ok(rs);
        }
        [Authorize]
        [HttpGet("getUserScore/{userId}")]
        public async Task<IActionResult> GetUserScore([FromRoute] string userId)
        {
            var rs = await _service.GetUserScores(userId);
            return Ok(rs);
        }
        [Authorize]
        [HttpPost("addScore/{userId}")]
        public async Task<IActionResult> AddScores([FromRoute] string userId, [FromBody] List<UserScores> userScores)
        {
            return Ok(await _service.AddScoreAsync(userId, userScores));
        }
        [Authorize]
        [HttpGet("hasSubmissionOnTaskOf/{userId}/{questionId}")]
        public async Task<IActionResult> HasSubmissionOnTaskOf([FromRoute] string userId, [FromRoute] int questionId)
        {
            return Ok(await _service.HasSubmissionOnTaskOf(userId, questionId));
        }
        [Authorize]
        [HttpGet("{userId}/rate")]
        public async Task<IActionResult> GetUserRate([FromRoute] string userId)
        {
            return Ok(await _service.GetUserRate(userId));
        }
    }
}
