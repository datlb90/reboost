using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class DiscussionVote : BaseEntity
    {
        public int DiscussionId { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }
        public virtual User User { get; set; }

    }
}
