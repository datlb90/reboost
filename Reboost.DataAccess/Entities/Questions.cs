using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Questions : BaseEntity
    {
        public int TaskId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public int SubmissionCount { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public bool HasSample { get; set; }
        public string AverageScore { get; set; }
        public string Status { get; set; }
        public System.DateTime AddedDate { get; set; }
        public System.DateTime LastActivityDate { get; set; }
        public virtual Tasks Task { get; set; }

    }
}
