using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task<User> AddScoreAsync(string userId, List<UserScores> userScores);
        Task<List<UserScores>> GetUserScores(string userId);

    }

    public class UserService: BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task<User> AddScoreAsync(string userId,List<UserScores> userScores)
        {
            return await _unitOfWork.Users.UpdateScoreAsync(userId,userScores);
        }
        public async Task<List<UserScores>> GetUserScores(string userId)
        {
            return await _unitOfWork.Users.GetUserScores(userId);
        }

        public async Task<User> GetByIdAsync(string id) {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }
    }
}
