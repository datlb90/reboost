using Microsoft.AspNetCore.Identity;
using Reboost.DataAccess.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reboost.WebApi.Identity
{
    //[Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        //[Key]
        //public override string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<UserScores> UserScores { get; set; }
    }
}
