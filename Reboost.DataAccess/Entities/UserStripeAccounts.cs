using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class UserStripeAccounts: BaseEntity
    {
        public string UserId { get; set; }
        public string StripeAccountId { get; set; }
    }
}
