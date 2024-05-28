using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess
{
    public class ReboostDbContext : DbContext
    {
        public virtual DbSet<CriteriaValues> CriteriaValues { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<InTextComments> InTextComments { get; set; }
        public virtual DbSet<QuestionParts> QuestionParts { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<QuestionTypes> QuestionTypes { get; set; }
        public virtual DbSet<RaterCredentials> RaterCredentials { get; set; }
        public virtual DbSet<Raters> Raters { get; set; }
        public virtual DbSet<RaterTraining> RaterTraining { get; set; }
        public virtual DbSet<RequestQueue> RequestQueue { get; set; }
        public virtual DbSet<ResponseTemplates> ResponseTemplates { get; set; }
        public virtual DbSet<ReviewData> ReviewData { get; set; }
        public virtual DbSet<ReviewRankings> ReviewRankings { get; set; }
        public virtual DbSet<ReviewRatings> ReviewRatings { get; set; }
        public virtual DbSet<ReviewRequests> ReviewRequests { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<RubricCriteria> RubricCriteria { get; set; }
        public virtual DbSet<Rubrics> Rubrics { get; set; }
        public virtual DbSet<Samples> Samples { get; set; }
        public virtual DbSet<Submissions> Submissions { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Tests> Tests { get; set; }
        public virtual DbSet<TestSections> TestSections { get; set; }
        public virtual DbSet<UserRanks> UserRanks { get; set; }
        public virtual DbSet<UserScores> UserScores { get; set; }
        public DbSet<LookUp> LookUps { get; set; }
        public DbSet<User> Users { get; set; }
        public virtual DbSet<RubricMilestones> RubricMilestones { get; set; }
        public virtual DbSet<Discussion> Discussion { get; set; }
        public virtual DbSet<DiscussionVote> DiscussionVote { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Annotations> Annotations { get; set; }
        public virtual DbSet<UserStripeAccounts> UserStripeAccounts { get; set; }
        public virtual DbSet<RequestAssignment> RequestAssignments { get; set; }
        public virtual DbSet<Disputes> Disputes { get; set; }
        public virtual DbSet<RaterBalances> RaterBalances { get; set; }
        public virtual DbSet<LearnerPaymentsHistory> LearnerPaymentsHistory { get; set; }
        public virtual DbSet<LearnerSubscriptions> LearnerSubscriptions { get; set; }
        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<ArticleLabels> ArticleLabels { get; set; }
        public virtual DbSet<ArticleRelations> ArticleRelations { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<UserLogins> UserLogins { get; set; }
        public virtual DbSet<Plans> Plans { get; set; }
        public virtual DbSet<Subscriptions> Subscriptions { get; set; }
        public virtual DbSet<ReviewScores> ReviewScores { get; set; }

        public ReboostDbContext(DbContextOptions<ReboostDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new UserConfiguration(builder.Entity<User>());
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
