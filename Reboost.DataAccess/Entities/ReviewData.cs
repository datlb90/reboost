using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class ReviewData : BaseEntity
    {
        public int ReviewId { get; set; }
        [NotMapped]
        public string CriteriaName { get; set; }
        public int CriteriaId { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public Nullable<decimal> Score { get; set; }
        public string Comment { get; set; }
        public string UserFeedback { get; set; }
        public virtual Reviews Review { get; set; }
    }
}
