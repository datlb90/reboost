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
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace Reboost.DataAccess.Repositories
{
    public interface IQuestionsRepository : IRepository<Questions>
    {
        Task<Questions> GetQuestionByIdAsync(int id);
        Task<Tasks> GetTaksById(int taskId);
        Task<List<Questions>> GetAllActiveQuestions();
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
        Task<List<SubmissionsForQuestionModel>> GetSubmissionsForQuestion(string userId, int questionId);
    }
    public class QuestionsRepository : BaseRepository<Questions>, IQuestionsRepository
    {
        public QuestionsRepository(ReboostDbContext context) : base(context)
        { }

        public async Task<Questions> GetQuestionByIdAsync(int id)
        {
            return await ReboostDbContext.Questions.Where(q => q.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Tasks> GetTaksById(int taskId)
        {
            return await ReboostDbContext.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync();
        }

        public async Task<List<Questions>> GetAllActiveQuestions()
        {
            return await ReboostDbContext.Questions.Where(q => q.Status == "Active" && q.TaskId == 3).ToListAsync();
        }

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
            var allQuestions = from quest in ReboostDbContext.Questions
                        join task in ReboostDbContext.Tasks on quest.TaskId equals task.Id
                        join sec in ReboostDbContext.TestSections on task.SectionId equals sec.Id
                        join test in ReboostDbContext.Tests on sec.TestId equals test.Id
                        where quest.Status == QuestionStatus.ACTIVE && tests.Contains(test.Name)
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
                            Status = "To do",
                            Difficulty = quest.Difficulty
                        };

            var completed =  (from s in ReboostDbContext.Submissions
                              where s.UserId == userId && s.Status != "Saved"
                              select new
                              {
                                  Id = s.QuestionId,
                                  Status = "Completed"
                              }).Distinct();

            var saved = (from s in ReboostDbContext.Submissions
                             where s.UserId == userId && s.Status == "Saved"
                             select new
                             {
                                 Id = s.QuestionId,
                                 Status = "Saved"
                             }).Distinct();

            var result = from all in allQuestions
                         join comp in completed on all.Id equals comp.Id into allCompleted
                         from allComp in allCompleted.DefaultIfEmpty()
                         join save in saved on all.Id equals save.Id into allSaved
                         from allSave in allSaved.DefaultIfEmpty()
                         select new QuestionModel
                         {
                             Id = all.Id,
                             Title = all.Title,
                             Section = all.Section,
                             Test = all.Test,
                             Time = all.Time,
                             Type = all.Type,
                             Sample = all.Sample,
                             AverageScore = all.AverageScore,
                             Submission = all.Submission,
                             Like = all.Like,
                             Status = allComp.Status != null ? allComp.Status : allSave.Status != null ? allSave.Status : all.Status,
                             Difficulty = all.Difficulty
                         };

            return await result.ToListAsync();
        }

        public async Task<List<QuestionModel>> GetAllExAsync()
        {
            return await (from quest in ReboostDbContext.Questions
                    join task in ReboostDbContext.Tasks on quest.TaskId equals task.Id
                    join sec in ReboostDbContext.TestSections on task.SectionId equals sec.Id
                    join test in ReboostDbContext.Tests on sec.TestId equals test.Id
                    where quest.TaskId != 6 && quest.TaskId != 7
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
                        Status = quest.Status,
                        Difficulty = quest.Difficulty,
                        AddedDate = quest.AddedDate,
                        LastActivityDate = quest.LastActivityDate,
                    }).OrderByDescending(q => q.LastActivityDate).ToListAsync();

            //using (var command = context.Database.GetDbConnection().CreateCommand())
            //{
            //    command.CommandText = "select q.Id as Id, q.Title, ts.Name as Test, q.type as Type, q.hasSample as Sample, q.AddedDate as AddedDate, t.Name as Section, t.Time as Time, q.AverageScore as AverageScore, q.SubmissionCount as Submission, q.LikeCount as LikeCount, q.Status as Status, u.UserName as CreatedBy"
            //        + " From Questions q"
            //        + " INNER JOIN Tasks t ON q.TaskId = t.Id"
            //        + " INNER JOIN TestSections s ON t.SectionId = s.Id"
            //        + " INNER JOIN Tests ts ON s.TestId = ts.Id"
            //        + " INNER JOIN AspNetUsers u ON q.UserId = u.Id";

            //    command.CommandType = CommandType.Text;

            //    context.Database.OpenConnection();

            //    using (var result = command.ExecuteReader())
            //    {
            //        var entities = new List<QuestionModel>();

            //        while (result.Read())
            //        {
            //            entities.Add(new QuestionModel
            //            {
            //                Id = result.GetFieldValue<int>("Id"),
            //                Title = result.GetFieldValue<string>("Title"),
            //                Section = result.GetFieldValue<string>("Section"),
            //                Test = result.GetFieldValue<string>("Test"),
            //                Time = result.GetFieldValue<string>("Time"),
            //                Type = result.GetFieldValue<string>("Type"),
            //                Sample = result.GetFieldValue<bool>("Sample"),
            //                AverageScore = result.GetFieldValue<string>("AverageScore"),
            //                Submission = result.GetFieldValue<int>("Submission"),
            //                Like = result.GetFieldValue<int>("LikeCount"),
            //                Status = result.GetFieldValue<string>("Status"),
            //                CreatedBy = result.GetFieldValue<string>("CreatedBy"),
            //                AddedDate = result.GetFieldValue<DateTime>("AddedDate")
            //            });
            //        }

            //        return await Task.FromResult(entities);
            //    }
            //}
        }

        public async Task<QuestionModel> GetByIdAsync(int id)
        {

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select q.Id as Id, q.Title, ts.Name as Test, q.type as Type, q.hasSample as Sample, t.Name as Section, " +
                    "q.AverageScore as AverageScore, q.SubmissionCount as Submission, q.LikeCount as LikeCount, q.DisLikeCount as DisLikeCount, " +
                    "t.Direction as Direction, t.Time as Time, q.TaskId as TaskId, s.TestId as TestId, q.UserId as UserId, q.Status as Status, q.Difficulty as Difficulty"
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
                        entities.Difficulty = result.GetFieldValue<string>("Difficulty");
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
            var listsubmissions = await (from s in ReboostDbContext.Submissions
                                         join q in ReboostDbContext.Questions on s.QuestionId equals q.Id
                                         join t in ReboostDbContext.Tasks on q.TaskId equals t.Id
                                         join ts in ReboostDbContext.TestSections on q.Task.SectionId equals ts.Id
                                         where s.UserId == userId && s.Status != "Saved"
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
                                             TimeSubmitted = s.SubmittedDate,
                                             Test = ts.Test.Name,
                                             TestSection = t.Name,
                                             QuestionType = q.Type
                                         }).OrderByDescending(s => s.TimeSubmitted).ToListAsync();
            return listsubmissions;
        }
        public async Task<List<SubmissionsForQuestionModel>> GetSubmissionsForQuestion(string userId, int questionId)
        {

            var listsubmissions = await (from s in ReboostDbContext.Submissions
                                         join d in ReboostDbContext.Documents on s.DocId equals d.Id
                                         where s.UserId == userId && s.QuestionId == questionId
                                         orderby s.UpdatedDate descending, s.SubmittedDate descending
                                         select new SubmissionsForQuestionModel
                                         {
                                             Id = s.Id,
                                             SubmittedTimeStr = s.SubmittedDate.ToString("MM/dd/yyyy hh:mm tt"),
                                             Status = s.Status,
                                             Action = (s.Status == SubmissionStatus.REVIEWED || s.Status == SubmissionStatus.COMPLETED) ? "View Review" : "",
                                             TimeTaken = s.TimeSpentInSeconds,
                                             TimeSubmitted = s.SubmittedDate,
                                             PageCount = d.PageCount,
                                             Text = d.Text
                                         }).ToListAsync();


            return listsubmissions;
        }
        public async Task<QuestionDataModel> GetAllDataForAddOrEditQuestion()
        {
            QuestionDataModel result = new QuestionDataModel();

            var tests = await ReboostDbContext.Tests.ToListAsync();
            var tasks = await ReboostDbContext.Tasks.Where(t => t.Id != 6 && t.Id != 7).Include("Section").ToListAsync();
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

            if (model.UploadedFile!=null && model.UploadedFile.Count > 0)
            {
                foreach (var item in model.UploadedFile)
                {
                    if (item.ContentType == "video/mp4" || item.ContentType == "audio/mpeg")
                    {
                        var extensionPath = Path.GetExtension(item.FileName);
                        var fileName = model.Id + extensionPath;
                        var audioDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/audio");
                        if (!Directory.Exists(audioDirectory))
                        {
                            Directory.CreateDirectory(audioDirectory);
                        }
                        var filePath = Path.Combine(audioDirectory, fileName);
                        
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(fileStream);
                        }

                        foreach (var p in model.QuestionParts)
                        {
                            if (p.Name == "Listening")
                            {
                                p.Content = fileName;
                            }

                        }
                    }
                    else
                    {
                        var extensionPath = Path.GetExtension(item.FileName);
                        var fileName = model.Id + extensionPath;
                        var audioDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/photo");
                        if (!Directory.Exists(audioDirectory))
                        {
                            Directory.CreateDirectory(audioDirectory);
                        }
                        var filePath = Path.Combine(audioDirectory, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(fileStream);
                        }

                        foreach (var p in model.QuestionParts)
                        {
                            if (p.Name == "Chart")
                            {
                                p.Content = fileName;
                            }
                        }
                    }
                }
            }

            foreach (var p in model.QuestionParts)
            {
                p.QuestionId = q.Id;
            }

            await ReboostDbContext.QuestionParts.AddRangeAsync(model.QuestionParts);
            await ReboostDbContext.SaveChangesAsync();

            model.QuestionId = q.Id;
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
            exist.Difficulty = q.Difficulty;
            exist.LastActivityDate = DateTime.UtcNow;
            await ReboostDbContext.SaveChangesAsync();

            
            if (model.UploadedFile != null && model.UploadedFile.Count > 0)
            {
                var parts = await ReboostDbContext.QuestionParts.Where(p => p.QuestionId == q.Id).ToListAsync();
                ReboostDbContext.QuestionParts.RemoveRange(parts);

                foreach (var item in model.UploadedFile)
                {
                    if (item.ContentType == "video/mp4" || item.ContentType == "audio/mpeg")
                    {
                        var extensionPath = Path.GetExtension(item.FileName);
                        var fileName = model.Id + extensionPath;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/audio", fileName);

                        FileInfo file = new FileInfo(filePath);

                        if(file != null)
                        {
                           file.Delete();
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(fileStream);
                        }

                        foreach (var p in model.QuestionParts)
                        {
                            if (p.Name == "Listening")
                            {
                                p.Content = fileName;
                            }

                        }
                    }
                    else
                    {
                        var extensionPath = Path.GetExtension(item.FileName);
                        var fileName = model.Id + extensionPath;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/photo", fileName);

                        FileInfo file = new FileInfo(filePath);

                        if (file != null)
                        {
                            file.Delete();
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(fileStream);
                        }

                        foreach (var p in model.QuestionParts)
                        {
                            if (p.Name == "Chart")
                            {
                                p.Content = fileName;
                            }
                        }
                    }
                }

                await ReboostDbContext.QuestionParts.AddRangeAsync(model.QuestionParts);
                await ReboostDbContext.SaveChangesAsync();
            }
            else
            {
                // Get parts that are not chart, or listening 
                var parts = await ReboostDbContext.QuestionParts.Where(p => p.QuestionId == q.Id && p.Name != "Listening" && p.Name != "Chart").ToListAsync();
                // Delete these parts
                ReboostDbContext.QuestionParts.RemoveRange(parts);

                // Add all parts that are not chart, or listening 
                await ReboostDbContext.QuestionParts.AddRangeAsync(model.QuestionParts.Where(p => p.Name != "Listening" && p.Name != "Chart"));
                await ReboostDbContext.SaveChangesAsync();
            }

            return model;
        }

        public async Task<Questions> PublishQuestionAsync(int id)
        {
            var question = await ReboostDbContext.Questions.FindAsync(id);
            if (question != null)
            {
                question.Status = QuestionStatus.ACTIVE;
                await ReboostDbContext.SaveChangesAsync();
            }
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
