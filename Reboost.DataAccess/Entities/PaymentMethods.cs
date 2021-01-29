using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class PaymentMethods: BaseEntity
    {
        public string StripePaymentMethodId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardNumber { get; set; }
        public string Last4Digit { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string CVC { get; set; }
        public string Status { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
