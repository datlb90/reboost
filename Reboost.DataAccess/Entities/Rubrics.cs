using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Rubrics: BaseEntity
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastActivityDate { get; set; }

        [ForeignKey("RubricId")]
        public ICollection<RubricCriteria> RubricCriteria { get; set; }
    }

    public class RubricCriteria : BaseEntity
    {
        public int Id { get; set; }
        public int RubricId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MaxScore { get; set; }
        public int Weight { get; set; }
        public DateTime LastActivityDate { get; set; }

        [ForeignKey("CriteriaId")]
        public ICollection<RubricMilestones> RubricMilestones { get; set; }
    }

    public class RubricMilestones : BaseEntity
    {
        public int Id { get; set; }
        public int CriteriaId { get; set; }
        public int BandScore { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }

}
