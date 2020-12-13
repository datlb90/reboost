using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
using Reboost.Service.Services;
using Reboost.Shared;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : BaseController<IQuestionsService>
    {
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionsService service, IMapper mapper) : base(service)
        {
            _mapper = mapper;
        }
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<QuestionModel>> GetAllAsync()
        {
            //return await _service.GetAllAsync();
            return await _service.GetAllExAsync();
        }

        [HttpGet("list/{userId}")]
        public async Task<IEnumerable<QuestionModel>> GetAllByUser(string userId)
        {
            return await _service.GetAllByUserAsync(userId);
        }

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

        [HttpGet]
        [Route("summary")]
        public async Task<Dictionary<string, int>> Summary()
        {
            return await _service.GetCountQuestionByTasksAsync();
        }


        [HttpGet]
        [Route("summary/tasks")]
        public async Task<List<Tasks>> SummaryTasks()
        {
            return await _service.GetTasksAsync();
        }
        [HttpGet]
        [Route("summaryPerUser/{userId}")]
        public async Task<List<SummaryPerUser>> GetSummaryByUserId([FromRoute] string userId)
        {
            return await _service.GetSummaryByUserId(userId);
        }
        [HttpGet]
        [Route("testForCurrentUsers/{userId}")]
        public async Task<List<string>> GetTestForCurrentUsers([FromRoute] string userId)
        {
            return await _service.GetTestForCurrentUsers(userId);
        }
        [HttpGet]
        [Route("questionCompletedIdByUser/{userId}")]
        public async Task<List<int>> GetQuestionCompletedIdByUser([FromRoute] string userId)
        {
            return await _service.GetQuestionCompletedIdByUser(userId);
        }
        [HttpGet]
        [Route("sampleForQuestion/{questionId}")]
        public async Task<List<SampleForQuestion>> GetSamplesForQuestion([FromRoute] int questionId)
        {
            return await _service.GetSamplesForQuestion(questionId);
        }
    }
}
