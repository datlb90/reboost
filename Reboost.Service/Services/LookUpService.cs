using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface ILookUpService
    {
        Task<IEnumerable<LookUp>> GetAllAsync();
        Task<LookUp> GetByIdAsync(int id);
        Task<List<LookUp>> GetByType(string type);
    }
    public class LookUpService : BaseService, ILookUpService
    {
        public LookUpService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<IEnumerable<LookUp>> GetAllAsync()
        {
            return await _unitOfWork.LookUps.GetAllAsync();
        }

        public async Task<LookUp> GetByIdAsync(int id)
        {
            return await _unitOfWork.LookUps.GetByIdAsync(id);
        }

        public async Task<List<LookUp>> GetByType(string type)
        {
            return await _unitOfWork.LookUps.GetByType(type);
        }
    }
}
