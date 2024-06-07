using Hangfire;
using Microsoft.Extensions.Configuration;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reboost.Shared.Extensions;
using Stripe;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using PayPal.Api;
using Newtonsoft.Json.Linq;
using OpenAI_API.Moderation;
using iTextSharp.text;

namespace Reboost.Service.Services
{
    public interface IReviewService
    {
        Task<EssayFeedback> getCriteriaFeedbackGPTTurbo(EssayFeedbackModel model);
        Task<EssayFeedback> getCriteriaFeedbackGPT4(EssayFeedbackModel model);

        Task<ReviewScores> getEssayScoreV2(CriteriaFeedbackModel model);
        Task<EssayFeedback> getEssayScoreV1(CriteriaFeedbackModel model);

        Task<string> getChartDescription(string userId, string fileName);
        Task<ReviewScores> getEssayScore(FeedbackRequestModel model);

        Task<ErrorsInText> getVocabularyErrorsInText(CriteriaFeedbackModel model);
        Task<ErrorsInText> getGrammarErrorsInText(CriteriaFeedbackModel model);

        Task<List<CriteriaFeedback>> GetCriteriaFeedback(User user, FeedbackRequestModel model);
        Task<ErrorsInText> getIntextComments(User user, CriteriaFeedbackModel model);

        Task<ErrorsInText> getIntextCommentsGPT4(CriteriaFeedbackModel model);
        Task<ErrorsInText> getIntextCommentsGPTTurbo(CriteriaFeedbackModel model);  
        Task<string> getAIFeedbackForCriteriaV5(CriteriaFeedbackModel model);
        Task<ErrorsInText> getErrorsInTextV2(CriteriaFeedbackModel model);
        Task<string> getExperimentAIFeedback(ExperimentFeedbackModel model);
       
        Task<ErrorsInText> getErrorsInText(CriteriaFeedbackModel model);
        Task<string> getAIFeedbackForCriteriaV4(CriteriaFeedbackModel model);
        Task<ReviewRatings> CreateAIReviewRatingAsync(ReviewRatings data);
        
        Task<string> getCriteriaFeedback(CriteriaFeedbackModel model);

        Task<string> getFeedbackForErrors(ErrorFeedbackModel model);
        Task<string> getAIFeedbackForCriteriaV2(CriteriaFeedbackModel model);
        Task<SubmissionRequestModel> GetSubmissionRequestModel(int submissionId, string userId);
        
        Task<AutomatedFeedbackModel> getAIFeedbackForCriteria(CriteriaFeedbackModel model);
        Task<GetReviewsModel> GetReviewByReviewRequestAsync(int requestId);
        Task<bool> EligibleForPeerReview(string userId);
        Task<Submissions> GetSubmissionById(int submissionId);
        Task<GetReviewsModel> GetAIReviewBySubmissionId(int submissionId);
        Task<GetReviewsModel> CreateAutomatedReview(string userId, int submissionId, string feedbackLanguage);
        Task<AnnotationModel> GetAnnotationsAsync(int docId, int reviewId);
        Task<IEnumerable<Annotations>> SaveAnnotationsAsync(int docId, int reviewId, IEnumerable<Annotations> annotations);
        Task<IEnumerable<InTextComments>> SaveCommentsAsync(IEnumerable<Annotations> annotations, IEnumerable<InTextComments> comments);
        Task<Reviews> SaveRubric(int reviewId, List<ReviewData> data);
        Task<Reviews> SaveFeedback(int reviewId, List<ReviewData> data);
        Task<List<ReviewData>> LoadFeedback(int reviewId);
        Task<Annotations> AddAnnotationAsync(Annotations annotation);
        Task<InTextComments> AddInTextCommentAsync(int docId, int reviewId, InTextComments cmt, Annotations anno);
        Task<InTextComments> DeleteInTextCommentAsync(int id);
        Task<int> DeleteAnnotationAsync(int id);
        Task<Annotations> EditAnnotationAsync(Annotations anno);
        Task<InTextComments> EditInTextComment(InTextComments cmt);
        Task<string> CreateNewSampleReviewDocumentAsync(string type, User user);
        Task<List<Reviews>> GetReviewsAsync();
        Task<List<Reviews>> GetReviewsByUserIdAsync(string userId);
        Task<GetReviewsModel> GetReviewByIdAsync(int id);
        Task<ReviewResponseModel> ChangeStatusAsync(int id, UpdateStatusModel model);
        Task<ReviewRequests> CreateRequestAsync(string userId, ReviewRequests requests);
        Task<List<GetReviewsModel>> GetRaterReviewsByIdAsync(String userId);
        Task<GetReviewsModel> GetOrCreateReviewByReviewRequestAsync(int submissionId, string userId);
        Task<int> CheckUserReviewValidationAsync(string role, User user, int reviewId);
        Task<int> CheckReviewValidationAsync(string role, User user, int reviewId);
        Task<ReviewRequests> GetReviewRequestBySubmissionId(int submissionId, string userId);
        Task<ReviewRatings> CreateReviewRatingAsync(ReviewRatings data, string userId);
        Task<ReviewRatings> GetReviewRatingsByReviewIdAsync(int reviewId);
        Task<RequestQueue> AddRequestQueue(RequestQueue data, string userId);
        Task<GetReviewsModel> CreateReviewFromQueue(string userId);
        Task<IEnumerable<Reviews>> GetUnRatedReviewsAsync(string userId);
        Task<GetReviewsModel> GetPendingReviewAsync(string userId);
        Task<CreatedProRequestModel> CreateProRequestAsync(ReviewRequests request);
        Task<CreatedProRequestModel> ReRequestProRequestAsync(ReviewRequests request);
        Task<GetReviewsModel> GetOrCreateReviewByProRequestId(int requestId, string currentUserId);
        Task<int> IsProRequestCheckAsync(int reviewId, string userId);
        Task<List<RaterTraining>> GetRaterTrainingsAsync(int raterId);
        Task<Raters> ReAssignRaterToAssignment(int requestId);
        Task<Disputes> CreateDisputeAsync(Disputes disputes);
        Task<List<Disputes>> GetAllDisputesAsync();
        Task<Disputes> GetDisputeByReviewIdAsync(int id);
        Task<Disputes> UpdateDisputeAsync(Disputes dispute);
        Task<List<Disputes>> GetAllLearnerDisputesAsync(string userId);
        Task<Reviews> Create(dynamic data);
        Task<List<ReviewRequestModel>> GetReviewRequestModel();
        Task<Raters> ReAssignReviewRequest(int requestId, int raterId);
        Task<Reviews> RecordPayment(int reviewId);
    }

