using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Submissions : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Submissions()
        {
            this.ReviewRequests = new HashSet<ReviewRequests>();
        }

        public string UserId { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey("Document")]
        public int DocId { get; set; }
        public string Type { get; set; }
        public System.DateTime SubmittedDate { get; set; }
        public int TimeSpentInSeconds { get; set; }

        public virtual Documents Document { get; set; }
        public virtual Questions Question { get; set; }
        public virtual ICollection<ReviewRequests> ReviewRequests { get; set; }
    }
}
