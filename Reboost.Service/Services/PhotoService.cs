using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IPhotoService
    {
        Task<List<Photo>> GetAllAsync();
        Task<Photo> GetByIdAsync(int id);
        Task<Photo> CreateAsync(Photo photo);
        Task<Photo> UpdateAsync(Photo photo);
        Task<Photo> DeleteAsync(int id);
    }
    public class PhotoService : BaseService, IPhotoService
    {
        public PhotoService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<Photo> CreateAsync(Photo photo)
        {
            return await _unitOfWork.Photos.CreateAsync(photo);
        }

        public async Task<Photo> DeleteAsync(int id)
        {
            return await _unitOfWork.Photos.DeleteAsync(id);
        }

        public async Task<List<Photo>> GetAllAsync()
        {
            return await _unitOfWork.Photos.GetAllAsync();
        }

        public async Task<Photo> GetByIdAsync(int id)
        {
            return await _unitOfWork.Photos.GetByIdAsync(id);
        }

        public async Task<Photo> UpdateAsync(Photo photo)
        {
            return await _unitOfWork.Photos.UpdateAsync(photo);
        }
    }
}