    public class ReviewService : BaseService, IReviewService
    {
        IConfiguration configuration;
        IMailService mailService;
        IUserService userService;
        IChatGPTService chatGPTService;
        ISubscriptionService subscriptionService;

        public ReviewService(IUnitOfWork unitOfWork,
            IConfiguration _configuration,
            IMailService _mailService,
            IUserService _userService,
            IChatGPTService _chatGPTService,
            ISubscriptionService _subscriptionService,
            IRubricService _rubricService) : base(unitOfWork)
        {
            configuration = _configuration;
            mailService = _mailService;
            userService = _userService;
            chatGPTService = _chatGPTService;
            subscriptionService = _subscriptionService;
        }

        public async Task<EssayFeedback> getCriteriaFeedbackGPTTurbo(EssayFeedbackModel model)
        {
            return await chatGPTService.getCriteriaFeedbackGPTTurbo(model);
        }

        public async Task<EssayFeedback> getCriteriaFeedbackGPT4(EssayFeedbackModel model)
        {
            return await chatGPTService.getCriteriaFeedbackGPT4(model);
        }

        public async Task<ReviewScores> getEssayScoreV2(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getEssayScore(model);
        }

        public async Task<EssayFeedback> getEssayScoreV1(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getEssayScoreGPT4(model);
        }

