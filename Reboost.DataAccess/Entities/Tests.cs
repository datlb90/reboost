using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Tests : BaseEntity
    {
        public Tests()
        {
            this.TestSections = new HashSet<TestSections>();
        }

        public string Name { get; set; }

        public virtual ICollection<TestSections> TestSections { get; set; }
        //public virtual ICollection<RaterCredentials> RaterCredentials { get; set; }

    }
}
