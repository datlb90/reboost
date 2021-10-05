using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class LearnerSubscriptions: BaseEntity
    {
        public string UserId { get; set; }
        public string MonthSubs { get; set; }
        public string YearSubs { get; set; }
        public virtual User User { get; set; }
    }
}
