using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface ISubmissionService
    {
        Task<Submissions> CreateAsync(Submissions rater);
    }
    public class SubmissionService : BaseService, ISubmissionService
    {
        public SubmissionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<Submissions> CreateAsync(Submissions sub)
        {
            return await _unitOfWork.Submission.Create(sub);
        }
    }
}
