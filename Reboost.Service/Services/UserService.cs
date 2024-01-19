using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IUserService
    {
        Task<ContactRequestModel> SendSupportEmail(string userName, ContactRequestModel model);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<User> AddScoreAsync(string userId, List<UserScores> userScores);
        Task<List<UserScores>> GetUserScores(string userId);
        Task<bool> HasSubmissionOnTaskOf(string userId, int questionId);
        Task<User> UpdateStripeIdAsync(User user, string id);
        Task<UserRanks> GetUserRate(string userId);
    }

    public class UserService: BaseService, IUserService
    {
        public IMailService mailService;
        public UserService(IUnitOfWork unitOfWork, IMailService _mailService) : base(unitOfWork)
        {
            mailService = _mailService;
        }
        public async Task<ContactRequestModel> SendSupportEmail(string userName, ContactRequestModel model)
        {
            List<string> listPaths = new List<string>();

            if (model.UploadedFiles != null && model.UploadedFiles.Count > 0)
            {
                foreach (var item in model.UploadedFiles)
                {
                    // File size must not exceed 5MB
                    if (item.Length > 3000000)
                    {
                        return null;
                    }
                }
            }
            string subject = "Reboost Customer Support Ticket";

            var content = $"<h1>Thank you for contacting us!</h1>" +
                            $"<p>We will respond as soon as we can with the following query:</p>" +
                            $"<p><strong>Customer name: </strong>" + model.Fullname + "/<p>" +
                            $"<p><strong>Email address: </strong> " + model.Email + "/<p>" +
                            $"<p><strong>Reason: </strong>" + model.Reason + "/<p>" +
                            $"<p><strong>Message: </strong>" + model.Message + "</p>" +
                            $"<p>Sincerely,</p><p>The Reboost Support Team</p>";

            await mailService.SendSupportEmail(model.Email, model.Fullname, subject, content, model.UploadedFiles);

            return model;
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
        public async Task<bool> HasSubmissionOnTaskOf(string userId, int questionId)
        {
            return await _unitOfWork.Users.HasSubmissionOnTaskOf(userId, questionId);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _unitOfWork.Users.GetByEmailAync(email);
        }
        public async Task<User> UpdateStripeIdAsync(User user, string stripeId)
        {
            user.StripeCustomerID = stripeId;
            return await _unitOfWork.Users.UpdateAsync(user);
        }

        public async Task<UserRanks> GetUserRate(string userId)
        {
            return await _unitOfWork.Users.GetUserRate(userId);
        }
    }
}
