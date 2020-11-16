using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class LookUp : BaseEntity
    {
        public string LookupType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