        public async Task<string> getChartDescription(string userId, string fileName)
        {
            try
            {

                if (!String.IsNullOrEmpty(fileName))
                {
                    var user = await userService.GetByIdAsync(userId);
                    var userSubscription = await subscriptionService.GetUserSubscription(userId);
                    if (userSubscription != null || user.FreeToken > 0)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/photo", fileName);
                        return await chatGPTService.getChartDescription(filePath);
                    }
                      
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return "";
        }


        public async Task<ReviewScores> getEssayScore(FeedbackRequestModel model)
        {
            if (model.hasGrade)
            {
                return await _unitOfWork.ReviewScores.GetEssayScores(model.reviewId);
            }
            else
            {
                var user = await userService.GetByIdAsync(model.userId);
                var userSubscription = await subscriptionService.GetUserSubscription(model.userId);
                if (userSubscription != null || user.FreeToken > 0)
                {
                    try
                    {
                        CriteriaFeedbackModel scoreModel = new CriteriaFeedbackModel
                        {
                            essay = model.essay,
                            task = model.task,
                            topic = model.topic,
                            chartDescription = model.chartDescription
                        };

                        // Get essay score from ChatGPT
                        ReviewScores scores = await chatGPTService.getEssayScore(scoreModel);
                        // Save the scores into database
                        scores.ReviewId = model.reviewId;
                        scores.UpdatedDate = DateTime.Now;
                        scores.CreatedDate = DateTime.Now;
                        await _unitOfWork.ReviewScores.Create(scores);
                        // Return the score to the UI
                        return scores;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }
                return null;
            }
        }

        public async Task<List<CriteriaFeedback>> GetCriteriaFeedback(User user, FeedbackRequestModel model)
        {
            if (model.hasGrade)
            {
                // If the review already has grade, return the feedback from db
                return await _unitOfWork.Review.GetCriteriaFeedback(model.reviewId);
            }
            else
            {
                var userSubscription = await subscriptionService.GetUserSubscription(user.Id);
                if(userSubscription != null || user.FreeToken > 0)
                {
                    // If there is no grade, get the feedback from chatGPT, save the feedback in database, then return the feedback
                    List<CriteriaFeedback> result = new List<CriteriaFeedback>(); // display feedback result
                    List<ReviewData> reviewDataList = new List<ReviewData>(); // save feedback to database
                    try
                    {
                        // 1. Get the rubric for the question
                        var rubricCriteria = await _unitOfWork.Rubrics.GetFeedbackRubric(model.questionId, model.task);
                        var taskList = new Task<EssayFeedback>[rubricCriteria.Count];

                        // 2. Get feedback from chatGPT
                        for (int i = 0; i < rubricCriteria.Count; i++)
                        {
                            // Get chart description for Task Achievement?
                            EssayFeedbackModel requestModel = new EssayFeedbackModel
                            {
                                essay = model.essay,
                                task = model.task,
                                topic = model.topic,
                                chartDescription = model.chartDescription,
                                criteriaName = rubricCriteria[i].name,
                                feedbackLanguage = model.feedbackLanguage,
                                criteriaId = rubricCriteria[i].criteriaId,
                                order = rubricCriteria[i].order
                            };

                            if (userSubscription != null && userSubscription.PlanId >= 4)
                            {
                                // Phản hồi chuyên sâu 
                                taskList[i] = chatGPTService.getCriteriaFeedbackGPT4(requestModel);
                            }
                            else
                            {
                                // Phản hồi chi tiết
                                taskList[i] = chatGPTService.getCriteriaFeedbackGPTTurbo(requestModel);
                            }
                        }

                        var completedTasks = await Task.WhenAll(taskList);

                        //var scoreResults = completedTasks[0];
                        foreach (var completedTask in completedTasks)
                        {
                            CriteriaFeedback criteriaFeedback = new CriteriaFeedback
                            {
                                criteriaId = completedTask.criteriaId,
                                name = completedTask.criteriaName,
                                comment = completedTask.comment,
                                mark = 0
                                //mark = mark
                            };

                            result.Add(criteriaFeedback);

                            ReviewData reviewData = new ReviewData
                            {
                                ReviewId = model.reviewId,
                                CriteriaId = completedTask.criteriaId,
                                CriteriaName = completedTask.criteriaName,
                                Comment = completedTask.comment,
                                Score = 0
                            };
                            reviewDataList.Add(reviewData);
                        }
                        if(userSubscription == null && user.FreeToken > 0)
                        {
                            user.FreeToken = user.FreeToken - 1;
                            await _unitOfWork.Users.UpdateAsync(user);
                        }
                        return result;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }
                return null;
            }
        }
        public async Task<ErrorsInText> getIntextComments(User user, CriteriaFeedbackModel model)
        {
            var userSubscription = await subscriptionService.GetUserSubscription(user.Id);
            if (userSubscription != null || user.FreeToken > 0)
            {
                if (userSubscription != null && userSubscription.PlanId >= 4)
                {
                    // Phản hồi chuyên sâu 
                    return await getIntextCommentsGPT4(model);
                }
                else
                {
                    // Phản hồi chi tiết
                    return await getIntextCommentsGPTTurbo(model);
                }
            }

            return null;
        }

        public async Task<string> getAIFeedbackForCriteriaV5(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getAIFeedbackForCriteriaV5(model);
        }

        public async Task<ErrorsInText> getIntextCommentsGPTTurbo(CriteriaFeedbackModel model)
        {
            // Make a ChatGpt call for each paragraph
            ErrorsInText result = new ErrorsInText();
            result.errors = new List<ErrorInText>();
            model.essay = model.essay.Replace("\r", String.Empty);
            string[] paragraphs = model.essay.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var taskList = new Task<ErrorsInText>[paragraphs.Length];
            for (int i = 0; i < paragraphs.Length; i++)
            {
                CriteriaFeedbackModel requestModel = new CriteriaFeedbackModel
                {
                    essay = paragraphs[i],
                    task = model.task,
                    topic = model.topic,
                    feedbackLanguage = model.feedbackLanguage
                };

                taskList[i] = chatGPTService.getIntextCommentsGPTTurbo(requestModel);
            }

            var completedTasks = await Task.WhenAll(taskList);

            foreach (var completedTask in completedTasks)
            {
                if (completedTask != null && completedTask.errors != null && completedTask.errors.Count > 0)
                {
                    foreach (ErrorInText error in completedTask.errors)
                    {
                        if (!result.errors.Any(e => e.error == error.error))
                        {
                            result.errors.Add(error);
                        }
                    }
                    //result.errors = result.errors.Concat(completedTask.errors).ToList();
                }
            }

            return result;

            //return await chatGPTService.getIntextCommentsGPTTurbo(model);
        }

        public async Task<ErrorsInText> getIntextCommentsGPT4(CriteriaFeedbackModel model)
        {
            // Make a ChatGpt call for each paragraph
            ErrorsInText result = new ErrorsInText();
            result.errors = new List<ErrorInText>();
            model.essay = model.essay.Replace("\r", String.Empty);
            string[] paragraphs = model.essay.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var taskList = new Task<ErrorsInText>[paragraphs.Length];
            for (int i = 0; i < paragraphs.Length; i++)
            {
                CriteriaFeedbackModel requestModel = new CriteriaFeedbackModel
                {
                    essay = paragraphs[i],
                    task = model.task,
                    topic = model.topic,
                    feedbackLanguage = model.feedbackLanguage
                };

                taskList[i] = chatGPTService.getIntextCommentsGPT4(requestModel);
            }

            var completedTasks = await Task.WhenAll(taskList);

            foreach (var completedTask in completedTasks)
            {
                if (completedTask != null && completedTask.errors != null && completedTask.errors.Count > 0)
                {
                    foreach (ErrorInText error in completedTask.errors)
                    {
                        if (!result.errors.Any(e => e.error == error.error))
                        {
                            result.errors.Add(error);
                        }
                    }
                    //result.errors = result.errors.Concat(completedTask.errors).ToList();
                }
            }

            return result;
        }


        //public async Task<ErrorsInText> getIntextCommentsGPT4(CriteriaFeedbackModel model)
        //{
        //    // Divide the essay into 2 parts. Make 2 chatgpt calls using Task.WhenAll
        //    model.essay = model.essay.Replace("\r", String.Empty);
        //    string[] paragraphs = model.essay.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        //    if (paragraphs.Length >= 2)
        //    {
        //        int median = paragraphs.Length / 2;

        //        string[] firstList = paragraphs.Take(median).ToArray();
        //        string firstPart = String.Join(Environment.NewLine, firstList);


        //        int remain = paragraphs.Length - median;
        //        string[] secondList = paragraphs.Skip(median).Take(remain).ToArray();
        //        string secondPart = String.Join(Environment.NewLine, secondList);


        //        CriteriaFeedbackModel model1 = new CriteriaFeedbackModel
        //        {
        //            essay = firstPart,
        //            task = model.task,
        //            topic = model.topic,
        //            chartDescription = model.chartDescription,
        //            criteriaName = model.criteriaName,
        //            feedbackLanguage = model.feedbackLanguage
        //        };

        //        CriteriaFeedbackModel model2 = new CriteriaFeedbackModel
        //        {
        //            essay = secondPart,
        //            task = model.task,
        //            topic = model.topic,
        //            chartDescription = model.chartDescription,
        //            criteriaName = model.criteriaName,
        //            feedbackLanguage = model.feedbackLanguage
        //        };


        //        var taskList = new[]
        //        {
        //            chatGPTService.getIntextCommentsGPT4(model1),
        //            chatGPTService.getIntextCommentsGPT4(model2)
        //        };

        //        var completedTasks = await Task.WhenAll(taskList);
        //        ErrorsInText result = new ErrorsInText();
        //        result.errors = new List<ErrorInText>();

        //        foreach (var completedTask in completedTasks)
        //        {
        //            if (completedTask != null && completedTask.errors != null && completedTask.errors.Count > 0)
        //            {
        //                foreach (ErrorInText error in completedTask.errors)
        //                {
        //                    if (!result.errors.Any(e => e.error == error.error))
        //                    {
        //                        result.errors.Add(error);
        //                    }
        //                }
        //                //result.errors = result.errors.Concat(completedTask.errors).ToList();
        //            }

        //        }

        //        return result;
        //    }

        //    return await chatGPTService.getIntextCommentsGPTTurbo(model);
        //}


        public async Task<Reviews> SaveFeedback(int reviewId, List<ReviewData> data)
        {
            return await _unitOfWork.Review.SaveFeedback(reviewId, data);
        }


        public async Task<ErrorsInText> getErrorsInTextV2(CriteriaFeedbackModel model)
        {
            // 1. Check if the learner has premium status
            // If true: make 2 chatgpt calls using Task.WhenAll
            // Else: make only 1 chatgpt calls

            ErrorsInText result = new ErrorsInText();
            result.errors = new List<ErrorInText>();
            // Make 2 chatgpt calls using Task.WhenAll
            string[] paragraphs = model.essay.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var taskList = new Task<ErrorsInText>[paragraphs.Length];
            for (int i = 0; i < paragraphs.Length; i++)
            {
                CriteriaFeedbackModel requestModel = new CriteriaFeedbackModel
                {
                    essay = paragraphs[i],
                    task = model.task,
                    topic = model.topic,
                    chartDescription = model.chartDescription,
                    criteriaName = model.criteriaName,
                    feedbackLanguage = model.feedbackLanguage
                };

                taskList[i] = chatGPTService.getErrorsInTextV2(requestModel);
            }

            var completedTasks = await Task.WhenAll(taskList);

            foreach (var completedTask in completedTasks)
            {
                if (completedTask != null && completedTask.errors != null && completedTask.errors.Count > 0)
                {
                    result.errors = result.errors.Concat(completedTask.errors).ToList();
                }
            }

            // Todo: Use result for saving critical errors into database

            return result;
        }

        public async Task<string> getExperimentAIFeedback(ExperimentFeedbackModel model)
        {
            return await chatGPTService.getExperimentAIFeedback(model);
        }

        public async Task<ErrorsInText> getVocabularyErrorsInText(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getVocabularyErrorsInText(model);
        }

        public async Task<ErrorsInText> getGrammarErrorsInText(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getGrammarErrorsInText(model);
        }

        public async Task<ErrorsInText> getErrorsInText(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getErrorsInText(model);
        }
        public async Task<string> getAIFeedbackForCriteriaV4(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getAIFeedbackForCriteriaV4(model);
        }
        public async Task<ReviewRatings> CreateAIReviewRatingAsync(ReviewRatings data)
        {
            var rs = await _unitOfWork.Review.CreateAIReviewRatingAsync(data);
            return rs;
        }

        public async Task<string> getCriteriaFeedback(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getCriteriaFeedback(model);
        }

        public async Task<string> getFeedbackForErrors(ErrorFeedbackModel model)
        {
            return await chatGPTService.getFeedbackForErrors(model);
        }

        public async Task<SubmissionRequestModel> GetSubmissionRequestModel(int submissionId, string userId)
        {
            return await _unitOfWork.Review.GetSubmissionRequestModel(submissionId, userId);
        }

        public async Task<string> getAIFeedbackForCriteriaV2(CriteriaFeedbackModel model)
        {
            model.topic = StripHTML(model.topic);
            return await chatGPTService.getAIFeedbackForCriteriaV2(model);
        }

        public async Task<AutomatedFeedbackModel> getAIFeedbackForCriteria(CriteriaFeedbackModel model)
        {
            AutomatedFeedbackModel result = await chatGPTService.getAIFeedbackForCriteria(model);
            string comment = "";
            switch (model.criteriaName)
            {
                case "Task Response":
                case "Task Achievement":
                case "Coherence & Cohesion":
                case "Lexical Resource":
                case "Grammatical Range & Accuracy":
                    comment = "";
                    if (model.feedbackLanguage == "vn")
                    {
                        comment += "- Những điểm bài viết đã làm tốt:";
                    }
                    else
                    {
                        comment += "- What the essay did well:";
                    }

                    comment += Environment.NewLine;
                    comment += result.feedback.strengths;
                    comment += Environment.NewLine;
                    comment += Environment.NewLine;
                    if (model.feedbackLanguage == "vn")
                    {
                        comment += "- Những điểm bài viết làm chưa tốt:";
                    }
                    else
                    {
                        comment += "- Areas for improvement:";
                    }

                    comment += Environment.NewLine;
                    comment += result.feedback.weaknesses;
                    comment += Environment.NewLine;
                    comment += Environment.NewLine;

                    if (model.feedbackLanguage == "vn")
                    {
                        comment += "- Hướng dẫn để học viên cải thiện:";
                    }
                    else
                    {
                        comment += "- Recommendations:";
                    }
                    comment += Environment.NewLine;
                    comment += result.feedback.recommendations;

                    result.comment = comment;
                    break;
                case "Overall Score & Feedback":
                    comment = "";
                    if (model.feedbackLanguage == "vn")
                    {
                        comment += "- Đánh giá tổng quan:";
                    }
                    else
                    {
                        comment += "- Overvall performance:";
                    }

                    comment += Environment.NewLine;
                    comment += result.feedback.performance;
                    comment += Environment.NewLine;
                    comment += Environment.NewLine;
                    if (model.feedbackLanguage == "vn")
                    {
                        comment += "- Những điểm mạnh nổi bật:";
                    }
                    else
                    {
                        comment += "- Standout strengths:";
                    }


                    comment += Environment.NewLine;
                    comment += result.feedback.strengths;
                    comment += Environment.NewLine;
                    comment += Environment.NewLine;
                    if (model.feedbackLanguage == "vn")
                    {
                        comment += "- Những điểm cần cải thiện:";
                    }
                    else
                    {
                        comment += "- Areas for improvement:";
                    }

                    comment += Environment.NewLine;
                    comment += result.feedback.weaknesses;
                    comment += Environment.NewLine;
                    comment += Environment.NewLine;

                    if (model.feedbackLanguage == "vn")
                    {
                        comment += "- Khuyễn nghị cho học viên:";
                    }
                    else
                    {
                        comment += "- Recommendations:";
                    }
                    comment += Environment.NewLine;
                    comment += result.feedback.recommendations;

                    result.comment = comment;
                    break;
                default:
                    break;
            }
            result.bandScore = (decimal)(Math.Round(result.score * 2, MidpointRounding.AwayFromZero) / 2); // (decimal)Math.Floor(result.score);
            return result;
        }

        public async Task<GetReviewsModel> CreateAutomatedReview(string userId, int submissionId, string feedbackLanguage = "vn")
        {
            Submissions submission = await _unitOfWork.Submission.GetByIdAsync(submissionId);

            // Delete any review request with this submission
            ReviewRequests existingRequest = await _unitOfWork.Review.GetReviewRequestBySubmissionId(submissionId, userId);
            if(existingRequest != null)
            {
                await _unitOfWork.Review.DeleteReviewRequest(existingRequest);
            }

            // Create a new ReviewRequest for this submission
            ReviewRequests request = new ReviewRequests
            {
                UserId = userId,
                SubmissionId = submissionId,
                FeedbackType = "AI",
                RequestedDateTime = DateTime.UtcNow,
                CompletedDateTime = DateTime.UtcNow,
                Status = "Completed",
                FeedbackLanguage = feedbackLanguage,
            };
            ReviewRequests newRequest = await _unitOfWork.Review.CreateReviewRequest(request);

            // Create the review without review data
            Reviews review = new Reviews
            {
                RequestId = newRequest.Id,
                ReviewerId = "AI",
                RevieweeId = userId,
                SubmissionId = submissionId,
                FinalScore = null,
                Status = ReviewStatus.IN_PROGRESS,
                TimeSpentInSeconds = 0,
                LastActivityDate = DateTime.UtcNow
            };
            var newReview = await _unitOfWork.Review.Create(review);

            // Send the result model to the front end so we can display the review page immediately
            GetReviewsModel result = new GetReviewsModel();
            result.QuestionId = submission.QuestionId;
            result.Review = newReview;
            result.ReviewId = newReview.Id;
            result.DocId = submission.DocId;
            return result;
        }

        public async Task<GetReviewsModel> GetReviewByReviewRequestAsync(int requestId)
        {
            return await _unitOfWork.Review.GetReviewByReviewRequestAsync(requestId);
        }
        public async Task<bool> EligibleForPeerReview(string userId)
        {
            return await _unitOfWork.Review.EligibleForPeerReview(userId);
        }

        public async Task<Submissions> GetSubmissionById(int submissionId)
        {
            return await _unitOfWork.Submission.GetByIdAsync(submissionId);
        }
        public async Task<GetReviewsModel> GetAIReviewBySubmissionId(int submissionId)
        {
            return await _unitOfWork.Review.GetAIReviewBySubmissionId(submissionId);
        }
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public async Task<AnnotationModel> GetAnnotationsAsync(int docId, int reviewId)
        {
            return await _unitOfWork.Review.GetAnnotationsAsync(docId, reviewId);
        }
        public async Task<int> CheckUserReviewValidationAsync(string role, User user, int reviewId)
        {
            return await _unitOfWork.Review.CheckUserReviewValidationAsync(role, user, reviewId);
        }
        public async Task<IEnumerable<Annotations>> SaveAnnotationsAsync(int docId, int reviewId, IEnumerable<Annotations> annotations )
        {
            foreach (var anot in annotations)
            {
                anot.ReviewId = reviewId;
            }
            return await _unitOfWork.Review.SaveAnnotationsAsync(docId, reviewId, annotations);
        }
        public async Task<IEnumerable<InTextComments>> SaveCommentsAsync(IEnumerable<Annotations> annotations, IEnumerable<InTextComments> comments )
        {
            var _comments = new List<InTextComments>();
            foreach (var cmt in comments)
            {
                var annot = annotations.Where(a => a.Uuid == cmt.Uuid).FirstOrDefault();
                if(annot != null)
                {
                    cmt.Annotation = null;
                    cmt.AnnotationId = annot.Id;
                    _comments.Add(cmt);
                }
            }
            return await _unitOfWork.Review.SaveCommentsAsync(_comments);
        }
        public async Task<Reviews> SaveRubric(int reviewId, List<ReviewData> data)
        {
            var rs = await _unitOfWork.Review.SaveRubric(reviewId, data);

            if (rs == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Review or Rater not existed");
            }

            return rs;
        }
        public async Task<List<ReviewData>> LoadFeedback(int reviewId)
        {
            return await _unitOfWork.Review.LoadFeedBack(reviewId);
        }
        public async Task<Annotations> AddAnnotationAsync(Annotations annotation)
        {
            return await _unitOfWork.Review.AddAnnotationAsync(annotation);
        }
        public async Task<InTextComments> AddInTextCommentAsync(int docId, int reviewId, InTextComments cmt, Annotations anno)
        {
            
            if(anno.Id == 0)
            {
                await _unitOfWork.Review.AddAnnotationAsync(anno);
            }

            cmt.AnnotationId = anno.Id;
            return await _unitOfWork.Review.AddInTextCommentAsync(cmt);
        }
        public async Task<InTextComments> DeleteInTextCommentAsync(int id)
        {
            return await _unitOfWork.Review.DeleteInTextCommentAsync(id);
        }
        public async Task<Annotations> EditAnnotationAsync(Annotations anno)
        {
            return await _unitOfWork.Review.EditAnnotationAsync(anno);
        }
        public async Task<int> DeleteAnnotationAsync(int id)
        {
            return await _unitOfWork.Review.DeleteAnnotationAsync(id);
        }
        public async Task<InTextComments> EditInTextComment(InTextComments cmt)
        {
            return await _unitOfWork.Review.EditInTextComment(cmt);
        }
        public async Task<string> CreateNewSampleReviewDocumentAsync(string type, User user)
        {
            return await _unitOfWork.Review.CreateNewSampleReviewDocumentAsync(type, user);
        }
        public async Task<List<Reviews>> GetReviewsAsync()
        {
            return await _unitOfWork.Review.GetReviewsAsync();
        }
        public async Task<ReviewResponseModel> ChangeStatusAsync(int id, UpdateStatusModel model)
        {
            return await _unitOfWork.Review.ChangeStatusAsync(id, model);
        }
        public async Task<List<Reviews>> GetReviewsByUserIdAsync(string userId)
        {
            return await _unitOfWork.Review.GetReviewsByUserIdAsync(userId);
        }
        public async Task<ReviewRequests> CreateRequestAsync(string userId, ReviewRequests requests)
        {
            DateTime now = DateTime.UtcNow;
            requests.RequestedDateTime = now;
            return await _unitOfWork.Review.CreateRequestAsync(userId, requests);
        }
        public async Task<RequestQueue> AddRequestQueue(RequestQueue requestQueue, string userId) {
            return await _unitOfWork.Review.AddRequestQueue(requestQueue, userId);
        }
        public async Task<List<GetReviewsModel>> GetRaterReviewsByIdAsync(String userId)
        {
            return await _unitOfWork.Review.GetRaterReviewsByIdAsync(userId);
        }
        public async Task<GetReviewsModel> GetOrCreateReviewByReviewRequestAsync(int requestId, string userId)
        {
            return await _unitOfWork.Review.GetOrCreateReviewByReviewRequestAsync(requestId, userId);
        }
        public async Task<ReviewRequests> GetReviewRequestBySubmissionId(int submissionId, string userId)
        {
            return await _unitOfWork.Review.GetReviewRequestBySubmissionId(submissionId, userId);
        }
        public async Task<int> CheckReviewValidationAsync(string role, User user, int reviewId)
        {
            return await _unitOfWork.Review.CheckReviewValidationAsync(role, user, reviewId);
        }
        public async Task<ReviewRatings> CreateReviewRatingAsync(ReviewRatings data, string userId)
        {
            var rs = await _unitOfWork.Review.CreateReviewRatingAsync(data, userId);

            var review = await _unitOfWork.Review.GetReviewByIdAsync(rs.ReviewId);
            
            if (review != null)
            {
                User user = null;
                if (review.Rater != null)
                    user = await userService.GetByIdAsync(review.Rater.UserId);
                else
                    user = await userService.GetByIdAsync(review.Review.ReviewerId);

                if(user != null)
                {
                    string url = $"{configuration["ClientUrl"]}/{review.Submission.QuestionId}/{review.Submission.DocId}/{review.ReviewId}";
                    await mailService.SendEmailAsync(user.Email, "Review Rated!", $"Your review is rated. Link at: <a href='{url}'>Clicking here</a>");
                }
            }

            return rs;
        }
        public async Task<ReviewRatings> GetReviewRatingsByReviewIdAsync(int reviewId)
        {
            return await _unitOfWork.Review.GetReviewRatingsByReviewIdAsync(reviewId);
        }
        public async Task<GetReviewsModel> CreateReviewFromQueue(string userId)
        {
            return await _unitOfWork.Review.CreateReviewFromQueue(userId);
        }
        public async Task<IEnumerable<Reviews>> GetUnRatedReviewsAsync(string userId)
        {
            return await _unitOfWork.Review.GetUnRatedReviewOfUser(userId);
        }
        public async Task<GetReviewsModel> GetPendingReviewAsync(string userId)
        {
            return await _unitOfWork.Review.GetPendingReviewAsync(userId);
        }
        public async Task<GetReviewsModel> GetReviewByIdAsync(int id)
        {
            return await _unitOfWork.Review.GetReviewByIdAsync(id);
        }
        public async Task<CreatedProRequestModel> CreateProRequestAsync(ReviewRequests request)
        {
            var rs = await _unitOfWork.Review.CreateProRequestAsync(request);

            if (rs == null)
            {
                return null;
            }

            string url = $"{configuration["ClientUrl"]}/review/pro/" + rs.Request.Id;
            // Send mail to rater with confirm link
            await mailService.SendEmailAsync(rs.Rater.User.Email, "Yêu Cầu Chấm Bài Mới Từ Reboost", $"Hi {rs.Rater.User.FirstName},<p>Bạn đã nhận được 1 yêu cầu chấm bài mới từ học viên của Reboost.</p> <p>Hãy hoàn thành bài chấm trong vòng 24h sử dụng đường link sau đây: <a href='{url}'>link chấm bài</a></p><p>Nếu bạn có thắc mắc gì xin vui lòng liên hệ trực tiếp với chúng tôi qua luồng email này.</p><p>Cảm ơn bạn,</p><p>Reboost Support</p>");

            return rs;
        }
        public async Task<CreatedProRequestModel> CreateProRequestWithCertifiedRatersAsync(ReviewRequests request)
        {
            var rs = await _unitOfWork.Review.CreateProRequestAsync(request);

            if (rs == null)
            {
                return null;
            }

            string url = $"{configuration["ClientUrl"]}/review/pro/" + rs.Request.Id;

            await mailService.SendEmailAsync(rs.Rater.User.Email, "New Writing Review Request!", $"You have a new pro request. You have 10 minutes to accept the request and 3 hours to finish the review after acceptance. Link at: <a href='{url}'>Clicking here</a>");

            // ReAssign to another rater if request not accepted after 10 minutes
            BackgroundJob.Schedule(() => ReAssignRater(rs.Request.Id), TimeSpan.FromMinutes(10));

            return rs;
        }
        public async Task ReAssignRater(int requestId)
        {
            var rater = await ReAssignRaterToAssignment(requestId);
            if (rater != null)
            {
                string url = $"{configuration["ClientUrl"]}/review/pro/" + requestId;
                // Send mail to rater with confirm link
                await mailService.SendEmailAsync(rater.User.Email, "New Writing Review Request!", $"You have a new pro request. You have 10 minutes to accept the request and 3 hours to finish the review after acceptance. Link at: <a href='{url}'>Clicking here</a>");
            }
        }
        public async Task<CreatedProRequestModel> ReRequestProRequestAsync(ReviewRequests request)
        {
            var rs = await _unitOfWork.Review.ReRequestProRequestAsync(request);

            if(rs == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "No rater available!");
            }

            string url = $"{configuration["ClientUrl"]}/review/pro/" + rs.Request.Id;
            // Send mail to rater with confirm link
            await mailService.SendEmailAsync(rs.Rater.User.Email, "Yêu Cầu Chấm Bài Mới Từ Reboost", $"Hi {rs.Rater.User.FirstName},<p>Bạn đã nhận được 1 yêu cầu chấm bài mới từ học viên của Reboost.</p> <p>Hãy hoàn thành bài chấm trong vòng 24h sử dụng đường link sau đây: <a href='{url}'>link chấm bài</a></p><p>Nếu bạn có thắc mắc gì xin vui lòng liên hệ trực tiếp với chúng tôi qua luồng email này.</p><p>Cảm ơn bạn,</p><p>Reboost Support</p>");

            //await mailService.SendEmailAsync(rs.Rater.User.Email, "New Writing Review Request!", $"You have a new pro request. Please complete the review within 24 hours using the following link: <a href='{url}'>Clicking here</a>");

            return rs;
        }
        public async Task<CreatedProRequestModel> ReRequestProRequestWithCertifiedRatersAsync(ReviewRequests request)
        {
            var rs = await _unitOfWork.Review.ReRequestProRequestAsync(request);

            if (rs == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "No rater available!");
            }

            string url = $"{configuration["ClientUrl"]}/review/pro/" + rs.Request.Id;
            // Send mail to rater with confirm link
            await mailService.SendEmailAsync(rs.Rater.User.Email, "New Writing Review Request!", $"You have a new pro request. You have 10 minutes to accept the request and 3 hours to finish the review after acceptance. Link at: <a href='{url}'>Clicking here</a>");

            // ReAssign to another rater if request not accepted after 10 minutes
            BackgroundJob.Schedule(
                () => ReAssignRater(rs.Request.Id), TimeSpan.FromMinutes(10)
            );

            return rs;
        }
        public async Task<GetReviewsModel> GetOrCreateReviewByProRequestId(int requestId, string currentUserId)
        {
            return await _unitOfWork.Review.GetOrCreateReviewByProRequestId(requestId, currentUserId);
        }
        public async Task<int> IsProRequestCheckAsync(int reviewId, string userId)
        {
            return await _unitOfWork.Review.IsProRequestCheckAsync(reviewId, userId);
        }
        public async Task<List<RaterTraining>> GetRaterTrainingsAsync(int raterId)
        {
            return await _unitOfWork.Review.GetRaterTrainingsAsync(raterId);
        }
        public async Task<Raters> ReAssignRaterToAssignment(int requestId)
        {
            return await _unitOfWork.Review.ReAssignRaterToAssignment(requestId);
        }
        public async Task<Disputes> CreateDisputeAsync(Disputes disputes)
        {
            var rs = await _unitOfWork.Review.CreateDisputeAsync(disputes);

            if (rs == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Review or Submission not existed");
            }

            var review = await _unitOfWork.Review.GetReviewByIdAsync(rs.ReviewId);

            if(review.Rater != null)
            {
                var user = await userService.GetByIdAsync(review.Rater.UserId);

                string url = $"{configuration["ClientUrl"]}" + rs.ReviewUrl;
                await mailService.SendEmailAsync(user.Email, "Dispute Created!", $"Your review is disputed. Link at: <a href='{url}'>Clicking here</a>");
            }

            return rs;
        }
        public async Task<List<Disputes>> GetAllDisputesAsync()
        {
            return await _unitOfWork.Review.GetAllDisputesAsync();
        }
        public async Task<Disputes> GetDisputeByReviewIdAsync(int id)
        {
            return await _unitOfWork.Review.GetDisputeByReviewIdAsync(id);
        }

        public async Task<Disputes> UpdateDisputeAsync(Disputes dispute)
        {
            var rs = await _unitOfWork.Review.UpdateDisputeAsync(dispute);

            if(rs == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Dispute not existed");
            }

            var reviewRequest = await GetOrCreateReviewByProRequestId(rs.Review.RequestId, rs.UserId);

            if (reviewRequest.ReviewRequest != null)
            {
                if(rs.Status == DisputeStatus.ACCEPTED && reviewRequest.ReviewRequest.FeedbackType == ReviewRequestType.PRO)
                {
                    //await paymentService.LearnerRefund(rs.UserId, rs.User.Email);
                    await ReRequestProRequestAsync(reviewRequest.ReviewRequest);
                }

                if(reviewRequest.ReviewRequest.FeedbackType == ReviewRequestType.FREE)
                {
                    throw new AppException(ErrorCode.InvalidArgument, "Can not refunded because this is an FREE request. Dispute's status change to ACCEPTED instead!");
                }
            }
            else
            {
                dispute.Status = DisputeStatus.OPEN;
                await _unitOfWork.Review.UpdateDisputeAsync(dispute);
            }

            return rs;
        }
        public async Task<List<Disputes>> GetAllLearnerDisputesAsync(string userId)
        {
            return await _unitOfWork.Review.GetAllLearnerDisputesAsync(userId);
        }
        public async Task<Reviews> Create(object data) {
            var newReview = new Reviews { 
                RevieweeId = (string)data.GetProp("revieweeId"),
                SubmissionId = (int)data.GetProp("submissionId"),
                Status = "Submitted",
                LastActivityDate = DateTime.UtcNow
            };
            return await _unitOfWork.Review.Create(newReview);
        }
        public async Task<List<ReviewRequestModel>> GetReviewRequestModel()
        {
            return await _unitOfWork.Review.GetReviewRequestModel();
        }
        public async Task<Raters> ReAssignReviewRequest(int requestId, int raterId)
        {
            var rater = await _unitOfWork.Review.ReAssignReviewRequest(requestId, raterId);
            if (rater != null)
            {
                string url = $"{configuration["ClientUrl"]}/review/pro/" + requestId;
                // Send mail to rater with confirm link
                await mailService.SendEmailAsync(rater.User.Email, "Yêu Cầu Chấm Bài Mới Từ Reboost", $"Hi {rater.User.FirstName},<p>Bạn đã nhận được 1 yêu cầu chấm bài mới từ học viên của Reboost.</p> <p>Hãy hoàn thành bài chấm trong vòng 24h sử dụng đường link sau đây: <a href='{url}'>link chấm bài</a></p><p>Nếu bạn có thắc mắc gì xin vui lòng liên hệ trực tiếp với chúng tôi qua luồng email này.</p><p>Cảm ơn bạn,</p><p>Reboost Support</p>");
            }
            return rater;
        }
        public async Task<Reviews> RecordPayment(int reviewId)
        {
            return await _unitOfWork.Review.RecordPayment(reviewId);
        }
    }
}
