using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Payments : BaseEntity
    {
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(19,2)")]
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
