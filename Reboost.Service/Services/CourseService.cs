using System;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reboost.DataAccess;

namespace Reboost.Service.Services
{
    public interface ICourseService
    {
        Task<Courses> getCourseByUrlTitle(string urlTitle);
    }

    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Courses> getCourseByUrlTitle(string urlTitle)
        {
            return await _unitOfWork.Courses.getCourseByUrlTitle(urlTitle);
        }
    }
}

