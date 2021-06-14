using System;
using System.Collections.Generic;
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
        public decimal MinimumRate { get; set; }
    }
}
