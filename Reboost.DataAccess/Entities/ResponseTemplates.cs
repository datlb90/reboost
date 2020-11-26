using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class ResponseTemplates : BaseEntity
    {
        public int TaskId { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public int ViewCount { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime AddedDate { get; set; }
        public System.DateTime LastActivityDate { get; set; }

        public virtual QuestionTypes Type { get; set; }
        //public virtual Tasks Task { get; set; }
    }
}
