using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reboost.DataAccess.Entities;
using Reboost.Service.Services;

namespace Reboost.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [Route("title/{urlTitle}")]
        public async Task<Courses> getCourseByUrlTitle(string urlTitle)
        {
            return await _courseService.getCourseByUrlTitle(urlTitle);
        }
    }
}

