using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using Reboost.Shared;

namespace Reboost.DataAccess.Repositories
{
    public interface ISubscriptionRepository : IRepository<Subscriptions>
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

    public class SubscriptionRepository : BaseRepository<Subscriptions>, ISubscriptionRepository
    {
        ReboostDbContext _context;

        private ReboostDbContext ReboostDbContext => context as ReboostDbContext;

        public SubscriptionRepository(ReboostDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserSubscription> GetUserSubscriptionModel(string userId)
        {
            var subscription = await GetUserSubscription(userId);

            if(subscription != null)
            {
                Plans plan = await ReboostDbContext.Plans.FindAsync(subscription.PlanId);
                UserSubscription userSubscription = new UserSubscription
                {
                    userId = userId,
                    planId = plan.Id,
                    duration = plan.Duration,
                    proratedAmount = await GetUserProratedAmount(userId)
                };

                return userSubscription;
            }

            return null;
        }

        public async Task<Plans> GetPlan(int planId)
        {
            return await ReboostDbContext.Plans.FirstOrDefaultAsync(p => p.Id == planId);
        }

        public async Task<int> GetUserProratedAmount(string userId)
        {
            // Check if the user already has an active subscription
            var activeSubscription = await ReboostDbContext.Subscriptions.Where(s => s.UserId == userId &&
                                            s.Status == "Active" && s.EndDate > DateTime.Now).FirstOrDefaultAsync();

            if (activeSubscription != null)
            {
                // Get the current plan
                Plans plan = await ReboostDbContext.Plans.FindAsync(activeSubscription.PlanId);

                var daysRemaining = (activeSubscription.EndDate - DateTime.Now).TotalDays;
                var totalDays = (activeSubscription.EndDate - activeSubscription.StartDate).TotalDays;
                var proRatedAmount = (daysRemaining / totalDays) * plan.Price;
                return (int)Math.Round(proRatedAmount);

            }

            return 0; // No active subscription
        }

        public async Task<Subscriptions> GetUserSubscription(string userId)
        {
            var activeSubscription = await ReboostDbContext.Subscriptions.Where(s => s.UserId == userId && s.Status == "Active").FirstOrDefaultAsync();
            if(activeSubscription != null)
            {
                // Check if the subscription is exprired
                if(activeSubscription.EndDate < DateTime.Now)
                {
                    activeSubscription.Status = "Expired";
                    activeSubscription.UpdatedAt = DateTime.Now;
                    ReboostDbContext.Subscriptions.Update(activeSubscription);
                    context.SaveChanges();

                    return null; // User has no active subscription
                }
                else
                {
                    return activeSubscription;
                }
            }
            return null; // user has no active subscription
        }


        public async Task<Subscriptions> GetUserActiveSubscription(string userId)
        {
            return await ReboostDbContext.Subscriptions.Where(s => s.UserId == userId && s.Status == "Active").FirstOrDefaultAsync();
        }

        public async Task<bool> UpgradeUserSubscription(string userId, int planId)
        {
            // Check if the user already has an active subscription
            var activeSubscription = await ReboostDbContext.Subscriptions.Where(s => s.UserId == userId && s.Status == "Active").FirstOrDefaultAsync();

            if (activeSubscription != null)
            {
                // In-activate the current plan
                activeSubscription.Status = "Upgraded";
                activeSubscription.UpdatedAt = DateTime.Now;

                ReboostDbContext.Subscriptions.Update(activeSubscription);
                await context.SaveChangesAsync();
            }

            // Get the new plan
            Plans newPlan = await ReboostDbContext.Plans.FindAsync(planId);

            var newStartDate = DateTime.Now;
            var newEndDate = newStartDate.AddMonths(newPlan.Duration);

            // Create a new subscription for the new plan
            var newSubscription = new Subscriptions
            {
                UserId = userId,
                PlanId = newPlan.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(newPlan.Duration),
                Status = "Active",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Add the new subscription
            ReboostDbContext.Subscriptions.Add(newSubscription);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RenewUserSubscription(string userId, int planId)
        {
            // Check if the user already has an active subscription
            var activeSubscription = await ReboostDbContext.Subscriptions.Where(s => s.UserId == userId && s.Status == "Active").FirstOrDefaultAsync();

            if (activeSubscription != null)
            {
                // Update the current subscription
                Plans newPlan = await ReboostDbContext.Plans.FindAsync(planId);
                activeSubscription.PlanId = planId;
                activeSubscription.EndDate = activeSubscription.EndDate.AddMonths(newPlan.Duration);
                activeSubscription.UpdatedAt = DateTime.Now;

                ReboostDbContext.Subscriptions.Update(activeSubscription);
                await context.SaveChangesAsync();

                return true; 
            }
            else
            {
                // If there is no active plan, create a new subscription
                var newSubscription = new Subscriptions
                {
                    UserId = userId,
                    PlanId = planId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(ReboostDbContext.Plans.Find(planId).Duration),
                    Status = "Active",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                ReboostDbContext.Subscriptions.Add(newSubscription);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> SubscribeUserToPlan(string userId, int planId)
        {
            // Check if the user already has an active subscription
            var activeSubscription = await ReboostDbContext.Subscriptions.Where(s => s.UserId == userId && s.Status == "Active").FirstOrDefaultAsync();

            if (activeSubscription != null)
            {
                // This should not get called
                // Expire the current plan
                activeSubscription.Status = "Expired";
                activeSubscription.UpdatedAt = DateTime.Now;
                ReboostDbContext.Subscriptions.Update(activeSubscription);
                await context.SaveChangesAsync();
            }

            // Create a new subscription
            var newSubscription = new Subscriptions
            {
                UserId = userId,
                PlanId = planId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(ReboostDbContext.Plans.Find(planId).Duration),
                Status = "Active",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ReboostDbContext.Subscriptions.Add(newSubscription);
            await context.SaveChangesAsync();

            return true;
        }
    }
}

