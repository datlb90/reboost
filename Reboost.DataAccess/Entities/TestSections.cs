using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class TestSections : BaseEntity
    {        
        public TestSections()
        {
            //this.Tasks = new HashSet<Tasks>();
            this.UserScores = new HashSet<UserScores>();
        }


        public int TestId { get; set; }
        public string Name { get; set; }
        public string MaxScores { get; set; }

        
        //public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<UserScores> UserScores { get; set; }

        
        public virtual Tests Test { get; set; }
    }
}
