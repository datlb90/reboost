using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IQuestionsService
    {
        Task<Questions> GetByTitle(string title);
        Task<IEnumerable<Questions>> GetAllAsync();
        Task<QuestionModel> GetByIdAsync(int id);
        Task<Questions> CreateAsync(Questions question);
        Task<Questions> UpdateAsync(Questions question);
        Task<Questions> DeleteAsync(int id);
        Task<List<Tasks>> GetTasksAsync();
        Task<Dictionary<string, int>> GetCountQuestionByTasksAsync();
        Task<IEnumerable<QuestionModel>> GetAllExAsync();
        Task<IEnumerable<QuestionModel>> GetAllByUserAsync(string userId);
        Task<List<SummaryPerUser>> GetSummaryByUserId(string UserId);
        Task<List<string>> GetTestForCurrentUsers(string UserId);
        Task<List<int>> GetQuestionCompletedIdByUser(string UserId);
        Task<List<SampleForQuestion>> GetSamplesForQuestion(int questionId);
        Task<List<SubmissionsModel>> GetAllSubmissionByUserId(string userId);
        Task<QuestionDataModel> GetAllDataForAddOrEditQuestion();
        Task<QuestionRequestModel> CreateQuestionAsync(Questions q, QuestionRequestModel model);
        Task<QuestionRequestModel> UpdateQuestionAsync(Questions q, QuestionRequestModel model);
        Task<Questions> PublishQuestionAsync(int id);
        Task<Questions> ApproveQuestionAsync(int id);
        Task<List<SubmissionsForQuestionModel>> GetSubmissionsForQuestion(string userId, int questionId);
    }
    public class QuestionsService : BaseService, IQuestionsService
    {
        public QuestionsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<Questions> GetByTitle(string title)
        {
            return await _unitOfWork.Questions.GetByTitle(title);
        }

        public async Task<Questions> CreateAsync(Questions question)
        {
            return await _unitOfWork.Questions.Create(question);
        }

        public async Task<Questions> DeleteAsync(int id)
        {
            await _unitOfWork.Samples.DeleteByQuestionIdAsync(id);
            await _unitOfWork.QuestionParts.DeleteByQuestionIdAsync(id);
            return await _unitOfWork.Questions.Delete(id);
        }

        public async Task<IEnumerable<Questions>> GetAllAsync()
        {
            return await _unitOfWork.Questions.GetAllAsync(null, "Task");
        }

        public async Task<QuestionModel> GetByIdAsync(int id)
        {
            return await _unitOfWork.Questions.GetByIdAsync(id);
        }

        public async Task<Questions> UpdateAsync(Questions question)
        {
            return await _unitOfWork.Questions.Update(question);
        }

        public async Task<List<Tasks>> GetTasksAsync()
        {
            return await _unitOfWork.Questions.GetTaskAsync();
        }

        public async Task<Dictionary<string, int>> GetCountQuestionByTasksAsync()
        {
            return await _unitOfWork.Questions.CountQuestByTaskAsync();
        }

        public async Task<IEnumerable<QuestionModel>> GetAllExAsync()
        {
            return await _unitOfWork.Questions.GetAllExAsync();
        }
        public async Task<IEnumerable<QuestionModel>> GetAllByUserAsync(string userId)
        {
            return await _unitOfWork.Questions.GetAllByUserAsync(userId);
        }

        public async Task<List<SummaryPerUser>> GetSummaryByUserId(string userId)
        {
            return await _unitOfWork.Questions.GetSummaryByUserId(userId);
        }

        public async Task<List<string>> GetTestForCurrentUsers(string UserId)
        {
            return await _unitOfWork.Questions.GetTestForCurrentUsers(UserId);
        }

        public async Task<List<int>> GetQuestionCompletedIdByUser(string UserId)
        {
            return await _unitOfWork.Questions.GetQuestionCompletedIdByUser(UserId);
        }

        public async Task<List<SampleForQuestion>> GetSamplesForQuestion(int questionId)
        {
            return await _unitOfWork.Questions.GetSamplesForQuestion(questionId);
        }
        public async Task<List<SubmissionsModel>> GetAllSubmissionByUserId(string userId)
        {
            return await _unitOfWork.Questions.GetAllSubmissionByUserIdAsync(userId);
        }
        public async Task<List<SubmissionsForQuestionModel>> GetSubmissionsForQuestion(string userId, int questionId)
        {
            return await _unitOfWork.Questions.GetSubmissionsForQuestion(userId, questionId);
        }
        public async Task<QuestionDataModel> GetAllDataForAddOrEditQuestion()
        {
            return await _unitOfWork.Questions.GetAllDataForAddOrEditQuestion();
        }
        public async Task<QuestionRequestModel> CreateQuestionAsync(Questions q, QuestionRequestModel model)
        {
            model.AddedDate = DateTime.UtcNow;
            model.LastActivityDate = DateTime.UtcNow;
            model.AverageScore = "0.0";
            model.HasSample = false;
            model.SubmissionCount = 0;
            model.ViewCount = 0;
            model.LikeCount = 0;
            model.DisLikeCount = 0;
            return await _unitOfWork.Questions.CreateQuestionAsync(q, model);
        }

        public async Task<QuestionRequestModel> UpdateQuestionAsync(Questions q, QuestionRequestModel model)
        {
            return await _unitOfWork.Questions.UpdateQuestionAsync(q, model);
        }

        public async Task<Questions> PublishQuestionAsync(int id)
        {
            return await _unitOfWork.Questions.PublishQuestionAsync(id);
        }

        public async Task<Questions> ApproveQuestionAsync(int id)
        {
            return await _unitOfWork.Questions.ApproveQuestionAsync(id);
        }
    }
}
