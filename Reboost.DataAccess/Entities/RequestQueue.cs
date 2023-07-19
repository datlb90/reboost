using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reboost.DataAccess.Entities
{
    public class RequestQueue : BaseEntity
    {
        public int RequestId { get; set; }

        public int Priority { get; set; }

        public DateTime RequestedDatetime { get; set; }

        public int Status { get; set; }

        [Column(TypeName = "decimal(2,1)")]
        public decimal MinimumRate { get; set; }
    }
}
