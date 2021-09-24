using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Raters : BaseEntity
    {
        public Raters()
        {
            this.RaterCredentials = new HashSet<RaterCredentials>();
        }

        public string UserId { get; set; }
        public string Gender { get; set; }
        public string Occupation { get; set; }
        public string FirstLanguage { get; set; }
        public System.DateTime AppliedDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Biography { get; set; }
        public System.DateTime LastActivityDate { get; set; }
        public string PaypalAccount { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RaterCredentials> RaterCredentials { get; set; }


    }
}
