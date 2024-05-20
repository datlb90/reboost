using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Orders : BaseEntity
    {
        public string UserId { get; set; }
        public int PlanId { get; set; }
        public string SubscriptionType { get; set; }
        public int Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionCode { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastActivityDate { get; set; }
    }

    public enum PaymentStatus
    {
        PENDING = 0,
        COMPLETED = 1,
        PROCESSED = 2,
        ERROR = 3,
        REFUNDED = 4,
    }
}
