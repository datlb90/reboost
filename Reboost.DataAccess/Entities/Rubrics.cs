using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Rubrics : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rubrics()
        {
            this.RubricCriteria = new HashSet<RubricCriteria>();
        }

        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime LastActivityDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RubricCriteria> RubricCriteria { get; set; }
        //public virtual Tasks Tasks { get; set; }
    }
}
