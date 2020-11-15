using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Rater : BaseEntity
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string Gender { get; set; }
        public string Occupation { get; set; }
        public string FirstLanguage { get; set; }
        public int idPhoto { get; set; }
        public float ScoreReport { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Biography { get; set; }
        public DateTime LastActivityDate { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<UserScore> Scores { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
