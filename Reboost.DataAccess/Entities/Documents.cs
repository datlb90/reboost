using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reboost.DataAccess.Entities
{
    public class Documents : BaseEntity
    {
        //public string FileName { get; set; }
        //public byte[] Data { get; set; }
        //public string Status { get; set; }
        //public int PageCount { get; set; }
        //public string Text { get; set; }
        //public DateTime CreatedDate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Documents()
        {
            this.Submissions = new HashSet<Submissions>();
        }

        public string Filename { get; set; }
        public byte[] Data { get; set; }
        public string Status { get; set; }
        public int PageCount { get; set; }
        public string Text { get; set; }
        public System.DateTime CreatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Submissions> Submissions { get; set; }
    }
}
