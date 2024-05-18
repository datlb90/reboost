using System;
using Reboost.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reboost.DataAccess;
using Stripe;

namespace Reboost.Service.Services
{
    public interface ISubscriptionService
    {
    }

    public class SubscriptionService : BaseService, ISubscriptionService
    {
        public SubscriptionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}

