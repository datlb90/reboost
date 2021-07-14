using Microsoft.AspNetCore.Identity;
using Reboost.DataAccess.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reboost.WebApi.Identity
{
    public class ApplicationUser : IdentityUser
    {
       public virtual ICollection<UserScores> UserScores { get; set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
    }
}
