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

namespace Reboost.DataAccess.Repositories
{
    public interface IRubricRepository : IRepository<Rubrics>
    {
        Task<Rubrics> GetByIdAsync(int id);
        Task<Rubrics> UpdateWithCriteraAsync(Rubrics rubrics);
        Task<List<RubricsModel>> GetByQuestionId(int id);
    }
    public class RubricRepository : BaseRepository<Rubrics>, IRubricRepository
    {
        private ReboostDbContext ReboostDbContext => context as ReboostDbContext;
        public RubricRepository(ReboostDbContext context) : base(context)
        { }
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

                               where q.Id == id
                               select new RubricsQuery
                               {
                                   Id = rm.Id,
                                   CriteriaId = rc.Id,
                                   CriteriaDescription = rc.Description,
                                   Name = rc.Name,
                                   BandScore = rm.BandScore,
                                   Description = rm.Description
                               }).ToListAsync();


            //List<RubricsQuery> query = new List<RubricsQuery>();

            //using (var cmd = ReboostDbContext.Database.GetDbConnection().CreateCommand())
            //{
            //    if (cmd.Connection.State != ConnectionState.Open)
            //    {
            //        cmd.Connection.Open();
            //    }

            //    var slQuery = "select  m.Id, m.CriteriaId, c.Description as CriteriaDesription, c.Name, m.BandScore, m.Description from questions q"
            //        + " join rubrics r on q.taskId = r.taskId"
            //        + " left join rubricCriteria c on c.rubricId = r.id"
            //        + " left join rubricMilestones m on m.criteriaId = c.Id"
            //        + $" where q.Id = {id}";

            //    cmd.CommandText = slQuery;

            //    var rd = cmd.ExecuteReader();

            //    while (rd.Read())
            //    {
            //        var Id = rd["Id"] as int? ?? -1;
            //        var CriteriaId = rd["CriteriaId"] as int? ?? -1;
            //        var CriteriaDescription = rd["CriteriaDesription"] as string;
            //        var Name = rd["Name"] as string;
            //        var BandScore = rd["BandScore"] as int? ?? -1;
            //        var Description = rd["Description"] as string;

            //        query.Add(new RubricsQuery {
            //            Id = Id,
            //            CriteriaId = CriteriaId,
            //            CriteriaDescription = CriteriaDescription,
            //            Name = Name,
            //            BandScore = BandScore,
            //            Description = Description
            //        });
            //    }

            //    cmd.Connection.Close();
            //}


            var group = query.GroupBy(q => q.Name).Select(g => new RubricsModel
            {
                Name = g.Key,
                Id = g.FirstOrDefault()?.CriteriaId,
                Description = g.FirstOrDefault()?.CriteriaDescription,
                BandScoreDescriptions = g.ElementAt(0).Id == -1 ? new List<BandScoreDescription>() : g.Select(d => new BandScoreDescription
                {
                    Id = d.Id,
                    BandScore = d.BandScore,
                    Description = d.Description
                }).ToList()
            }).ToList();

            return group;

        }
    }
}
