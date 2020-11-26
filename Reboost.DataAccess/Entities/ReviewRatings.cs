using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class ReviewRatings : BaseEntity
    {
        public string UserId { get; set; }
        public int ReviewId { get; set; }
        public decimal Rate { get; set; }
        public string Comment { get; set; }
    }
}
