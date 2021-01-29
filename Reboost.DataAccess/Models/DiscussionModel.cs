using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class DiscussionModel
    {
        public int Id { get; set; }
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
        public List<Discussion>? Discussions { get; set; }
        public List<DiscussionVote>? DiscussionVote { get; set; }
    }
}
