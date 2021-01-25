using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Discussion : BaseEntity
    {
        public Discussion()
        {
            this.Tags = new HashSet<Tags>();
        }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        public string? Title { get; set; }
        public int Views { get; set; }
        public int Votes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Content { get; set; }
        public int? ParentId { get; set; }
        public int? Level { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Tags> Tags { get; set; }
        public virtual ICollection<DiscussionVote> DiscussionVote { get; set; }
    }
}
