using Microsoft.AspNetCore.Identity;
using Reboost.DataAccess.Entities;
using System;
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
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int FreeToken { get; set; }
        public int PremiumToken { get; set; }
    }
}
