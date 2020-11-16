using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IRaterService
    {
        Task<IEnumerable<Rater>> GetAllAsync();
        Task<Rater> GetByIdAsync(int id);
        Task<Rater> CreateAsync(Rater rater);
        Task<Rater> UpdateAsync(Rater rater);
        Task<Rater> UpdateStatusAsync(int id, string status);
        Task<Rater> DeleteAsync(int id);
    }

    public class RaterService : BaseService,IRaterService
    {
        public RaterService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<Rater> CreateAsync(Rater rater)
        {
            return await _unitOfWork.Raters.Create(rater);
        }

        public async Task<Rater> DeleteAsync(int id)
        {
            return await _unitOfWork.Raters.Delete(id);
        }

        public async Task<IEnumerable<Rater>> GetAllAsync()
        {
            return await _unitOfWork.Raters.GetAllAsync();
        }

        public async Task<Rater> GetByIdAsync(int id)
        {
            return await _unitOfWork.Raters.GetByIdAsync(id, "Scores");
        }

        public async Task<Rater> UpdateAsync(Rater rater)
        {
            return await _unitOfWork.Raters.Update(rater);
        }
        public async Task<Rater> UpdateStatusAsync(int id, string status)
        {
            var rater = await _unitOfWork.Raters.GetByIdAsync(id);
            rater.Status = status;

            return await _unitOfWork.Raters.Update(rater);
        }
    }
}
