using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class RequestAssignment: BaseEntity
    {
        public int RequestId { get; set; }
        public int RaterId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
