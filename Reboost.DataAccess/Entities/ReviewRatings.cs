using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class ReviewRatings : BaseEntity
    {
        public string UserId { get; set; }

        [Required]
        public int ReviewId { get; set; }

        [Required]
        public decimal Rate { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
