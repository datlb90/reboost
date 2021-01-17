using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
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

        [HttpGet]
        public async Task<IActionResult> Get() {
            var rs = await _service.GetAllAsync();
            return Ok(rs);
        }

        [HttpGet("getUserScore/{userId}")]
        public async Task<IActionResult> GetUserScore([FromRoute] string userId)
        {
            var rs = await _service.GetUserScores(userId);
            return Ok(rs);
        }
        [HttpPost("addScore/{userId}")]
        public async Task<IActionResult> AddScores([FromRoute] string userId, [FromBody] List<UserScores> userScores)
        {
            return Ok(await _service.AddScoreAsync(userId, userScores));
        }
        //[HttpPost]
        //public async Task<IActionResult> Post(ApplicationUser user)
        //{
        //    return Ok(await _service.Update(user));
        //}

        [HttpGet("hasSubmissionOnTaskOf/{userId}/{questionId}")]
        public async Task<IActionResult> HasSubmissionOnTaskOf([FromRoute] string userId, [FromRoute] int questionId)
        {
            return Ok(await _service.HasSubmissionOnTaskOf(userId, questionId));
        }
    }
}
