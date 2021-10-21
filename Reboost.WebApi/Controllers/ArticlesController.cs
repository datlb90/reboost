using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ArticlesController : BaseController<IArticlesService>
    {
        public ArticlesController(IArticlesService service) : base(service)
        {
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateNewArticlesAsync([FromBody] CreateArticlesModel data)
        {
            var rs = await _service.CreateNewArticleAsync(data);
            return Ok(rs);
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllArticlesAsync()
        {
            var rs = await _service.GetAllArticlesAsync();
            return Ok(rs);
        }

        [Authorize]
        [HttpGet]
        [Route("labels")]
        public async Task<IActionResult> GetAllArticleLabelAsync()
        {
            var rs = await _service.GetAllArticleLabelsAsync();
            return Ok(rs);
        }

        [Authorize]
        [HttpGet]
        [Route("relations")]
        public async Task<IActionResult> GetAllArticleRelationAsync()
        {
            var rs = await _service.GetAllArticleRelationsAsync();
            return Ok(rs);
        }
    }
}
