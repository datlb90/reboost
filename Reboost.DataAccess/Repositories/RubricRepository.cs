using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;

namespace Reboost.DataAccess.Repositories
{
    public interface IRubricRepository : IRepository<Rubrics>
    {
        Task<List<FeedbackRubric>> GetFeedbackRubric(int questionId, string task);
        Task<Rubrics> GetByIdAsync(int id);
        Task<Rubrics> UpdateWithCriteraAsync(Rubrics rubrics);
        Task<List<RubricsModel>> GetByQuestionId(int id);
    }
    public class RubricRepository : BaseRepository<Rubrics>, IRubricRepository
    {
        private ReboostDbContext ReboostDbContext => context as ReboostDbContext;
        public RubricRepository(ReboostDbContext context) : base(context)
        { }

        public async Task<List<FeedbackRubric>> GetFeedbackRubric(int questionId, string task)
        {
           return await (from question in ReboostDbContext.Questions
                        join rubric in ReboostDbContext.Rubrics
                            on question.TaskId equals rubric.TaskId into qr
                            from questionRubric in qr.DefaultIfEmpty()
                        join criteria in ReboostDbContext.RubricCriteria
                            on questionRubric.Id equals criteria.RubricId into rc
                            from rubricCriteria in rc.DefaultIfEmpty()
                        where question.Id == questionId && rubricCriteria.Name != "Overall Score & Feedback" && rubricCriteria.Name != "Critical Errors" &&
                        (task == "Academic Writing Task 1" ? rubricCriteria.Name != "Arguments Assessment" : true )
                        select new FeedbackRubric
                        {
                            criteriaId = rubricCriteria.Id,
                            name = rubricCriteria.Name,
                            order = rubricCriteria.OrderId
                        }).OrderBy(r => r.order).ToListAsync();
        }

        public async Task<Rubrics> GetByIdAsync(int id)
        {
            return await ReboostDbContext.Rubrics
                .Include(r => r.RubricCriteria)
                .Include(r => r.RubricCriteria).ThenInclude(u => u.RubricMilestone)
                .AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Rubrics> UpdateWithCriteraAsync(Rubrics rubrics)
        {
            var creterias = ReboostDbContext.RubricCriteria.Where(c => c.RubricId == rubrics.Id);
            ReboostDbContext.RemoveRange(creterias);
            await ReboostDbContext.SaveChangesAsync();
            return await Update(rubrics);
        }

        public async Task<List<RubricsModel>> GetByQuestionId(int id)
        {
            var query = await (from q in ReboostDbContext.Questions
                               join r in ReboostDbContext.Rubrics on q.TaskId equals r.TaskId
                               join rc in ReboostDbContext.RubricCriteria on r.Id equals rc.RubricId
                               join rm in ReboostDbContext.RubricMilestones on rc.Id equals rm.CriteriaId
                               //into rcm from s in rcm.DefaultIfEmpty()
                               where q.Id == id && rc.Name != "Overall Score & Feedback"
                               select new RubricsQuery
                               {
                                   Id = rm.Id,
                                   CriteriaId = rc.Id,
                                   CriteriaDescription = rc.Description,
                                   HasScore = rc.HasScore,
                                   Name = rc.Name,
                                   BandScore = rm.BandScore,
                                   Description = rm.Description,
                                   OrderId = rc.OrderId
                               }).ToListAsync();


            var group = query.GroupBy(q => q.Name).Select(g => new RubricsModel
            {
                Name = g.Key,
                Id = g.FirstOrDefault()?.CriteriaId,
                Description = g.FirstOrDefault()?.CriteriaDescription,
                HasScore = g.FirstOrDefault().HasScore,
                OrderId = g.FirstOrDefault().OrderId,
                BandScoreDescriptions = g.ElementAt(0).Id == -1 ? new List<BandScoreDescription>() : g.Select(d => new BandScoreDescription
                {
                    Id = d.Id,
                    BandScore = d.BandScore,
                    Description = d.Description
                }).ToList()
            }).OrderBy( g => g.OrderId).ToList();

            return group;

        }
    }
}
