using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class QuestionTypes : BaseEntity
    {
        public int TemplateId { get; set; }
        public string Name { get; set; }
    }
}
