using System;
using Reboost.DataAccess.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Reboost.DataAccess.Repositories
{
    public interface ICourseRepository : IRepository<Courses>
    {
        Task<Courses> getCourseByUrlTitle(string urlTitle);
    }

    public class CourseRepository : BaseRepository<Courses>, ICourseRepository
    {
        public CourseRepository(ReboostDbContext context)
           : base(context)
        { }

        public async Task<Courses> getCourseByUrlTitle(string urlTitle)
        {
            return await ReboostDbContext.Courses
                        .Where(c => c.UrlTitle == urlTitle)
                        .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.Lessons)
                        .FirstOrDefaultAsync();
        }

        private ReboostDbContext ReboostDbContext
        {
            get { return context as ReboostDbContext; }
        }
    }
}

