using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class ReviewRankings : BaseEntity
    {
        public string Name { get; set; }
        public int StartScore { get; set; }
        public int EndScore { get; set; }
        public int AverageCompletionTime { get; set; }
        public int PriorityLevel { get; set; }
        [Column(TypeName = "decimal(2,1)")]
        public decimal MinimumRate { get; set; }
    }
}
