using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class UserRanks : BaseEntity
    {
        public string UserId { get; set; }
        public decimal AverageReviewRate { get; set; }
    }
}
