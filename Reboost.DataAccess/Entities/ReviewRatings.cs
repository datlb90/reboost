using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class ReviewRatings : BaseEntity
    {
        public string UserId { get; set; }

        [Required]
        public int ReviewId { get; set; }

        [Required]
        [Column(TypeName = "decimal(2,1)")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }
    }
}
