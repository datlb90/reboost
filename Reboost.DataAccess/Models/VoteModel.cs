using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class VoteModel
    {
        public int DiscussionId { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }
    }
}
