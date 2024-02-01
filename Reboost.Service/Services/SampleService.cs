using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.events.IndexEvents;

namespace Reboost.Service.Services
{
    public interface ISampleService
    {
        Task<IEnumerable<Samples>> GetAllAsync();
        Task<Samples> GetByIdAsync(int id);
        Task<Samples> CreateAsync(Samples entity);
        Task<Samples> UpdateAsync(Samples entity);
        Task<Samples> DeleteAsync(int id);
        Task<Samples> ApproveSampleByIdAsync(int id);

    }
    public class SampleService : BaseService, ISampleService
    {
        public SampleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<Samples> CreateAsync(Samples entity)
        {
            entity.LastActivityDate = DateTime.UtcNow;
            entity = await _unitOfWork.Samples.Create(entity);

            Questions question = await _unitOfWork.Questions.GetQuestionByIdAsync(entity.QuestionId);
            question.HasSample = true;
            await _unitOfWork.Questions.Update(question);

            return entity;
        }

        public async Task<Samples> DeleteAsync(int id)
        {
            Samples sample = await _unitOfWork.Samples.Delete(id);
            var samples = await _unitOfWork.Samples.GetSamplesByQuestionId(sample.QuestionId);
            if(samples == null || samples.Count == 0)
            {
                Questions question = await _unitOfWork.Questions.GetQuestionByIdAsync(sample.QuestionId);
                question.HasSample = false;
                await _unitOfWork.Questions.Update(question);
            }
            return sample;
        }

        public async Task<IEnumerable<Samples>> GetAllAsync()
        {
            return await _unitOfWork.Samples.GetAllAsync();
        }

        public async Task<Samples> GetByIdAsync(int id)
        {
            return await _unitOfWork.Samples.GetByIdAsync(id);
        }

        public async Task<Samples> UpdateAsync(Samples entity)
        {
            return await _unitOfWork.Samples.Update(entity);
        }

        public async Task<Samples> ApproveSampleByIdAsync(int id)
        {
            return await _unitOfWork.Samples.ApproveSampleByIdAsync(id);
        }
    }

}
