using System;
namespace Reboost.DataAccess.Entities
{
    public class RequestQueue : BaseEntity
    {
        public int RequestId { get; set; }

        public int Priority { get; set; }

        public DateTime RequestedDatetime { get; set; }

        public int Status { get; set; }

        public decimal MinimumRate { get; set; }
    }
}
