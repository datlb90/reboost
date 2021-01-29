using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Payments : BaseEntity
    {
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
