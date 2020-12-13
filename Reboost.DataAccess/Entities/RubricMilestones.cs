using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class RubricMilestones : BaseEntity
    {
        public int CriteriaId { get; set; }
        public int BandScore { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public virtual RubricCriteria Criteria { get; set; }
    }
}
