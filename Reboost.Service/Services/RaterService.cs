using Microsoft.AspNetCore.Http;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IRaterService
    {
        Task<IEnumerable<Raters>> GetAllAsync();
        Task<Raters> GetByIdAsync(int id);
        Task<Raters> GetByUserIdAsync(String id);
        Task<Raters> CreateAsync(Raters rater, List<IFormFile> uploadFiles);
        Task<Raters> UpdateAsync(Raters rater, List<IFormFile> uploadFiles);
        Task<Raters> UpdateStatusAsync(int id, string status);
        Task<Raters> UpdateCredentialAsync(Raters rater, List<IFormFile> uploadFiles);
        Task<Raters> DeleteAsync(int id);
        Task<List<string>> GetApplyTo(int raterId);
        Task<decimal> GetRaterRatingAsync(string UserID);
        Task<Raters> GetRaterPaypalAccountAsync(string userId);
        Task<Raters> UpdateRaterPaypalAccountAsync(string userId, string paypalAccount);
        Task<List<Raters>> GetAllMasterRater();
    }

    public class RaterService : BaseService, IRaterService
    {
        public RaterService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<Raters> CreateAsync(Raters rater, List<IFormFile> uploadFiles)
        {
            foreach (var item in rater.RaterCredentials)
            {
                var file = uploadFiles.FirstOrDefault(f => f.FileName == item.FileName);
                if (file != null)
                    item.Data = GetBytesFromFile(file);
            }

            var existed = await _unitOfWork.Raters.GetByUserIdAsync(rater.UserId);
            if (existed != null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Rater existed");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(rater.UserId);
            if (user == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "User not existed");
            }

            user.FirstName = rater.User.FirstName;
            user.LastName = rater.User.LastName;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.Users.UpdateScoreAsync(rater.UserId, rater.User.UserScores.ToList());

            rater.User = null;

            // To do: gửi email cho admin

            return await _unitOfWork.Raters.Create(rater);
        }

        public async Task<Raters> DeleteAsync(int id)
        {
            return await _unitOfWork.Raters.Delete(id);
        }

        public async Task<IEnumerable<Raters>> GetAllAsync()
        {
            return (await _unitOfWork.Raters.GetAllAsync(null, "User")).OrderByDescending(r => r.AppliedDate);
        }

        public async Task<Raters> GetByIdAsync(int id)
        {
            return await _unitOfWork.Raters.GetDetailByIdAsync(id);
        }

        public async Task<Raters> GetByUserIdAsync(String id)
        {
            return await _unitOfWork.Raters.GetByUserIdAsync(id);
        }
        public async Task<List<string>> GetApplyTo(int raterId)
        {
            return await _unitOfWork.Raters.GetApplyTo(raterId);
        }

        public async Task<Raters> UpdateAsync(Raters rater, List<IFormFile> uploadFiles)
        {
            // Update user's credentials
            if(uploadFiles != null)
            {
                foreach (var item in rater.RaterCredentials)
                {
                    item.RaterId = rater.Id;
                    var file = uploadFiles.FirstOrDefault(f => f.FileName == item.FileName);
                    if (file != null)
                        item.Data = GetBytesFromFile(file);
                }
                await _unitOfWork.RaterCredential.UpdateManyByRaterAync(rater.Id, rater.RaterCredentials.ToList());
            }

            // Update user's firstname and lastname
            var user = await _unitOfWork.Users.GetByIdAsync(rater.UserId);
            if (user == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "User not existed");
            }

            if(!user.FirstName.Trim().Equals(rater.User.FirstName) || !user.LastName.Trim().Equals(rater.User.LastName))
            {
                user.FirstName = rater.User.FirstName;
                user.LastName = rater.User.LastName;

                await _unitOfWork.Users.UpdateAsync(user);
            }
            
            if(rater.Note == "null")
            {
                rater.Note = null;
            }

            // Update user's score
            await _unitOfWork.Users.UpdateScoreAsync(rater.UserId, rater.User.UserScores.ToList());

            rater.User = null;
            rater.RaterCredentials = null;

            // Update rater's information
            var rs = await _unitOfWork.Raters.Update(rater);
            return rs;
        }

        public async Task<Raters> UpdateCredentialAsync(Raters rater, List<IFormFile> uploadFiles)
        {
            foreach (var item in rater.RaterCredentials)
            {
                item.RaterId = rater.Id;
                var file = uploadFiles.FirstOrDefault(f => f.FileName == item.FileName);
                if (file != null)
                    item.Data = GetBytesFromFile(file);
            }

            Raters _rater = await _unitOfWork.Raters.GetByIdAsync(rater.Id);

            _rater.RaterCredentials = rater.RaterCredentials;
            _rater.Status = rater.Status;
            _rater.Biography = rater.Biography;

            await _unitOfWork.RaterCredential.UpdateManyByRaterAync(rater.Id, rater.RaterCredentials.ToList());

            rater.User = null;
            rater.RaterCredentials = null;
            var rs = await _unitOfWork.Raters.Update(_rater);
            return rs;
        }

        public async Task<Raters> UpdateStatusAsync(int id, string status)
        {
            var rater = await _unitOfWork.Raters.GetByIdAsync(id);
            rater.Status = status;
            return await _unitOfWork.Raters.Update(rater);
        }

        private byte[] GetBytesFromFile(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public async Task<decimal> GetRaterRatingAsync(string UserID)
        {
            return await _unitOfWork.Raters.GetRaterRatingAsync(UserID);
        }

        public async Task<Raters> GetRaterPaypalAccountAsync(string userId)
        {
            return await _unitOfWork.Raters.GetRaterPaypalAccountAsync(userId);
        }

        public async Task<Raters> UpdateRaterPaypalAccountAsync(string userId, string paypalAccount)
        {
            var rs = await _unitOfWork.Raters.UpdateRaterPaypalAccountAsync(userId, paypalAccount);

            if (rs == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Rater not exists!");
            }

            return rs;
        }

        public async Task<List<Raters>> GetAllMasterRater()
        {
            return await _unitOfWork.Raters.GetAllMasterRater();
        }
    }
}
