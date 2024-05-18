using System;
namespace Reboost.DataAccess.Entities
{
    public class Subscriptions : BaseEntity
    {
        public string UserId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

