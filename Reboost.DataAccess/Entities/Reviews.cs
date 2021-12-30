using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Reviews : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reviews()
        {
            this.ReviewData = new HashSet<ReviewData>();
        }

        public int RequestId { get; set; }
        public string ReviewerId { get; set; }
        public string RevieweeId { get; set; }
        public int SubmissionId { get; set; }
        public Nullable<decimal> FinalScore { get; set; }
        public string Status { get; set; }
        public int TimeSpentInSeconds { get; set; }
        public System.DateTime LastActivityDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewData> ReviewData { get; set; }
    }
}
