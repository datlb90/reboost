using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess;
using Reboost.Service.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : BaseController<ISubmissionService>
    {
        
        private ReboostDbContext db;

        public SubmissionController(ISubmissionService service, ReboostDbContext ctx) : base(service)
        {
            db = ctx;
        }
        //[Authorize]
        [HttpGet("getByUser/{userId}/{questionId}")]
        public async Task<IActionResult> GetByUser([FromRoute] string userId, [FromRoute] int questionId)
        {
            var query = from sub in db.Submissions
                         join rev in db.Reviews on sub.Id equals rev.SubmissionId
                         join doc in db.Documents on sub.DocId equals doc.Id
                        where sub.QuestionId == questionId && sub.UserId == userId
                        select new { SubmittedDate = sub.SubmittedDate.ToString("MM/dd/yyyy hh:mm tt"), Id = sub.Id, ReviewId = rev.Id, DocId = doc.Id };

            var rs = await Task.FromResult(query);

            return Ok(rs);
        }
    }
}
