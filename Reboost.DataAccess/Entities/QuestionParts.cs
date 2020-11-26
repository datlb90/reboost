using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class QuestionParts : BaseEntity
    {
        public int QuestionId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int order { get; set; }
        public virtual Questions Question { get; set; }
    }
}
