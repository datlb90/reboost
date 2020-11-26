using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class ReviewRankings : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReviewRankings()
        {
            this.UserRanks = new HashSet<UserRanks>();
        }

        public string Name { get; set; }
        public int StartScore { get; set; }
        public int EndScore { get; set; }
        public int AverageCompletionTime { get; set; }
        public int PriorityLevel { get; set; }
        public decimal MinimumRate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRanks> UserRanks { get; set; }
    }
}
