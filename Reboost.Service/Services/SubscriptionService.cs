using System;
using Reboost.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reboost.DataAccess;
using Stripe;
using Reboost.Shared;

namespace Reboost.Service.Services
{
    public interface ISubscriptionService
    {
        Task<UserSubscription> GetUserSubscriptionModel(string userId);
        Task<Plans> GetPlan(int planId);
        Task<int> GetUserProratedAmount(string userId);
        Task<Subscriptions> GetUserSubscription(string userId);
        Task<Subscriptions> GetUserActiveSubscription(string userId);
        Task<bool> SubscribeUserToPlan(string userId, int planId);
        Task<bool> RenewUserSubscription(string userId, int planId);
        Task<bool> UpgradeUserSubscription(string userId, int planId);
    }

    public class SubscriptionService : BaseService, ISubscriptionService
    {
        public SubscriptionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<UserSubscription> GetUserSubscriptionModel(string userId)
        {
            return await _unitOfWork.Subscriptions.GetUserSubscriptionModel(userId);
        }

        public async Task<Plans> GetPlan(int planId)
        {
            return await _unitOfWork.Subscriptions.GetPlan(planId);
        }

        public async Task<int> GetUserProratedAmount(string userId)
        {
            return await _unitOfWork.Subscriptions.GetUserProratedAmount(userId);
        }

        public async Task<Subscriptions> GetUserSubscription(string userId)
        {
            return await _unitOfWork.Subscriptions.GetUserSubscription(userId);
        }


        public async Task<Subscriptions> GetUserActiveSubscription(string userId)
        {
            return await _unitOfWork.Subscriptions.GetUserActiveSubscription(userId);
        }

        public async Task<bool> UpgradeUserSubscription(string userId, int planId)
        {
            return await _unitOfWork.Subscriptions.UpgradeUserSubscription(userId, planId);
        }

        public async Task<bool> RenewUserSubscription(string userId, int planId)
        {
            return await _unitOfWork.Subscriptions.RenewUserSubscription(userId, planId);
        }

        public async Task<bool> SubscribeUserToPlan(string userId, int planId)
        {
            return await _unitOfWork.Subscriptions.SubscribeUserToPlan(userId, planId);
        }

    }
}

