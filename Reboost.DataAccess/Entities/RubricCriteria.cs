using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class RubricCriteria : BaseEntity
    {
        public RubricCriteria()
        {
            this.CriteriaValues = new HashSet<CriteriaValues>();
            this.RubricMilestone = new HashSet<RubricMilestones>();
        }

        public int RubricId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public decimal MaxScore { get; set; }
        public Nullable<int> Weight { get; set; }
        public System.DateTime LastActivityDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CriteriaValues> CriteriaValues { get; set; }
        public virtual ICollection<RubricMilestones> RubricMilestone { get; set; }
        public virtual Rubrics Rubric { get; set; }
    }
}
