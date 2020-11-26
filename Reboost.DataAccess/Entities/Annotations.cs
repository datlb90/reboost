using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Annotations : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Annotations()
        {
            this.InTextComments = new HashSet<InTextComments>();
        }

        public int DocId { get; set; }
        public int ReviewId { get; set; }
        public string Type { get; set; }
        public int PageNumber { get; set; }
        public int TopPosition { get; set; }
        public string Color { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InTextComments> InTextComments { get; set; }
    }
}
