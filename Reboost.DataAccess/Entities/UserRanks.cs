using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class UserRanks : BaseEntity
    {
        public string UserId { get; set; }
        [Column(TypeName = "decimal(2,1)")]
        public decimal AverageReviewRate { get; set; }
    }
}
