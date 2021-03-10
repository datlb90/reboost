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
    public class RubricController : BaseController<IRubricService>
    {
        private readonly IMapper _mapper;
        public RubricController(IRubricService service, IMapper mapper) : base(service)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Rubrics>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Rubrics> GetByIdAsync(int id)
        {
            var rater = await _service.GetByIdAsync(id);
            return rater;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Rubrics> DeleteAsync(int id)
        {
            return await _service.DeleteAsync(id);
        }

        [HttpPost]
        [Route("create")]
        public async Task<Rubrics> CreateAsync([FromForm] Rubrics model)
        {
            var _rubric = _mapper.Map<Rubrics>(model);
            return await _service.CreateAsync(_rubric);
        }

        [HttpPost]
        [Route("update")]
        public async Task<Rubrics> UpdateAsync([FromForm] Rubrics model)
        {
            var _rubric = _mapper.Map<Rubrics>(model);
            return await _service.UpdateAsync(_rubric);
        }
        [HttpGet]
        [Route("getByQuestionId/{id}")]
        public async Task<List<RubricsModel>> GetByQuestionId([FromRoute] int id)
        {
            return await _service.GetByQuestionId(id);
        }
    }
}
