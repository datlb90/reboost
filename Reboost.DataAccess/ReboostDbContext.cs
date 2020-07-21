using System;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess
{
    public class ReboostDbContext : DbContext
    {
        public DbSet<Documents> Documents { get; set; }

        public ReboostDbContext(DbContextOptions<ReboostDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        { }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}
