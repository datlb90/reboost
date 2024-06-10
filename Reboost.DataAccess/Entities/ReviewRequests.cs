using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class ReviewRequests : BaseEntity
    {
        public string UserId { get; set; }
        public int SubmissionId { get; set; }
        public string FeedbackType { get; set; } // AI, Free, Pro
        public System.DateTime RequestedDateTime { get; set; }
        public Nullable<System.DateTime> CompletedDateTime { get; set; }
        public string Status { get; set; }
        public string FeedbackLanguage { get; set; } // en, vn
        public string SpecialRequest { get; set; }
        public string ReviewType { get; set; } // Chi tiết, Chuyên sâu
        public virtual Submissions Submission { get; set; }
    }
}
