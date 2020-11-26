using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class QuestionParts: BaseEntity
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public int order { get; set; }
    }
}
