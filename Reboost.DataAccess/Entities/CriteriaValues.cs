using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class CriteriaValues : BaseEntity
    {
        public int CriteriaId { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public virtual RubricCriteria RubricCriteria { get; set; }
    }
}
