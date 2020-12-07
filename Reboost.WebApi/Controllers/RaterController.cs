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
    public class RaterController : BaseController<IRaterService>
    {
        private readonly IMapper _mapper;

        public RaterController(IRaterService service, IMapper mapper) : base(service)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Raters>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Raters> GetByIdAsync(int id)
        {
            var rater = await _service.GetByIdAsync(id);
            var applyTo = await _service.GetApplyTo(id);
            var raterModel = _mapper.Map<RaterResponseModel>(rater);
            raterModel.ApplyTo = applyTo;

            return raterModel;
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Raters> DeleteAsync(int id)
        {
            return await _service.DeleteAsync(id);
        }
        [HttpPost]
        [Route("create")]
        public async Task<Raters> CreateAsync([FromForm] RaterRequestModel model)
        {
            var _rater = _mapper.Map<Raters>(model);
            return await _service.CreateAsync(_rater, model.UploadedFiles);
        }

        [HttpPost]
        [Route("update")]
        public async Task<Raters> UpdateAsync([FromForm] RaterRequestModel model)
        {
            var _rater = _mapper.Map<Raters>(model);
            return await _service.UpdateAsync(_rater, model.UploadedFiles);
        }
        
        [HttpGet("update/status/{id}/{status}")]
        public async Task<Raters> UpdateStatus(int id, string status) {
            return await _service.UpdateStatusAsync(id, status);
        }
    }
}
