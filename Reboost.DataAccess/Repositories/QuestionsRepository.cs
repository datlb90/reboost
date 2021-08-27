using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reboost.Shared;
using Reboost.DataAccess.Models;

namespace Reboost.DataAccess.Repositories
{
    public interface IQuestionsRepository : IRepository<Questions>
    {
        Task<Questions> GetByTitle(string title);
        Task<List<Tasks>> GetTaskAsync();
        Task<Dictionary<string, int>> CountQuestByTaskAsync();
        Task<List<QuestionModel>> GetAllExAsync();
        Task<QuestionModel> GetByIdAsync(int id);
        Task<List<SummaryPerUser>> GetSummaryByUserId(string userId);
        Task<List<string>> GetTestForCurrentUsers(string UserId);
        Task<List<int>> GetQuestionCompletedIdByUser(string UserId);
        Task<List<SampleForQuestion>> GetSamplesForQuestion(int questionId);
        Task<List<QuestionModel>> GetAllByUserAsync(string userId);
        Task<List<SubmissionsModel>> GetAllSubmissionByUserIdAsync(string userId);
        Task<QuestionDataModel> GetAllDataForAddOrEditQuestion();
        Task<QuestionRequestModel> CreateQuestionAsync(Questions q, QuestionRequestModel model);
        Task<QuestionRequestModel> UpdateQuestionAsync(Questions q, QuestionRequestModel model);
        Task<Questions> PublishQuestionAsync(int id);
        Task<Questions> ApproveQuestionAsync(int id);

    }
    public class QuestionsRepository : BaseRepository<Questions>, IQuestionsRepository
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

        public object SamplesForQuestion { get; private set; }

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
        public async Task<List<QuestionModel>> GetAllByUserAsync(string userId) {
            var tests = await GetTestForCurrentUsers(userId);
            var query = from quest in ReboostDbContext.Questions
                        join task in ReboostDbContext.Tasks on quest.TaskId equals task.Id
                        join sec in ReboostDbContext.TestSections on task.SectionId equals sec.Id
                        join test in ReboostDbContext.Tests on sec.TestId equals test.Id
                        where tests.Contains(test.Name) && quest.Status == QuestionStatus.ACTIVE
                        select new QuestionModel
                        {
                            Id = quest.Id,
                            Title = quest.Title,
                            Section = task.Name,
                            Test = test.Name,
                            Time = task.Time,
                            Type = quest.Type,
                            Sample = quest.HasSample,
                            AverageScore = quest.AverageScore,
                            Submission = quest.SubmissionCount,
                            Like = quest.LikeCount,
                            Status = (from sub in ReboostDbContext.Submissions where sub.UserId == userId && sub.QuestionId == quest.Id select sub).Count() > 0 ? "Completed" : "To do"
                        };
            return await query.ToListAsync();
        }

