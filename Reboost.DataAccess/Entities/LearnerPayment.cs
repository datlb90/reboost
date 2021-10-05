using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class LearnerPaymentsHistory : BaseEntity
    {
        public string UserId { get; set; }
        public int QuestionId { get; set; }
        public string PaypalInvoiceId { get; set; }
        public string Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
