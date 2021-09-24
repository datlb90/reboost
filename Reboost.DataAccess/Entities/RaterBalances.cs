using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class RaterBalances : BaseEntity
    {
        public int RaterId { get; set; }
        public int ReviewId { get; set; }
        public DateTime CreatedDate { get; set; }        
        public  DateTime?  PayoutDate { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public virtual Raters Rater { get; set; }
    }
}