        public async Task<List<QuestionModel>> GetAllExAsync()
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select q.Id as Id, q.Title, ts.Name as Test, q.type as Type, q.hasSample as Sample, q.AddedDate as AddedDate, t.Name as Section, t.Time as Time, q.AverageScore as AverageScore, q.SubmissionCount as Submission, q.LikeCount as LikeCount, q.Status as Status, u.UserName as CreatedBy"
                    + " From Questions q"
                    + " INNER JOIN Tasks t ON q.TaskId = t.Id"
                    + " INNER JOIN TestSections s ON t.SectionId = s.Id"
                    + " INNER JOIN Tests ts ON s.TestId = ts.Id"
                    + " INNER JOIN AspNetUsers u ON q.UserId = u.Id";

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
                            Time = result.GetFieldValue<string>("Time"),
                            Type = result.GetFieldValue<string>("Type"),
                            Sample = result.GetFieldValue<bool>("Sample"),
                            AverageScore = result.GetFieldValue<string>("AverageScore"),
                            Submission = result.GetFieldValue<int>("Submission"),
                            Like = result.GetFieldValue<int>("LikeCount"),
                            Status = result.GetFieldValue<string>("Status"),
                            CreatedBy = result.GetFieldValue<string>("CreatedBy"),
                            AddedDate = result.GetFieldValue<DateTime>("AddedDate")
                        });
                    }

                    return await Task.FromResult(entities);
                }
            }
        }

        public async Task<QuestionModel> GetByIdAsync(int id)
        {

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select q.Id as Id, q.Title, ts.Name as Test, q.type as Type, q.hasSample as Sample, t.Name as Section, q.AverageScore as AverageScore, q.SubmissionCount as Submission, q.LikeCount as LikeCount, q.DisLikeCount as DisLikeCount, t.Direction as Direction, t.Time as Time, q.TaskId as TaskId, s.TestId as TestId, q.UserId as UserId, q.Status as Status"
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
                        entities.Time = result.GetFieldValue<string>("Time");
                        entities.Type = result.GetFieldValue<string>("Type");
                        entities.Sample = result.GetFieldValue<bool>("Sample");
                        entities.Status = result.GetFieldValue<string>("Status");
                        entities.AverageScore = result.GetFieldValue<string>("AverageScore");
                        entities.Submission = result.GetFieldValue<int>("Submission");
                        entities.Like = result.GetFieldValue<int>("LikeCount");
                        entities.Direction = result.GetFieldValue<string>("Direction");
                        entities.Dislike = result.GetFieldValue<int>("DisLikeCount");
                        entities.QuestionsPart = resultPartsModel;
                        entities.TaskId = result.GetFieldValue<int>("TaskId");
                        entities.TestId = result.GetFieldValue<int>("TestId");
                        entities.UserId = result.GetFieldValue<string>("UserId");
                    }
                    return await Task.FromResult(entities);
                }
            }
        }

        public async Task<List<SummaryPerUser>> GetSummaryByUserId(string userId)
        {
            var tests = await GetTestForCurrentUsers(userId);

            var query = from sub in ReboostDbContext.Submissions
                        join quest in ReboostDbContext.Questions on sub.QuestionId equals quest.Id
                        join task in ReboostDbContext.Tasks on quest.TaskId equals task.Id
                        join sec in ReboostDbContext.TestSections on task.SectionId equals sec.Id
                        join test in ReboostDbContext.Tests on sec.TestId equals test.Id
                        where sub.UserId == userId && tests.Contains(test.Name)
                        group quest by new { task.Name, quest.Id } into g
                        select new SummaryPerUser
                        {
                            Section = g.Key.Name,
                            Count = g.Count()
                        };

            var tasks = from task in ReboostDbContext.Tasks
                        select new SummaryPerUser
                        {
                            Section = task.Name,
                            Count = 0
                        };


            var listQuery = new List<SummaryPerUser>();
            foreach (var item in query)
            {
                var newquery = new SummaryPerUser();
                newquery.Section = item.Section;
                newquery.Count = item.Count;
                listQuery.Add(newquery);
            }

            var listTask = new List<SummaryPerUser>();
            foreach (var item in tasks)
            {
                var newquery = new SummaryPerUser();
                newquery.Section = item.Section;
                newquery.Count = item.Count;
                foreach (var _item in listQuery)
                {
                    if (newquery.Section == _item.Section && _item.Count > 0)
                    {
                        newquery.Count++;
                    }
                }
                listTask.Add(newquery);
            }

            var ielts = await (from task in ReboostDbContext.Tasks
                        where task.SectionId == 8
                        select task).ToListAsync();

            var toefls = await (from task in ReboostDbContext.Tasks
                               where task.SectionId == 4
                               select task).ToListAsync();

            if (tests.Count == 1)
            {
                if(tests[0].ToUpper() == TestsName.TOEFL)
                {
                    foreach (var item in ielts)
                    {
                        var t = listTask.Where(r => r.Section.Trim() == item.Name.Trim()).FirstOrDefault();
                        listTask.Remove(t);
                    }
                }
                else
                {
                    foreach (var item in toefls)
                    {
                        var t = listTask.Where(r => r.Section.Trim() != item.Name.Trim()).FirstOrDefault();
                        listTask.Remove(t);
                    }
                }
            }

            return listTask;
        }

        public async Task<List<string>> GetTestForCurrentUsers(string UserId)
        {
            return await (from us in ReboostDbContext.UserScores
                          join ts in ReboostDbContext.TestSections on us.SectionId equals ts.Id
                          join t in ReboostDbContext.Tests on ts.TestId equals t.Id
                          where us.UserId == UserId
                          group t by t.Name into g
                          select g.Key).ToListAsync();
        }
        public async Task<List<int>> GetQuestionCompletedIdByUser(string UserId)
        {
            return await (from qs in ReboostDbContext.Questions
                          join s in ReboostDbContext.Submissions on qs.Id equals s.QuestionId
                          where s.UserId == UserId
                          select qs.Id).ToListAsync();
        }

        public async Task<List<SampleForQuestion>> GetSamplesForQuestion(int questionId)
        {
            return await (from s in ReboostDbContext.Samples
                          join q in ReboostDbContext.Questions on s.QuestionId equals q.Id
                          where q.Id == questionId
                          select new SampleForQuestion { Id = s.Id, QuestionId = s.QuestionId, SampleText = s.SampleText, BandScore = s.BandScore, Comment = s.Comment, LastActivityDate = s.LastActivityDate, Title = q.Title }).ToListAsync();

        }
        private string GetAction(string status)
        {
            switch (status.Trim())
            {
                case SubmissionStatus.PENDING:
                case SubmissionStatus.SUBMITTED:
                case SubmissionStatus.COMPLETED:
                case SubmissionStatus.REVIEW_REQUESTED: return "View Submission";
                case SubmissionStatus.REVIEWED: return "View Review";
                default: return "View Submission";
            }
        }
        public async Task<List<SubmissionsModel>> GetAllSubmissionByUserIdAsync(string userId)
        {
            
            var listReviewRequest = await (from r in ReboostDbContext.ReviewRequests
                                           where r.UserId == userId
                                           select r
                                    ).ToListAsync();

            var listsubmissions = await (from s in ReboostDbContext.Submissions
                                         join q in ReboostDbContext.Questions on s.QuestionId equals q.Id
                                         where s.UserId == userId
                                         orderby s.UpdatedDate descending, s.SubmittedDate descending
                                         select new SubmissionsModel
                                         {
                                             Id = s.Id,
                                             QuestionId = q.Id,
                                             Question = q.Title,
                                             Status = s.Status,
                                             Action =
                                                (s.Status == SubmissionStatus.PENDING ||
                                                s.Status == SubmissionStatus.SUBMITTED ||
                                                s.Status == SubmissionStatus.REVIEW_REQUESTED) ? "View Submission" :
                                                (s.Status == SubmissionStatus.REVIEWED || 
                                                s.Status == SubmissionStatus.COMPLETED) ? "View Review" : "View Submission",
                                             TimeTaken = s.TimeSpentInSeconds,
                                             TimeSubmitted = s.SubmittedDate
                                         }).ToListAsync();

            //foreach (ReviewRequests r in listReviewRequest)
            //{
            //    //int flag = listsubmissions.Any(s => s.Id == r.SubmissionId && r.Status == "Completed") ? 2 : listsubmissions.Any(s => s.Id == r.SubmissionId) ? 1 : 0;
            //    //if (listsubmissions.Any(s => s.Id == r.SubmissionId && r.Status == "Completed"))
            //    //{
            //    //    int index = listsubmissions.FindIndex(s => s.Id == r.SubmissionId);
            //    //    listsubmissions[index].Action = "View Review";
            //    //    listsubmissions[index].Status = "Reviewed";
            //    //}
            //    //else if (listsubmissions.Any(s => s.Id == r.SubmissionId))
            //    //{
            //    //    int index = listsubmissions.FindIndex(s => s.Id == r.SubmissionId);
            //    //    listsubmissions[index].Action = "View Submission";
            //    //    listsubmissions[index].Status = "Requested";
            //    //}
            //}
            return listsubmissions;
        }

        public async Task<QuestionDataModel> GetAllDataForAddOrEditQuestion()
        {
            QuestionDataModel result = new QuestionDataModel();

            var tests = await ReboostDbContext.Tests.ToListAsync();
            var tasks = await ReboostDbContext.Tasks.Include("Section").ToListAsync();
            var types = await (from question in ReboostDbContext.Questions
                               select question.Type).Distinct().ToListAsync();

            result.Tests = tests;
            result.Tasks = tasks;
            result.Types = types;

            return result;
        }

        public async Task<QuestionRequestModel> CreateQuestionAsync(Questions q, QuestionRequestModel model)
        {
            await ReboostDbContext.Questions.AddAsync(q);
            await ReboostDbContext.SaveChangesAsync();

            foreach(var p in model.QuestionParts)
            {
                p.QuestionId = q.Id;
            }

            await ReboostDbContext.QuestionParts.AddRangeAsync(model.QuestionParts);
            await ReboostDbContext.SaveChangesAsync();

            return model;
        }

        public async Task<QuestionRequestModel> UpdateQuestionAsync(Questions q, QuestionRequestModel model)
        {
            var exist = await ReboostDbContext.Questions.FindAsync(q.Id);

            if(exist == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Question not existed!");
            }

            exist.TaskId = q.TaskId;
            exist.Title = q.Title;
            exist.Type = q.Type;
            await ReboostDbContext.SaveChangesAsync();

            var parts = await ReboostDbContext.QuestionParts.Where(p => p.QuestionId == q.Id).ToListAsync();
            ReboostDbContext.QuestionParts.RemoveRange(parts);

            await ReboostDbContext.QuestionParts.AddRangeAsync(model.QuestionParts);
            await ReboostDbContext.SaveChangesAsync();

            return model;
        }

        public async Task<Questions> PublishQuestionAsync(int id)
        {
            var question = await ReboostDbContext.Questions.FindAsync(id);

            question.Status = QuestionStatus.ACTIVE;

            await ReboostDbContext.SaveChangesAsync();

            return question;
        }

        public async Task<Questions> ApproveQuestionAsync(int id)
        {
            var question = await ReboostDbContext.Questions.FindAsync(id);

            question.Status = QuestionStatus.APPROVED;

            await ReboostDbContext.SaveChangesAsync();

            return question;
        }
    }
}
