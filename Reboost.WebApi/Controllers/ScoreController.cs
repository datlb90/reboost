using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
using Reboost.Service.Services;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : BaseController<IScoreService>
    {
        public ScoreController(IScoreService service) : base(service)
        {

        }
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<UserScore>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<UserScore> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<UserScore> DeleteAsync(int id)
        {
            return await _service.DeleteAsync(id);
        }
        [HttpPost]
        [Route("create")]
        public async Task<UserScore> CreateAsync(UserScore Score)
        {
            return await _service.CreateAsync(Score);
        }
        [HttpPost]
        [Route("update")]
        public async Task<UserScore> UpdateAsync(UserScore Score)
        {
            return await _service.UpdateAsync(Score);
        }
    }
}
