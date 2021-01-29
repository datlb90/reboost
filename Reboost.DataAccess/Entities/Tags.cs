using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Tags: BaseEntity
    {
        public string Name { get; set; }
        public int DiscussionId { get; set; }
    }
}
