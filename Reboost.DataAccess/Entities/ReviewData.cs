using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class ReviewData : BaseEntity
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public int CriteriaId { get; set; }
        public decimal Score { get; set; }
        public string Comment { get; set; }
    }
}
