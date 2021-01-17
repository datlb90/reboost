using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : BaseController<IDiscussionService>
    {
        private readonly IMapper _mapper;

        public DiscussionController(IDiscussionService service, IMapper mapper) : base(service)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Discussion>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IEnumerable<Discussion>> GetAllByQuestionIdAsync(int id)
        {
            return await _service.GetAllByQuestionIdAsync(id);
        }

        [HttpGet]
        [Route("get-id/{id}")]
        public async Task<Discussion> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Discussion> DeleteAsync(int id)
        {
            return await _service.DeleteAsync(id);
        }

        [HttpPost]
        [Route("create")]
        public async Task<Discussion> CreateAsync([FromBody] Discussion model)
        {
            var _discus = _mapper.Map<Discussion>(model);
            return await _service.CreateAsync(_discus);
        }

        [HttpPost]
        [Route("update")]
        public async Task<Discussion> UpdateAsync([FromBody] Discussion model)
        {
            var _discus = _mapper.Map<Discussion>(model);
            return await _service.UpdateAsync(_discus);
        }

        [HttpGet]
        [Route("getAllTags")]
        public async Task<List<Tags>> GetTagsAsync()
        {
            return await _service.GetAllTagsAsync();
        }
        [HttpGet]
        [Route("getComments/{id}")]
        public async Task<List<DiscussionModel>> GetComments(int id)
        {
            return await _service.GetComments(id);
        }
        [HttpGet]
        [Route("increaseView/{id}")]
        public async Task<Discussion> IncreaseViewAsync(int id)
        {
            return await _service.IncreaseViewAsync(id);
        }
        [HttpPost]
        [Route("upVote")]
        public async Task<Discussion> UpVoteAsync([FromBody] VoteModel voteModel)
        {
            return await _service.UpVoteAsync(voteModel);
        }
        [HttpPost]
        [Route("downVote")]
        public async Task<Discussion> DownVoteAsync([FromBody] VoteModel voteModel)
        {
            return await _service.DownVoteAsync(voteModel);
        }
    }
}
