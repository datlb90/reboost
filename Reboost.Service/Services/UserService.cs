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
        bool IpAddressExist(string ipAddress);
        Task<bool> UsernameExist(string email, string phoneNumber);
        Task RecordUserLogin(string userId);
        Task<ContactRequestModel> SendSupportEmail(ContactRequestModel model);
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

        public bool IpAddressExist(string ipAddress)
        {
            return _unitOfWork.Users.IpAddressExist(ipAddress);
        }
        public async Task<bool> UsernameExist(string email, string phoneNumber)
        {
            return await _unitOfWork.Users.UsernameExist(email, phoneNumber);
        }
        public async Task RecordUserLogin(string userId)
        {
            await _unitOfWork.Users.RecordUserLogin(userId);
        }
        public async Task<ContactRequestModel> SendSupportEmail(ContactRequestModel model)
        {
            List<string> listPaths = new List<string>();

            if (model.UploadedFiles != null && model.UploadedFiles.Count > 0)
            {
                foreach (var item in model.UploadedFiles)
                {
                    // File size must not exceed 3MB
                    if (item.Length > 3000000)
                    {
                        return null;
                    }
                }
            }
            string subject = "Reboost - Hỗ Trợ Khách Hàng";

            var content = $"<p>Xin chào " + model.Fullname + ",</p>" +
                            $"<p>Cảm ơn bạn đã liên hệ với chúng tôi.</p>" +
                            $"<p>Chuyên viên hỗ trợ khách hàng của Reboost sẽ liên hệ với bạn trong thời gian sớm nhất liên quan tới yêu cầu sau đây:</p>" +
                            $"<p><strong>Lý do: </strong>" + model.Reason + "</p>" +
                            $"<p><strong>Yêu cầu: </strong>" + model.Message + "</p>" +
                            $"<p>Nếu cần cung cấp thêm thông tin gì, bạn có thể gửi trực tiếp qua luồng email này.</p>" +
                            $"<p>Xin chân thành cảm ơn,</p><p>Reboost Support</p>";

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
