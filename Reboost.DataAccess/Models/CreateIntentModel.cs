using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class CreateIntentModel
    {
        public int Amount { get; set; }
    }
    public class CreateSubscriptionModel
    {
        public string priceId { get; set; }
        public string methodId { get; set; }
    }
    public class AttachMethodModel
    {
        public string methodId { get; set; }
    }
    public class BankAccountCreateModel
    {
        public string UserId { get; set; }
    }
}
