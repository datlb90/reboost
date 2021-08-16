using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Disputes : BaseEntity
    {
        public string Name { get; set; }
        public int QuestionId { get; set; }
        public int ReviewId { get; set; }
        public string ReviewUrl { get; set; }
        public string Reasons { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public virtual Reviews Review { get; set; }
    }
}
