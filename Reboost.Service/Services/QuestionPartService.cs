using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IQuestionPartService
    {
        Task<IEnumerable<QuestionParts>> GetAllAsync();
        Task<QuestionParts> GetByIdAsync(int id);
        Task<QuestionParts> CreateAsync(QuestionParts entity);
        Task<QuestionParts> UpdateAsync(QuestionParts entity);
        Task<QuestionParts> DeleteAsync(int id);

    }
    public class QuestionPartService : BaseService, IQuestionPartService
    {
        public QuestionPartService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<QuestionParts> CreateAsync(QuestionParts entity)
        {
            return await _unitOfWork.QuestionParts.Create(entity);
        }

        public async Task<QuestionParts> DeleteAsync(int id)
        {
            return await _unitOfWork.QuestionParts.Delete(id);
        }

        public async Task<IEnumerable<QuestionParts>> GetAllAsync()
        {
            return await _unitOfWork.QuestionParts.GetAllAsync();
        }

        public async Task<QuestionParts> GetByIdAsync(int id)
        {
            return await _unitOfWork.QuestionParts.GetByIdAsync(id);
        }

        public async Task<QuestionParts> UpdateAsync(QuestionParts entity)
        {
            return await _unitOfWork.QuestionParts.Update(entity);
        }
    }

}
