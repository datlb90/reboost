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
    public class LookUpController : BaseController<ILookUpService>
    {
        public LookUpController(ILookUpService service) : base(service)
        {

        }
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<LookUp>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<LookUp> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }
        [HttpGet]
        [Route("getByType/{type}")]
        public async Task<List<LookUp>> GetByIdAsync(string type)
        {
            return await _service.GetByType(type);
        }
    }
}
