using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;

namespace Reboost.Service.Services
{
    public interface IRequestQueueService
    {
        Task<RequestQueue> GetTopPriorityRequest();
    }

    public class RequestQueueService : IRequestQueueService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequestQueueService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<RequestQueue> GetTopPriorityRequest()
        {
            return await _unitOfWork.RequestQueues.GetTopPriorityRequest();
        }
    }
}
