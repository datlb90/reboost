using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class RaterTraining : BaseEntity
    {
        public int RaterId { get; set; }
        public string Test { get; set; }
        public string Status { get; set; }
        public int ReviewId { get; set; }
        public string Note { get; set; }
        public virtual Raters Rater { get; set; }
        public virtual Reviews Review { get; set; }
    }
}
