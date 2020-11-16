using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        Task<List<Photo>> GetAllAsync();
        Task<Photo> GetByIdAsync(int id);
        Task<Photo> CreateAsync(Photo photo);
        Task<Photo> UpdateAsync(Photo photo);
        Task<Photo> DeleteAsync(int id);
    }
    public class PhotoRepository : BaseRepository, IPhotoRepository
    {
        public PhotoRepository(ReboostDbContext context) : base(context)
        { }

        public async Task<Photo> CreateAsync(Photo photo)
        {
            Context.Photos.Add(photo);
            await Context.SaveChangesAsync();
            return photo;
        }

        public async Task<Photo> DeleteAsync(int id)
        {
            Photo _photo = await Context.Photos.SingleOrDefaultAsync(r => r.Id == id);
            Context.Photos.Remove(_photo);
            await Context.SaveChangesAsync();
            return _photo;
        }

        public async Task<List<Photo>> GetAllAsync()
        {
            return await Context.Photos.ToListAsync();
        }

        public async Task<Photo> GetByIdAsync(int id)
        {
            return await Context.Photos.Where(r => r.Id == id).FirstAsync();
        }

        public async Task<Photo> UpdateAsync(Photo photo)
        {
            Photo _photo = await Context.Photos.SingleOrDefaultAsync(r => r.Id == photo.Id);
            _photo.UserId = photo.UserId;
            _photo.File = photo.File;
            _photo.FileName = photo.FileName;
            _photo.FileType = photo.FileType;
            await Context.SaveChangesAsync();
            return _photo;
        }
    }
}
