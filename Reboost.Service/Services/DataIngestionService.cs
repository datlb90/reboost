using ExcelDataReader;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IDataIngestionService
    {
        Task IngestQuestionsAndSamples();
    }

    public class DataIngestionService : BaseService, IDataIngestionService
    {
        private readonly IQuestionsService _questionsService;
        private readonly IQuestionPartService _questionPartService;
        private readonly ISampleService _sampleService;

        public DataIngestionService(IUnitOfWork unitOfWork, IQuestionsService questionsService, 
            IQuestionPartService questionPartService, ISampleService sampleService) : base(unitOfWork)
        {
            _questionsService = questionsService;
            _questionPartService = questionPartService;
            _sampleService = sampleService;
        }

        public async Task IngestQuestionsAndSamples()
        {
            List<QuestionData> questionList = new List<QuestionData>();
            List<SampleData> sampleList = new List<SampleData>();
            string questionFilePath = "C:/Users/datba/Desktop/Data/Reboost Data/Questions.xlsx";
            using (var stream = System.IO.File.Open(questionFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int count = 0;
                    do
                    {
                        while (reader.Read() && count < reader.RowCount)
                        {
                            count++;
                            QuestionData data = new QuestionData();
                            data.Id = (int)reader.GetDouble(0);
                            data.Test = reader.GetString(1);
                            data.Task = reader.GetString(2);
                            switch (data.Task)
                            {
                                case "Independent Writing":
                                    data.TaskId = 1;
                                    break;
                                case "Integrated Writing":
                                    data.TaskId = 2;
                                    break;
                                case "Academic Task 1":
                                    data.TaskId = 3;
                                    break;
                                case "Academic Task 2":
                                    data.TaskId = 4;
                                    break;
                                case "General Training Task 1":
                                    data.TaskId = 5;
                                    break;
                                case "General Training Task 2":
                                    data.TaskId = 6;
                                    break;
                                default:
                                    data.TaskId = 0;
                                    break;
                            }
                            data.Question = reader.GetString(3);
                            data.Type = reader.GetString(4);
                            data.Title = reader.GetString(5);
                            data.MediaUrl = reader.GetString(6);
                            data.Reading = reader.GetString(7);
                            data.Transcript = reader.GetString(8);
                            questionList.Add(data);
                        }
                    } while (reader.NextResult());
                }
            }

            string sampleFilePath = "C:/Users/datba/Desktop/Data/Reboost Data/Samples.xlsx";
            using (var stream = System.IO.File.Open(sampleFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int count = 0;
                    do
                    {
                        while (reader.Read() && count < reader.RowCount)
                        {
                            count++;
                            SampleData data = new SampleData();
                            data.Id = (int)reader.GetDouble(0);
                            data.QuestionId = (int)reader.GetDouble(1);
                            data.SampleText = reader.GetString(2);
                            data.Rate = reader.GetValue(3) == null ? null : (decimal?)Convert.ToDecimal(reader.GetDouble(3));
                            data.Comment = reader.GetString(4);
                           
                            sampleList.Add(data);
                        }
                    } while (reader.NextResult());
                }
            }


            foreach(QuestionData question in questionList)
            {
                var thisQuestion = await _questionsService.GetByTitle(question.Title);
                if(thisQuestion == null)
                {
                    Questions newQuestion = new Questions();
                    newQuestion.TaskId = question.TaskId;
                    newQuestion.Type = question.Type;
                    newQuestion.Title = question.Title;
                    newQuestion.SubmissionCount = 0;
                    newQuestion.ViewCount = 0;
                    newQuestion.LikeCount = 0;
                    newQuestion.DisLikeCount = 0;
                    newQuestion.HasSample = sampleList.AsEnumerable().Any(s => s.QuestionId == question.Id);
                    newQuestion.AverageScore = "0.0";
                    newQuestion.Status = "Active";
                    newQuestion.AddedDate = DateTime.UtcNow;
                    newQuestion.LastActivityDate = DateTime.UtcNow;

                    Questions thisNewQuestion = await _questionsService.CreateAsync(newQuestion);
                    // Add question parts
                    QuestionParts newQuestionPart1 = new QuestionParts();
                    newQuestionPart1.QuestionId = thisNewQuestion.Id;
                    newQuestionPart1.Name = "Question";
                    newQuestionPart1.Content = question.Question;
                    newQuestionPart1.Order = 1;
                    await _questionPartService.CreateAsync(newQuestionPart1);

                    if(thisNewQuestion.TaskId == 2)
                    {
                        QuestionParts newQuestionPart2 = new QuestionParts();
                        newQuestionPart2.QuestionId = thisNewQuestion.Id;
                        newQuestionPart2.Name = "Reading";
                        newQuestionPart2.Content = question.Reading;
                        newQuestionPart2.Order = 2;
                        await _questionPartService.CreateAsync(newQuestionPart2);

                        QuestionParts newQuestionPart3 = new QuestionParts();
                        newQuestionPart3.QuestionId = thisNewQuestion.Id;
                        newQuestionPart3.Name = "Listening";
                        newQuestionPart3.Content = !String.IsNullOrEmpty(question.MediaUrl) ? question.MediaUrl : "Missing";
                        newQuestionPart3.Order = 3;
                        await _questionPartService.CreateAsync(newQuestionPart3);

                        QuestionParts newQuestionPart4 = new QuestionParts();
                        newQuestionPart4.QuestionId = thisNewQuestion.Id;
                        newQuestionPart4.Name = "Transcript";
                        newQuestionPart4.Content = !String.IsNullOrEmpty(question.Transcript) ? question.Transcript : "Missing";
                        newQuestionPart4.Order = 4;
                        await _questionPartService.CreateAsync(newQuestionPart4);

                    } 
                    else if(thisNewQuestion.TaskId == 3)
                    {
                        QuestionParts newQuestionPart2 = new QuestionParts();
                        newQuestionPart2.QuestionId = thisNewQuestion.Id;
                        newQuestionPart2.Name = "Chart";
                        newQuestionPart2.Content = !String.IsNullOrEmpty(question.MediaUrl) ? question.MediaUrl : "Missing";
                        newQuestionPart2.Order = 2;
                        await _questionPartService.CreateAsync(newQuestionPart2);
                    }

                    // Add new samples if any
                    if (thisNewQuestion.HasSample)
                    {
                        var samples = sampleList.AsEnumerable().Where(s => s.QuestionId == question.Id).ToList();

                        foreach(SampleData sample in samples)
                        {
                            Samples newSample = new Samples();
                            newSample.QuestionId = thisNewQuestion.Id;
                            newSample.SampleText = sample.SampleText;
                            newSample.BandScore = sample.Rate;
                            newSample.Comment = sample.Comment;
                            newSample.LastActivityDate = DateTime.UtcNow;
                            await _sampleService.CreateAsync(newSample);
                        }
                    }
                }
            }
        }
    }
}
