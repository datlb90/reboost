using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
using Reboost.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : BaseController<ISampleService>
    {
        private ISampleService _service;
        public SampleController(ISampleService service) : base(service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateSampleAsync([FromBody] Samples data)
        {
            var rs = await _service.CreateAsync(data);
            return Ok(rs);
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllSamplesAsync()
        {
            var rs = await _service.GetAllAsync();
            return Ok(rs);
        }

        [Authorize]
        [HttpGet]
        [Route("approve/{id}")]
        public async Task<IActionResult> ApproveSampleAsync([FromRoute] int id)
        {
            var rs = await _service.ApproveSampleByIdAsync(id);
            return Ok(rs);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteSample([FromRoute] int id)
        {
            var rs = await _service.DeleteAsync(id);
            return Ok(rs);
        }

    }
}
