using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IScoreService
    {
        Task<List<UserScore>> GetAllAsync();
        Task<UserScore> GetByIdAsync(int id);
        Task<UserScore> CreateAsync(UserScore score);
        Task<UserScore> UpdateAsync(UserScore score);
        Task<UserScore> DeleteAsync(int id);
    }
    public class ScoreService : BaseService, IScoreService
    {
        public ScoreService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<UserScore> CreateAsync(UserScore score)
        {
            return await _unitOfWork.Scores.CreateAsync(score);
        }

        public async Task<UserScore> DeleteAsync(int id)
        {
            return await _unitOfWork.Scores.DeleteAsync(id);
        }

        public async Task<List<UserScore>> GetAllAsync()
        {
            return await _unitOfWork.Scores.GetAllAsync();
        }

        public async Task<UserScore> GetByIdAsync(int id)
        {
            return await _unitOfWork.Scores.GetByIdAsync(id);
        }

        public async Task<UserScore> UpdateAsync(UserScore Score)
        {
            return await _unitOfWork.Scores.UpdateAsync(Score);
        }
    }
}
