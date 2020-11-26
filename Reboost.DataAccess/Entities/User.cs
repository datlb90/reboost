using Microsoft.AspNetCore.Identity;
using Reboost.DataAccess.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reboost.DataAccess.Entities
{
    [Table("AspNetUsers")]
    public class User
    {
        [Key]
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public virtual ICollection<UserScores> UserScores { get; set; }
    }
}
