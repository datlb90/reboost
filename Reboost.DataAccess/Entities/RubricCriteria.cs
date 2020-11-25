using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class RubricCriteria : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RubricCriteria()
        {
            this.CriteriaValues = new HashSet<CriteriaValues>();
        }

        public int RubricId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MaxScore { get; set; }
        public Nullable<int> Weight { get; set; }
        public System.DateTime LastActivityDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CriteriaValues> CriteriaValues { get; set; }
        public virtual Rubrics Rubrics { get; set; }
    }
}
