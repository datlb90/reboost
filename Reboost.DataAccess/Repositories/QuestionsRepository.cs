using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface IQuestionsRepository : IRepository<Questions>
    {
        Task<Questions> GetByTitle(string title);
        Task<List<Tasks>> GetTaskAsync();
        Task<Dictionary<string, int>> CountQuestByTaskAsync();
        Task<List<QuestionModel>> GetAllExAsync();
        Task<QuestionModel> GetByIdAsync(int id);

    }
    public class QuestionsRepository : Repository<Questions>, IQuestionsRepository
    {
        public QuestionsRepository(ReboostDbContext context) : base(context)
        { }

        public async Task<Questions> GetByTitle(string title)
        {
            return await ReboostDbContext.Questions.Where(q => q.Title == title).FirstOrDefaultAsync();
        }

        public async Task<List<Tasks>> GetTaskAsync()
        {
            var rs = await ReboostDbContext.Tasks.AsNoTracking().ToListAsync();
            return rs;
        }
        private ReboostDbContext ReboostDbContext
        {
            get { return context as ReboostDbContext; }
        }
        public async Task<Dictionary<string, int>> CountQuestByTaskAsync()
        {
            Dictionary<string, int> dicTask = new Dictionary<string, int>();
            var questGroupByTask = ReboostDbContext.Questions.Include("Task").AsEnumerable().GroupBy(q => q.Task.Name);
            foreach (var group in questGroupByTask)
            {
                dicTask.Add(group.Key, group.Count());
            }
            var questCompleted = ReboostDbContext.Questions.AsEnumerable().GroupBy(q => q.Status);
            dicTask.Add("Completed", questCompleted.Count());

            return dicTask;
        }

        public async Task<List<QuestionModel>> GetAllExAsync()
        {

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select q.Id as Id, q.Title, ts.Name as Test, q.type as Type, q.hasSample as Sample, t.Name as Section, q.AverageScore as AverageScore, q.SubmissionCount as Submission, q.LikeCount as LikeCount, q.Status as Status"
                    + " From Questions q"
                    + " INNER JOIN Tasks t ON q.TaskId = t.Id"
                    + " INNER JOIN TestSections s ON t.SectionId = s.Id"
                    + " INNER JOIN Tests ts ON s.TestId = ts.Id";

                command.CommandType = CommandType.Text;

                context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<QuestionModel>();

                    while (result.Read())
                    {
                        entities.Add(new QuestionModel
                        {
                            Id = result.GetFieldValue<int>("Id"),
                            Title = result.GetFieldValue<string>("Title"),
                            Section = result.GetFieldValue<string>("Section"),
                            Test = result.GetFieldValue<string>("Test"),
                            Type = result.GetFieldValue<string>("Type"),
                            Sample = result.GetFieldValue<bool>("Sample"),
                            AverageScore = result.GetFieldValue<string>("AverageScore"),
                            Submission = result.GetFieldValue<int>("Submission"),
                            Like = result.GetFieldValue<int>("LikeCount"),
                            Status = result.GetFieldValue<string>("Status")
                        }); ;
                    }

                    return await Task.FromResult(entities);
                }
            }
        }

        public async Task<QuestionModel> GetByIdAsync(int id)
        {

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select q.Id as Id, q.Title, ts.Name as Test, q.type as Type, q.hasSample as Sample, t.Name as Section, q.AverageScore as AverageScore, q.SubmissionCount as Submission, q.LikeCount as LikeCount, q.DisLikeCount as DisLikeCount"
                    + " From Questions q"
                    + " INNER JOIN Tasks t ON q.TaskId = t.Id"
                    + " INNER JOIN TestSections s ON t.SectionId = s.Id"
                    + " INNER JOIN Tests ts ON s.TestId = ts.Id"
                    + " WHERE q.Id = " + id;

                command.CommandType = CommandType.Text;
                context.Database.OpenConnection();
                var resultParts = ReboostDbContext.QuestionParts.AsEnumerable().Where(qp => qp.QuestionId == id)?.ToList();
                var resultPartsModel = new List<QuestionPartModel>();
                resultParts.ForEach(rs =>
                {
                    resultPartsModel.Add(new QuestionPartModel
                    {
                        QuestionId = rs.QuestionId,
                        Content = rs.Content,
                        Name = rs.Name
                    });
                });
                using (var result = command.ExecuteReader())
                {
                    var entities = new QuestionModel();

                    while (result.Read())
                    {

                        entities.Id = result.GetFieldValue<int>("Id");
                        entities.Title = result.GetFieldValue<string>("Title");
                        entities.Section = result.GetFieldValue<string>("Section");
                        entities.Test = result.GetFieldValue<string>("Test");
                        entities.Type = result.GetFieldValue<string>("Type");
                        entities.Sample = result.GetFieldValue<bool>("Sample");
                        entities.AverageScore = result.GetFieldValue<string>("AverageScore");
                        entities.Submission = result.GetFieldValue<int>("Submission");
                        entities.Like = result.GetFieldValue<int>("LikeCount");
                        entities.Dislike = result.GetFieldValue<int>("DisLikeCount");
                        entities.QuestionsPart = resultPartsModel;

                    }
                    return await Task.FromResult(entities);
                }
            }
        }
    }
}
