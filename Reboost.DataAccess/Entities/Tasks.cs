using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Tasks : BaseEntity
    {
        public int SectionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Focus { get; set; }
        public string Time { get; set; }
        public string Direction { get; set; }
    }
}
