using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class UserRanks : BaseEntity
    {
        public string UserId { get; set; }
        public int RankId { get; set; }
        public int RankingScore { get; set; }
        public decimal AverageReviewRate { get; set; }

        public virtual ReviewRankings ReviewRankings { get; set; }
    }
}
