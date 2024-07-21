using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reboost.DataAccess.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Reboost.DataAccess.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string IpAddress { get; set; }
        public int FreeToken { get; set; }
        public int PremiumToken { get; set; }
        public string? StripeCustomerID { get; set; }
        public virtual ICollection<UserScores> UserScores { get; set; }
    }

    public class UserConfiguration
    {
        public UserConfiguration(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("AspNetUsers", "dbo");
            entity.HasKey(e => e.Id);
        }
    }
}
