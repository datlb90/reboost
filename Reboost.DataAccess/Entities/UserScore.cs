using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class UserScore : BaseEntity
    {
        public int UserId { get; set; }
        public string SectionId { get; set; }
        public string Subject { get; set; }
        public string Skill { get; set; }
        public double Score { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        
        public virtual Rater Rater { get; set; }

    }
}
