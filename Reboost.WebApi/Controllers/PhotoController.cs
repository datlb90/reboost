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
    public class PhotoController : BaseController<IPhotoService>
    {
        public PhotoController(IPhotoService service) : base(service)
        {

        }
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Photo>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Photo> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Photo> DeleteAsync(int id)
        {
            return await _service.DeleteAsync(id);
        }
        [HttpPost]
        [Route("create")]
        public async Task<Photo> CreateAsync(Photo photo)
        {
            return await _service.CreateAsync(photo);
        }
        [HttpPost]
        [Route("update")]
        public async Task<Photo> UpdateAsync(Photo photo)
        {
            return await _service.UpdateAsync(photo);
        }
    }
}
