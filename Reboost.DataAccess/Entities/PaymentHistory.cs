using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class PaymentHistory: BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
    }
}
