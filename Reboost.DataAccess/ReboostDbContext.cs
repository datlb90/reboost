using System;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess
{
    public class ReboostDbContext : DbContext
    {
        public DbSet<Documents> Documents { get; set; }
        public DbSet<RequestQueue> RequestQueues { get; set; }
        public DbSet<Photo> Photos{ get; set; }
        public DbSet<Rater> Raters { get; set; }
        public DbSet<UserScore> Scores { get; set; }
        public DbSet<LookUp> LookUps { get; set; }


        public ReboostDbContext(DbContextOptions<ReboostDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Rater>()
                    .HasMany(c => c.Photos)
                    .WithOne(e => e.Rater)
                    .HasForeignKey(e => e.UserId);
            builder.Entity<Rater>()
                    .HasMany(c => c.Scores)
                    .WithOne(e => e.Rater)
                    .HasForeignKey(e => e.UserId);
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}
