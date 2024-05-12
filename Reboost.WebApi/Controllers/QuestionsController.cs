using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;
using Reboost.Shared;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : BaseController<IQuestionsService>
    {
        private readonly IMapper _mapper;
        private IUserService _userService;
        private ISampleService _sampleService;
        private ILogger<QuestionsController> _logger;

        public QuestionsController(IQuestionsService service, IMapper mapper,
            IUserService userService, ISampleService sampleService, ILogger<QuestionsController> logger) : base(service)
        {
            _mapper = mapper;
            _userService = userService;
            _sampleService = sampleService;
            _logger = logger;
        }

        [HttpPost("image/to/text")]
        public async Task<ImageToTopicAndEssayModel> getWritingTextFromImage()
        {
            ImageToTopicAndEssayModel result = new ImageToTopicAndEssayModel
            {
                essay = "Không thể trích xuất từ ảnh",
                topic = "Không thể trích xuất từ ảnh",
            };
            IFormFile file = HttpContext.Request.Form.Files.Count() > 0 ? HttpContext.Request.Form.Files[0] : null;
            if(file != null)
            {
                try
                {
                    _logger.LogInformation("Image to text requested");
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        byte[] imageData = ms.ToArray();
                        return await _service.getWritingTextFromImage(imageData);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Cannot process Image to text request: ", e.InnerException.Message);
                    return result;
                }
                
            }
            return result;
        }

        [HttpGet("initial/test")]
        public async Task<List<InitQuestionModel>> GetInitQuestionModels()
        {
            return await _service.GetInitQuestionModels();
        }

        [HttpPost("peronsal/submission")]
        public async Task<IActionResult> CreatePersonalSubmission([FromForm] RequestReviewForWriting model)
        {
            //var currentUserClaim = HttpContext.User;
            //var email = currentUserClaim.FindFirst("Email");
            //var currentUser = await _userService.GetByEmailAsync(email.Value);

            var submission = await _service.CreatePersonalSubmission(model);

            return Ok(submission);
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<QuestionModel>> GetAllAsync()
        {
            return await _service.GetAllExAsync();
        }
        [Authorize]
        [HttpGet("list/{userId}")]
        public async Task<IEnumerable<QuestionModel>> GetAllByUser(string userId)
        {
            return await _service.GetAllByUserAsync(userId);
        }

        [Authorize]
        [HttpGet]
        [Route("getById/{id}")]
        public async Task<QuestionModel> GetByIdAsync([FromRoute] int id)
        {
            //return await _service.GetAllAsync();
            var data = await _service.GetByIdAsync(id);
            if (data.Id == id)
            {
                return data;
            }
            return null;
        }
        [Authorize]
        [HttpGet]
        [Route("summary")]
        public async Task<Dictionary<string, int>> Summary()
        {
            return await _service.GetCountQuestionByTasksAsync();
        }

        [Authorize]
        [HttpGet]
        [Route("summary/tasks")]
        public async Task<List<Tasks>> SummaryTasks()
        {
            return await _service.GetTasksAsync();
        }
        [Authorize]
        [HttpGet]
        [Route("summaryPerUser/{userId}")]
        public async Task<List<SummaryPerUser>> GetSummaryByUserId([FromRoute] string userId)
        {
            return await _service.GetSummaryByUserId(userId);
        }
        [Authorize]
        [HttpGet]
        [Route("testForCurrentUsers/{userId}")]
        public async Task<List<string>> GetTestForCurrentUsers([FromRoute] string userId)
        {
            return await _service.GetTestForCurrentUsers(userId);
        }
        [Authorize]
        [HttpGet]
        [Route("questionCompletedIdByUser/{userId}")]
        public async Task<List<int>> GetQuestionCompletedIdByUser([FromRoute] string userId)
        {
            return await _service.GetQuestionCompletedIdByUser(userId);
        }

        //[Authorize]
        [HttpGet]
        [Route("sampleForQuestion/{questionId}")]
        public async Task<List<SampleForQuestion>> GetSamplesForQuestion([FromRoute] int questionId)
        {
            return await _service.GetSamplesForQuestion(questionId);
        }
        [Authorize]
        [HttpGet]
        [Route("submissions/{userId}")]
        public async Task<List<SubmissionsModel>> GetAllSubmissionsByUserId([FromRoute] string userId)
        {
            return await _service.GetAllSubmissionByUserId(userId);
        }
        [Authorize]
        [HttpGet]
        [Route("submissions/{userId}/{questionId}")]
        public async Task<List<SubmissionsForQuestionModel>> GetSubmissionsForQuestion(string userId, int questionId)
        {
            return await _service.GetSubmissionsForQuestion(userId, questionId);
        }
        [Authorize]
        [HttpGet]
        [Route("add-edit/data")]
        public async Task<QuestionDataModel> GetDataForAddOrEditQuestionAsync()
        {
            return await _service.GetAllDataForAddOrEditQuestion();
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateNewQuestionAsync([FromForm] QuestionRequestModel model)
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            model.UserId = currentUser.Id;
            var _question = _mapper.Map<Questions>(model);
            var result = await _service.CreateQuestionAsync(_question, model);
            
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateQuestionAsync([FromForm] QuestionRequestModel model)
        {
            model.LastActivityDate = DateTime.UtcNow;
            var _question = _mapper.Map<Questions>(model);
            var result = await _service.UpdateQuestionAsync(_question, model);

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("publish/{id}")]
        public async Task<IActionResult> PublishQuestionAsync([FromRoute] int id)
        {
            var rs = await _service.PublishQuestionAsync(id);
            return Ok(rs);
        }

        [Authorize]
        [HttpGet]
        [Route("approve/{id}")]
        public async Task<IActionResult> ApproveQuestionAsync([FromRoute] int id)
        {
            var rs = await _service.ApproveQuestionAsync(id);
            return Ok(rs);
        }

        [Authorize] 
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int id)
        {
            var rs = await _service.DeleteAsync(id);
            return Ok(rs);

        }
    }
}
