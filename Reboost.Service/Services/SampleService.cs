using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface ISampleService
    {
        Task<IEnumerable<Samples>> GetAllAsync();
        Task<Samples> GetByIdAsync(int id);
        Task<Samples> CreateAsync(Samples entity);
        Task<Samples> UpdateAsync(Samples entity);
        Task<Samples> DeleteAsync(int id);

    }
    public class SampleService : BaseService, ISampleService
    {
        public SampleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<Samples> CreateAsync(Samples entity)
        {
            return await _unitOfWork.Samples.Create(entity);
        }

        public async Task<Samples> DeleteAsync(int id)
        {
            return await _unitOfWork.Samples.Delete(id);
        }

        public async Task<IEnumerable<Samples>> GetAllAsync()
        {
            return await _unitOfWork.Samples.GetAllAsync();
        }

        public async Task<Samples> GetByIdAsync(int id)
        {
            return await _unitOfWork.Samples.GetByIdAsync(id);
        }

        public async Task<Samples> UpdateAsync(Samples entity)
        {
            return await _unitOfWork.Samples.Update(entity);
        }
    }

}
