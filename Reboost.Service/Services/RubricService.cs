using Microsoft.AspNetCore.Http;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IRubricService
    {
        Task<IEnumerable<Rubrics>> GetAllAsync();
        Task<Rubrics> GetByIdAsync(int id);
        Task<Rubrics> CreateAsync(Rubrics rubric);
        Task<Rubrics> UpdateAsync(Rubrics rubric);
        Task<Rubrics> DeleteAsync(int id);
        Task<List<RubricsModel>> getByQuestionId(int id);
    }
    public class RubricService : BaseService, IRubricService
    {
        public RubricService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<IEnumerable<Rubrics>> GetAllAsync()
        {
            return await _unitOfWork.Rubrics.GetAllAsync();
        }
        public async Task<Rubrics> GetByIdAsync(int id)
        {
            return await _unitOfWork.Rubrics.GetByIdAsync(id);
        }

        public async Task<Rubrics> CreateAsync(Rubrics r)
        {
            return await _unitOfWork.Rubrics.Create(r);
        }
        public async Task<Rubrics> UpdateAsync(Rubrics r)
        {
            return await _unitOfWork.Rubrics.Update(r);
        }
        public async Task<Rubrics> DeleteAsync(int id)
        {
            return await _unitOfWork.Rubrics.Delete(id);
        }

        public async Task<List<RubricsModel>> getByQuestionId(int id)
        {
            return await _unitOfWork.Rubrics.getByQuestionId(id);
        }
    }
}
