using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
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
    }
}
