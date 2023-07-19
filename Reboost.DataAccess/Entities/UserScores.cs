using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class UserScores : BaseEntity
    {
        public string UserId { get; set; }
        public int SectionId { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public decimal Score { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public virtual TestSections Section { get; set; }
        public virtual User User { get; set; }
    }
}
