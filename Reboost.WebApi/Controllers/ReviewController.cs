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
        public ReviewController(IReviewService service) : base(service)
        {

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
    }

    

}
