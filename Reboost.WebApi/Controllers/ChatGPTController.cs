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

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGPTController : BaseController<IChatGPTService>
    {
        public ChatGPTController(IChatGPTService service) : base(service){ }

        [HttpGet("getEssayFeedback")]
        public async Task<IActionResult> GetEssayFeedback()
        {
            return Ok();
        }

        [HttpGet("question/difficulty")]
        public async Task GetDifficultyLevel()
        {
            await _service.GetDifficultyLevelForAllQuestions();
        }
    }
}

