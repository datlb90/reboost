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
        private IUserService _userService;
        public ArticlesController(IArticlesService service, IUserService userService) : base(service)
        {
            _userService = userService;
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

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetArticleById([FromRoute] int id)
        {
            // Get current user info
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            string userId = null;
            if(email != null)
            {
                var currentUser = await _userService.GetByEmailAsync(email.Value);
                userId = currentUser.Id;
            }

            var rs = await _service.GetArticleById(id, userId);
            return Ok(rs);
        }

        [Authorize]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateArticleAsync([FromRoute] int id, [FromBody] CreateArticlesModel data)
        {
            var rs = await _service.UpdateArticleAsync(id, data);
            return Ok(rs);
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteArticleAsync([FromRoute] int id)
        {
            var rs = await _service.DeleteArticleAsync(id);
            return Ok(rs);
        }
    }
}
