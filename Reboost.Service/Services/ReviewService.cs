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
using static System.Net.Mime.MediaTypeNames;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Reboost.Service.Services
{
    public interface IReviewService
    {
        // Start version 2.0
        Task<ErrorsInText> getIntextCommentsV2(CriteriaFeedbackModel model);
        Task<EssayFeedbackV2> getCriteriaFeedbackFreeV2(EssayFeedbackModel model);
        Task<EssayFeedbackV2> GetArgumentFeedbackV2(EssayFeedbackModel model);
        Task<List<CriteriaFeedback>> GetCriteriaFeedbackV2(User user, FeedbackRequestModel model);
        Task<ErrorsInText> getIntextFeedbackV2(User user, CriteriaFeedbackModel model);
        // End version 2.0

        Task<EssayFeedback> getCriteriaFeedbackFree1(EssayFeedbackModel model);
        Task<ErrorsInText> getIntextCommentsFree(CriteriaFeedbackModel model);
        Task<ReviewScores> getEssayScoreFree(CriteriaFeedbackModel model);
        Task<EssayFeedback> getCriteriaFeedbackFree(EssayFeedbackModel model);
        Task<InitialSubmissionModel> GetInitialSubmission(string userId);

        Task<EssayFeedback> getCriteriaFeedbackGPT4O(EssayFeedbackModel model);
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
        Task<GetReviewsModel> CreateAutomatedReview(string userId, int submissionId, string feedbackLanguage, string reviewType);
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

        public async Task<List<CriteriaFeedback>> GetCriteriaFeedbackV2(User user, FeedbackRequestModel model)
        {
            if (model.hasGrade)
            {
                // If the review already has grade, return the feedback from db
                return await _unitOfWork.Review.GetCriteriaFeedback(model.reviewId);
            }
            else
            {
                var userSubscription = await subscriptionService.GetUserSubscription(user.Id);
                if (userSubscription != null || (model.feedbackType == "detail" && user.FreeToken > 0) || (model.feedbackType == "deep" && user.PremiumToken > 0))
                {
                    // If there is no grade, get the feedback from chatGPT, save the feedback in database, then return the feedback
                    List<CriteriaFeedback> result = new List<CriteriaFeedback>(); // display feedback result
                    try
                    {
                        // 1. Get the rubric for the question
                        var rubricCriteria = await _unitOfWork.Rubrics.GetFeedbackRubric(model.questionId, model.task);
                        var taskList = new Task<EssayFeedbackV2>[rubricCriteria.Count];

                        // 2. Get feedback from chatGPT
                        for (int i = 0; i < rubricCriteria.Count; i++)
                        {
                            if (rubricCriteria[i].name == "Arguments Assessment")
                            {
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

                                taskList[i] = GetArgumentFeedbackV2(requestModel);
                            }
                            else
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

                                taskList[i] = chatGPTService.getCriteriaFeedbackFreeV2(requestModel);
                            }
                            
                        }

                        EssayFeedbackV2[] completedTasks = await Task.WhenAll(taskList);

                        //var scoreResults = completedTasks[0];
                        foreach (EssayFeedbackV2 completedTask in completedTasks)
                        {
                            if(completedTask != null)
                            {
                                CriteriaFeedback criteriaFeedback = new CriteriaFeedback
                                {
                                    criteriaId = completedTask.criteriaId,
                                    name = completedTask.criteriaName,
                                    comment = "",
                                    mark = 0
                                };
                                // Generate html connent for the comment
                                switch (completedTask.criteriaName)
                                {
                                    case "Improved Version":
                                        criteriaFeedback.comment = completedTask.improvedVersion;
                                        break;
                                    case "Vocabulary":
                                        criteriaFeedback.comment = completedTask.vocabulary;
                                        break;
                                    case "Arguments Assessment":
                                        string htmlContent = "<div>";
                                        int start = 1;
                                        foreach (ArgumentFeedback feedback in completedTask.argumentFeedback)
                                        {
                                            htmlContent += "<p><strong>" + start + ". Quoted Text: </strong>\"" + feedback.paragraphText + "\"</p>";
                                            if (model.feedbackLanguage == "vn")
                                                htmlContent += "<p><strong>- Đánh Giá Chi Tiết: </strong>\"" + feedback.assessment + "\"</p>";
                                            else
                                                htmlContent += "<p><strong>- Detailed Assessment: </strong>\"" + feedback.assessment + "\"</p>";

                                            if (model.feedbackLanguage == "vn")
                                                htmlContent += "<p><strong>- Gợi Ý Cải Thiện: </strong>" + feedback.howToImprove + "\"</p>";
                                            else
                                                htmlContent += "<p><strong>- How To Improve: </strong>" + feedback.howToImprove + "\"</p>";

                                            if (model.feedbackLanguage == "vn")
                                                htmlContent += "<p><strong>- Phiên Bản Cải Thiện: </strong>\"" + feedback.improvedVersion + "\"</p>";
                                            else
                                                htmlContent += "<p><strong>- Improved Version: </strong>\"" + feedback.improvedVersion + "\"</p>";
                                            htmlContent += "<hr>";
                                            start++;
                                        }
                                        htmlContent += "</div>";
                                        criteriaFeedback.comment = htmlContent;

                                        break;
                                    case "Task Achievement":
                                        string taskAchivementHtml = "<ul>";
                                        // Fulfilling the Prompt Requirements
                                        if (model.feedbackLanguage == "vn")
                                            taskAchivementHtml += "<li><p><strong>Đáp Ứng Các Yêu Cầu Của Đề Bài:</strong></p>";
                                        else
                                            taskAchivementHtml += "<li><p><strong>Fulfilling the Prompt Requirements:</strong></p>";
                                        taskAchivementHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            taskAchivementHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.fulfillRequirements.assessment + "</p>";
                                        else
                                            taskAchivementHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.fulfillRequirements.assessment + "</p>";

                                        taskAchivementHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            taskAchivementHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.fulfillRequirements.howToImprove + "</p>";
                                        else
                                            taskAchivementHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.fulfillRequirements.howToImprove + "</p>";
                                        taskAchivementHtml += "</li></ul></li>";

                                        // Highlighting Key Data & Features
                                        if (model.feedbackLanguage == "vn")
                                            taskAchivementHtml += "<li><p><strong>Nêu Bật Các Dữ Liệu Và Tính Năng Chính:</strong></p>";
                                        else
                                            taskAchivementHtml += "<li><p><strong>Highlighting Key Features & Data:</strong></p>";
                                        taskAchivementHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            taskAchivementHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.highlightKeyFeatures.assessment + "</p>";
                                        else
                                            taskAchivementHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.highlightKeyFeatures.assessment + "</p>";

                                        taskAchivementHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            taskAchivementHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.highlightKeyFeatures.howToImprove + "</p>";
                                        else
                                            taskAchivementHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.highlightKeyFeatures.howToImprove + "</p>";
                                        taskAchivementHtml += "</li></ul></li>";

                                        // Comparing and Contrasting Data
                                        if (model.feedbackLanguage == "vn")
                                            taskAchivementHtml += "<li><p><strong>So Sánh Và Đối Chiếu Dữ Liệu:</strong></p>";
                                        else
                                            taskAchivementHtml += "<li><p><strong>Comparing and Contrasting Data:</strong></p>";
                                        taskAchivementHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            taskAchivementHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.compareAndContrast.assessment + "</p>";
                                        else
                                            taskAchivementHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.compareAndContrast.assessment + "</p>";

                                        taskAchivementHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            taskAchivementHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.compareAndContrast.howToImprove + "</p>";
                                        else
                                            taskAchivementHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.compareAndContrast.howToImprove + "</p>";
                                        taskAchivementHtml += "</li></ul></li>";
                                        // end
                                        taskAchivementHtml += "</ul>";

                                        criteriaFeedback.comment = taskAchivementHtml;
                                        criteriaFeedback.mark = completedTask.criteriaFeedback.score;
                                        break;
                                    case "Task Response":
                                        string tasResponseHtml = "<ul>";
                                        // Addressing All Parts of the Question
                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<li><p><strong>Trả Lời Tất Cả Các Phần Của Câu Hỏi:</strong></p>";
                                        else
                                            tasResponseHtml += "<li><p><strong>Addressing All Parts of the Question:</strong></p>";
                                        tasResponseHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.answeringAllParts.assessment + "</p>";
                                        else
                                            tasResponseHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.answeringAllParts.assessment + "</p>";

                                        tasResponseHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.answeringAllParts.howToImprove + "</p>";
                                        else
                                            tasResponseHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.answeringAllParts.howToImprove + "</p>";
                                        tasResponseHtml += "</li></ul></li>";

                                        // Clarity of Position
                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<li><p><strong>Trình Bày Quan Điểm Rõ Ràng Suốt Bài:</strong></p>";
                                        else
                                            tasResponseHtml += "<li><p><strong>Clarity of Position:</strong></p>";
                                        tasResponseHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.clarityOfPosition.assessment + "</p>";
                                        else
                                            tasResponseHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.clarityOfPosition.assessment + "</p>";

                                        tasResponseHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.clarityOfPosition.howToImprove + "</p>";
                                        else
                                            tasResponseHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.clarityOfPosition.howToImprove + "</p>";
                                        tasResponseHtml += "</li></ul></li>";

                                        // Development of Ideas
                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<li><p><strong>Trình Bày Và Phát Triển Ý Tưởng:</strong></p>";
                                        else
                                            tasResponseHtml += "<li><p><strong>Development of Ideas:</strong></p>";
                                        tasResponseHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.developmentOfIdeas.assessment + "</p>";
                                        else
                                            tasResponseHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.developmentOfIdeas.assessment + "</p>";

                                        tasResponseHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.developmentOfIdeas.howToImprove + "</p>";
                                        else
                                            tasResponseHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.developmentOfIdeas.howToImprove + "</p>";
                                        tasResponseHtml += "</li></ul></li>";

                                        // Justification of Opinions
                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<li><p><strong>Tính Thuyết Phục Trong Quan Điểm:</strong></p>";
                                        else
                                            tasResponseHtml += "<li><p><strong>Justification of Opinions:</strong></p>";
                                        tasResponseHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.justificationOfOpinion.assessment + "</p>";
                                        else
                                            tasResponseHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.justificationOfOpinion.assessment + "</p>";

                                        tasResponseHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            tasResponseHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.justificationOfOpinion.howToImprove + "</p>";
                                        else
                                            tasResponseHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.justificationOfOpinion.howToImprove + "</p>";
                                        tasResponseHtml += "</li></ul></li>";

                                        // end
                                        tasResponseHtml += "</ul>";

                                        criteriaFeedback.comment = tasResponseHtml;
                                        criteriaFeedback.mark = completedTask.criteriaFeedback.score;
                                        break;
                                    case "Coherence & Cohesion":
                                        string coherenceHtml = "<ul>";
                                        // Logical Organization
                                        if (model.feedbackLanguage == "vn")
                                            coherenceHtml += "<li><p><strong>Sắp Xếp Thông Tin Một Cách Hợp Lý:</strong></p>";
                                        else
                                            coherenceHtml += "<li><p><strong>Logical Organization:</strong></p>";
                                        coherenceHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            coherenceHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.logicalOrganization.assessment + "</p>";
                                        else
                                            coherenceHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.logicalOrganization.assessment + "</p>";

                                        coherenceHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            coherenceHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.logicalOrganization.howToImprove + "</p>";
                                        else
                                            coherenceHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.logicalOrganization.howToImprove + "</p>";
                                        coherenceHtml += "</li></ul></li>";

                                        // Paragraphing
                                        if (model.feedbackLanguage == "vn")
                                            coherenceHtml += "<li><p><strong>Sử Dụng Đoạn Văn:</strong></p>";
                                        else
                                            coherenceHtml += "<li><p><strong>Paragraphing:</strong></p>";
                                        coherenceHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            coherenceHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.paragraphing.assessment + "</p>";
                                        else
                                            coherenceHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.paragraphing.assessment + "</p>";

                                        coherenceHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            coherenceHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.paragraphing.howToImprove + "</p>";
                                        else
                                            coherenceHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.paragraphing.howToImprove + "</p>";
                                        coherenceHtml += "</li></ul></li>";

                                        // Use of Cohesive Devices
                                        if (model.feedbackLanguage == "vn")
                                            coherenceHtml += "<li><p><strong>Sử Dụng Đa Dạng Các Công Cụ Liên Kết:</strong></p>";
                                        else
                                            coherenceHtml += "<li><p><strong>Use of Cohesive Devices:</strong></p>";

                                        coherenceHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            coherenceHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.cohesiveDevices.assessment + "</p>";
                                        else
                                            coherenceHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.cohesiveDevices.assessment + "</p>";

                                        coherenceHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            coherenceHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.cohesiveDevices.howToImprove + "</p>";
                                        else
                                            coherenceHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.cohesiveDevices.howToImprove + "</p>";
                                        coherenceHtml += "</li></ul></li>";
                                        // end
                                        coherenceHtml += "</ul>";

                                        criteriaFeedback.comment = coherenceHtml;
                                        criteriaFeedback.mark = completedTask.criteriaFeedback.score;
                                        break;
                                    case "Lexical Resource":
                                        string lexicalHtml = "<ul>";
                                        // Logical Organization
                                        if (model.feedbackLanguage == "vn")
                                            lexicalHtml += "<li><p><strong>Sử Dụng Đa Dạng Từ Vựng:</strong></p>";
                                        else
                                            lexicalHtml += "<li><p><strong>Range of Vocabulary:</strong></p>";
                                        lexicalHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            lexicalHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.rangeOfVocabulary.assessment + "</p>";
                                        else
                                            lexicalHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.rangeOfVocabulary.assessment + "</p>";

                                        lexicalHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            lexicalHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.rangeOfVocabulary.howToImprove + "</p>";
                                        else
                                            lexicalHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.rangeOfVocabulary.howToImprove + "</p>";
                                        lexicalHtml += "</li></ul></li>";

                                        // Accuracy of Word Choice
                                        if (model.feedbackLanguage == "vn")
                                            lexicalHtml += "<li><p><strong>Sử Dụng Từ Vựng Chính Xác:</strong></p>";
                                        else
                                            lexicalHtml += "<li><p><strong>Accuracy of Word Choice:</strong></p>";
                                        lexicalHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            lexicalHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.accuracyOfWordChoice.assessment + "</p>";
                                        else
                                            lexicalHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.accuracyOfWordChoice.assessment + "</p>";

                                        lexicalHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            lexicalHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.accuracyOfWordChoice.howToImprove + "</p>";
                                        else
                                            lexicalHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.accuracyOfWordChoice.howToImprove + "</p>";
                                        lexicalHtml += "</li></ul></li>";

                                        // Spelling and Word Formation
                                        if (model.feedbackLanguage == "vn")
                                            lexicalHtml += "<li><p><strong>Chính Tả Và Cấu Trúc Từ:</strong></p>";
                                        else
                                            lexicalHtml += "<li><p><strong>Spelling and Word Formation:</strong></p>";

                                        lexicalHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            lexicalHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.spellingAndFormation.assessment + "</p>";
                                        else
                                            lexicalHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.spellingAndFormation.assessment + "</p>";

                                        lexicalHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            lexicalHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.spellingAndFormation.howToImprove + "</p>";
                                        else
                                            lexicalHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.spellingAndFormation.howToImprove + "</p>";
                                        lexicalHtml += "</li></ul></li>";
                                        // end
                                        lexicalHtml += "</ul>";

                                        criteriaFeedback.comment = lexicalHtml;
                                        criteriaFeedback.mark = completedTask.criteriaFeedback.score;
                                        break;
                                    case "Grammatical Range & Accuracy":
                                        string grammarHtml = "<ul>";
                                        // Logical Organization
                                        if (model.feedbackLanguage == "vn")
                                            grammarHtml += "<li><p><strong>Sử Dụng Đa Dạng Các Cấu Trúc Câu:</strong></p>";
                                        else
                                            grammarHtml += "<li><p><strong>Range of Grammatical Structures:</strong></p>";
                                        grammarHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            grammarHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.grammarRange.assessment + "</p>";
                                        else
                                            grammarHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.grammarRange.assessment + "</p>";

                                        grammarHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            grammarHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.grammarRange.howToImprove + "</p>";
                                        else
                                            grammarHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.grammarRange.howToImprove + "</p>";
                                        grammarHtml += "</li></ul></li>";

                                        // Accuracy of Word Choice
                                        if (model.feedbackLanguage == "vn")
                                            grammarHtml += "<li><p><strong>Sử Dụng Ngữ Pháp Chính Xác:</strong></p>";
                                        else
                                            grammarHtml += "<li><p><strong>Accuracy in Grammatical Forms:</strong></p>";
                                        grammarHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            grammarHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.grammarAccuracy.assessment + "</p>";
                                        else
                                            grammarHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.grammarAccuracy.assessment + "</p>";

                                        grammarHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            grammarHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.grammarAccuracy.howToImprove + "</p>";
                                        else
                                            grammarHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.grammarAccuracy.howToImprove + "</p>";
                                        grammarHtml += "</li></ul></li>";

                                        // Spelling and Word Formation
                                        if (model.feedbackLanguage == "vn")
                                            grammarHtml += "<li><p><strong>Sử Dụng Dấu Câu Đúng Cách:</strong></p>";
                                        else
                                            grammarHtml += "<li><p><strong>Punctuation:</strong></p>";

                                        grammarHtml += "<ul><li>";
                                        if (model.feedbackLanguage == "vn")
                                            grammarHtml += "<p><strong>Giải Thích Chi Tiết: </strong>" + completedTask.criteriaFeedback.punctuation.assessment + "</p>";
                                        else
                                            grammarHtml += "<p><strong>Detailed Explannation: </strong>" + completedTask.criteriaFeedback.punctuation.assessment + "</p>";

                                        grammarHtml += "</li><li>";

                                        if (model.feedbackLanguage == "vn")
                                            grammarHtml += "<p><strong>Gợi Ý Cải Thiện: </strong>" + completedTask.criteriaFeedback.punctuation.howToImprove + "</p>";
                                        else
                                            grammarHtml += "<p><strong>How To Improve: </strong>" + completedTask.criteriaFeedback.punctuation.howToImprove + "</p>";
                                        grammarHtml += "</li></ul></li>";
                                        // end
                                        grammarHtml += "</ul>";

                                        criteriaFeedback.comment = grammarHtml;
                                        criteriaFeedback.mark = completedTask.criteriaFeedback.score;
                                        break;
                                    default:
                                        break;
                                }
                                result.Add(criteriaFeedback);
                            }
                        }

                        // Update feedback token
                        if (userSubscription == null)
                        {
                            if (model.feedbackType == "detail" && user.FreeToken > 0)
                            {
                                user.FreeToken = user.FreeToken - 1;
                            }
                            else if (model.feedbackType == "deep" && user.PremiumToken > 0)
                            {
                                user.PremiumToken = user.PremiumToken - 1;
                            }
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

        public async Task<ErrorsInText> getIntextFeedbackV2(User user, CriteriaFeedbackModel model)
        {
            var userSubscription = await subscriptionService.GetUserSubscription(user.Id);

            if (userSubscription != null || (model.feedbackType == "detail" && user.FreeToken > 0) || (model.feedbackType == "deep" && user.PremiumToken > 0))
            {
                if (userSubscription == null)
                {
                    return await chatGPTService.getIntextCommentsV2(model);
                }
                else
                {
                    if (userSubscription.PlanId >= 4)
                    {
                        return await getDeepIntextComments(model);
                    }
                    else
                    {
                        return await getDetailedIntextComments(model);
                    }
                }
            }

            return null;
        }

        public async Task<ErrorsInText> getDetailedIntextComments(CriteriaFeedbackModel model)
        {
            // Make a ChatGpt call for each paragraph
            ErrorsInText result = new ErrorsInText();
            if (!String.IsNullOrEmpty(model.essay))
            {
                result.errors = new List<ErrorInText>();
                model.essay = model.essay.Replace("\r", String.Empty);
                string[] paragraphs = model.essay.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (paragraphs.Length > 1)
                {
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

                        taskList[i] = chatGPTService.getIntextCommentsV2(requestModel);
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
                        }
                    }

                    return result;
                }
                else
                {
                    // If this is a 1-paragraph essay
                    // Regular expression to split on period, exclamation or question mark, followed by space or end of string.
                    // This regex handles cases where the punctuation is followed by a space or is at the end of the text.
                    string[] sentences = Regex.Split(model.essay, @"(?<=[\.!\?])\s+(?=(?:[A-Z]|$))");

                    if (sentences.Length >= 2)
                    {
                        var taskList = new Task<ErrorsInText>[2];
                        int median = sentences.Length / 2;
                        if (sentences.Length % 2 != 0) median += 1; // Optionally adjust how the median is calculated for odd numbers

                        string[] firstList = sentences.Take(median).ToArray();
                        string firstPart = String.Join(Environment.NewLine, firstList);

                        CriteriaFeedbackModel request1 = new CriteriaFeedbackModel
                        {
                            essay = firstPart,
                            task = model.task,
                            topic = model.topic,
                            feedbackLanguage = model.feedbackLanguage
                        };

                        taskList[0] = chatGPTService.getIntextCommentsV2(request1);

                        int remain = sentences.Length - median;
                        string[] secondList = sentences.Skip(median).Take(remain).ToArray();
                        string secondPart = String.Join(Environment.NewLine, secondList);


                        CriteriaFeedbackModel request2 = new CriteriaFeedbackModel
                        {
                            essay = secondPart,
                            task = model.task,
                            topic = model.topic,
                            feedbackLanguage = model.feedbackLanguage
                        };

                        taskList[1] = chatGPTService.getIntextCommentsV2(request2);

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
                            }
                        }

                        return result;

                    }
                    else
                    {
                        // Handle case with fewer than two sentences
                        CriteriaFeedbackModel request = new CriteriaFeedbackModel
                        {
                            essay = model.essay,
                            task = model.task,
                            topic = model.topic,
                            feedbackLanguage = model.feedbackLanguage
                        };

                        return await chatGPTService.getIntextCommentsV2(request);
                    }
                }
            }
            return result;
        }

        public async Task<ErrorsInText> getDeepIntextComments(CriteriaFeedbackModel model)
        {
            // Make a ChatGpt call for each sentence
            ErrorsInText result = new ErrorsInText();
            result.errors = new List<ErrorInText>();
            if (!String.IsNullOrEmpty(model.essay))
            {
                // If this is a 1-paragraph essay
                // Regular expression to split on period, exclamation or question mark, followed by space or end of string.
                // This regex handles cases where the punctuation is followed by a space or is at the end of the text.
                //string[] sentences = Regex.Split(model.essay, @"(?<=[\.!\?])\s+(?=(?:[A-Z]|$))");
                string[] sentences = Regex.Split(model.essay, @"(?<=[\.!\?])\s+(?=\S*(?:[A-Z]|$))");

                // Filtering out any empty or whitespace-only entries
                sentences = sentences.Where(sentence => !string.IsNullOrWhiteSpace(sentence)).ToArray();
                var taskList = new Task<ErrorsInText>[sentences.Length];
                for (int i = 0; i < sentences.Length; i++)
                {
                    CriteriaFeedbackModel requestModel = new CriteriaFeedbackModel
                    {
                        essay = sentences[i],
                        task = model.task,
                        topic = model.topic,
                        feedbackLanguage = model.feedbackLanguage
                    };

                    taskList[i] = chatGPTService.getIntextCommentsV2(requestModel);
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
                    }
                }
            }
            return result;
        }



        public async Task<ErrorsInText> getIntextCommentsV2(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getIntextCommentsV2(model);
        }

        public async Task<EssayFeedbackV2> GetArgumentFeedbackV2(EssayFeedbackModel model)
        {
            if (!String.IsNullOrEmpty(model.essay))
            {
                EssayFeedbackV2 result = new EssayFeedbackV2();
                result.argumentFeedback = new List<ArgumentFeedback>();
                result.criteriaName = model.criteriaName;
                result.criteriaId = model.criteriaId;
                model.essay = model.essay.Replace("\r", String.Empty);
                string[] paragraphs = model.essay.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                if(paragraphs.Length <= 0)
                {
                    return null;
                }
                else if (paragraphs.Length == 1)
                {
                    EssayFeedbackModel requestModel = new EssayFeedbackModel
                    {
                        essay = paragraphs[0],
                        task = model.task,
                        topic = model.topic,
                        feedbackLanguage = model.feedbackLanguage
                    };

                    ArgumentFeedback feedback = await chatGPTService.GetArgumentFeedbackV2(requestModel);
                    result.argumentFeedback.Add(feedback);
                    return result;
                }
                else if( paragraphs.Length == 2)
                {
                    var taskList = new Task<ArgumentFeedback>[paragraphs.Length];
                    for (int i = 0; i < paragraphs.Length; i++)
                    {
                        EssayFeedbackModel requestModel = new EssayFeedbackModel
                        {
                            essay = paragraphs[i],
                            task = model.task,
                            topic = model.topic,
                            feedbackLanguage = model.feedbackLanguage
                        };

                        taskList[i] = chatGPTService.GetArgumentFeedbackV2(requestModel);
                    }
                    var completedTasks = await Task.WhenAll(taskList);

                    foreach (ArgumentFeedback feedback in completedTasks)
                    {
                        result.argumentFeedback.Add(feedback);
                    }
                    return result;
                }
                else if (paragraphs.Length == 3)
                {
                    var taskList = new Task<ArgumentFeedback>[paragraphs.Length - 1];
                    for (int i = 1; i < paragraphs.Length; i++)
                    {
                        EssayFeedbackModel requestModel = new EssayFeedbackModel
                        {
                            essay = paragraphs[i],
                            task = model.task,
                            topic = model.topic,
                            feedbackLanguage = model.feedbackLanguage
                        };

                        taskList[i - 1] = chatGPTService.GetArgumentFeedbackV2(requestModel);
                    }
                    var completedTasks = await Task.WhenAll(taskList);

                    foreach (ArgumentFeedback feedback in completedTasks)
                    {
                        result.argumentFeedback.Add(feedback);
                    }
                    return result;
                }
                else // length >= 4
                {
                    var taskList = new Task<ArgumentFeedback>[paragraphs.Length - 2];
                    for (int i = 1; i < paragraphs.Length - 1; i++)
                    {
                        EssayFeedbackModel requestModel = new EssayFeedbackModel
                        {
                            essay = paragraphs[i],
                            task = model.task,
                            topic = model.topic,
                            feedbackLanguage = model.feedbackLanguage
                        };

                        taskList[i - 1] = chatGPTService.GetArgumentFeedbackV2(requestModel);
                    }
                    var completedTasks = await Task.WhenAll(taskList);

                    foreach (ArgumentFeedback feedback in completedTasks)
                    {
                        result.argumentFeedback.Add(feedback);
                    }

                    return result;
                }
            }
            return null;
        }

        public async Task<EssayFeedbackV2> getCriteriaFeedbackFreeV2(EssayFeedbackModel model)
        {
            return await chatGPTService.getCriteriaFeedbackFreeV2(model);
        }

        public async Task<ErrorsInText> getIntextCommentsFree(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getIntextCommentsFree(model);
        }

        public async Task<ReviewScores> getEssayScoreFree(CriteriaFeedbackModel model)
        {
            return await chatGPTService.getEssayScoreFree(model);
        }

        public async Task<EssayFeedback> getCriteriaFeedbackFree1(EssayFeedbackModel model)
        {
            return await chatGPTService.getCriteriaFeedbackFree1(model);
        }

        public async Task<EssayFeedback> getCriteriaFeedbackFree(EssayFeedbackModel model)
        {
            return await chatGPTService.getCriteriaFeedbackFree(model);
        }

        public async Task<InitialSubmissionModel> GetInitialSubmission(string userId)
        {
            return await _unitOfWork.Submission.GetInitialSubmission(userId);
        }

        public async Task<EssayFeedback> getCriteriaFeedbackGPT4O(EssayFeedbackModel model)
        {
            return await chatGPTService.getCriteriaFeedbackGPT4O(model);
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
                    if (userSubscription != null || user.FreeToken > 0 || user.PremiumToken > 0)
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
                if (userSubscription != null || user.FreeToken > 0 || user.PremiumToken > 0)
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
                        ReviewScores scores = await chatGPTService.getEssayScoreFree(scoreModel);
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
                if(userSubscription != null || (model.feedbackType == "detail" && user.FreeToken > 0) || (model.feedbackType == "deep" && user.PremiumToken > 0))
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

                            if (userSubscription == null)
                            {
                                taskList[i] = chatGPTService.getCriteriaFeedbackFree(requestModel);
                            }
                            else
                            {
                                if(userSubscription.PlanId >= 4)
                                {
                                    taskList[i] = chatGPTService.getCriteriaFeedbackGPT4O(requestModel);
                                }
                                else
                                {
                                    taskList[i] = chatGPTService.getCriteriaFeedbackGPTTurbo(requestModel);
                                }
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

                        // Update feedback token
                        if (userSubscription == null)
                        {
                            if (model.feedbackType == "detail" && user.FreeToken > 0)
                            {
                                user.FreeToken = user.FreeToken - 1;
                            }
                            else if (model.feedbackType == "deep" && user.PremiumToken > 0)
                            {
                                user.PremiumToken = user.PremiumToken - 1;
                            }
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

            if (userSubscription != null || (model.feedbackType == "detail" && user.FreeToken > 0) || (model.feedbackType == "deep" && user.PremiumToken > 0))
            {
                if (userSubscription == null)
                {
                    return await chatGPTService.getIntextCommentsFree(model);
                }
                else
                {
                    if (userSubscription.PlanId >= 4)
                    {
                        return await getIntextCommentsGPT4(model);
                    }
                    else
                    {
                        return await getIntextCommentsGPTTurbo(model);
                    }
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
            if (!String.IsNullOrEmpty(model.essay))
            {
                result.errors = new List<ErrorInText>();
                model.essay = model.essay.Replace("\r", String.Empty);
                string[] paragraphs = model.essay.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (paragraphs.Length > 1)
                {
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
                        }
                    }

                    return result;
                }
                else
                {
                    // If this is a 1-paragraph essay
                    // Regular expression to split on period, exclamation or question mark, followed by space or end of string.
                    // This regex handles cases where the punctuation is followed by a space or is at the end of the text.
                    string[] sentences = Regex.Split(model.essay, @"(?<=[\.!\?])\s+(?=(?:[A-Z]|$))");

                    if (sentences.Length >= 2)
                    {
                        var taskList = new Task<ErrorsInText>[2];
                        int median = sentences.Length / 2;
                        if (sentences.Length % 2 != 0) median += 1; // Optionally adjust how the median is calculated for odd numbers

                        string[] firstList = sentences.Take(median).ToArray();
                        string firstPart = String.Join(Environment.NewLine, firstList);

                        CriteriaFeedbackModel request1 = new CriteriaFeedbackModel
                        {
                            essay = firstPart,
                            task = model.task,
                            topic = model.topic,
                            feedbackLanguage = model.feedbackLanguage
                        };

                        taskList[0] = chatGPTService.getIntextCommentsGPTTurbo(request1);

                        int remain = sentences.Length - median;
                        string[] secondList = sentences.Skip(median).Take(remain).ToArray();
                        string secondPart = String.Join(Environment.NewLine, secondList);


                        CriteriaFeedbackModel request2 = new CriteriaFeedbackModel
                        {
                            essay = secondPart,
                            task = model.task,
                            topic = model.topic,
                            feedbackLanguage = model.feedbackLanguage
                        };

                        taskList[1] = chatGPTService.getIntextCommentsGPTTurbo(request2);

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
                            }
                        }

                        return result;

                    }
                    else
                    {
                        // Handle case with fewer than two sentences
                        CriteriaFeedbackModel request = new CriteriaFeedbackModel
                        {
                            essay = model.essay,
                            task = model.task,
                            topic = model.topic,
                            feedbackLanguage = model.feedbackLanguage
                        };

                        return await chatGPTService.getIntextCommentsGPTTurbo(request);
                    }
                }
            }
            return result;
        }

        public async Task<ErrorsInText> getIntextCommentsGPT4(CriteriaFeedbackModel model)
        {
            // Make a ChatGpt call for each sentence
            ErrorsInText result = new ErrorsInText();
            result.errors = new List<ErrorInText>();
            if (!String.IsNullOrEmpty(model.essay))
            {
                // If this is a 1-paragraph essay
                // Regular expression to split on period, exclamation or question mark, followed by space or end of string.
                // This regex handles cases where the punctuation is followed by a space or is at the end of the text.
                //string[] sentences = Regex.Split(model.essay, @"(?<=[\.!\?])\s+(?=(?:[A-Z]|$))");
                string[] sentences = Regex.Split(model.essay, @"(?<=[\.!\?])\s+(?=\S*(?:[A-Z]|$))");

                // Filtering out any empty or whitespace-only entries
                sentences = sentences.Where(sentence => !string.IsNullOrWhiteSpace(sentence)).ToArray();


                var taskList = new Task<ErrorsInText>[sentences.Length];
                for (int i = 0; i < sentences.Length; i++)
                {
                    CriteriaFeedbackModel requestModel = new CriteriaFeedbackModel
                    {
                        essay = sentences[i],
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
                    }
                }
            }
            return result;
        }


        //public async Task<ErrorsInText> getIntextCommentsGPT4(CriteriaFeedbackModel model)
        //{
        //    // Make a ChatGpt call for each paragraph
        //    ErrorsInText result = new ErrorsInText();
        //    result.errors = new List<ErrorInText>();
        //    model.essay = model.essay.Replace("\r", String.Empty);
        //    string[] paragraphs = model.essay.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        //    var taskList = new Task<ErrorsInText>[paragraphs.Length];
        //    for (int i = 0; i < paragraphs.Length; i++)
        //    {
        //        CriteriaFeedbackModel requestModel = new CriteriaFeedbackModel
        //        {
        //            essay = paragraphs[i],
        //            task = model.task,
        //            topic = model.topic,
        //            feedbackLanguage = model.feedbackLanguage
        //        };

        //        taskList[i] = chatGPTService.getIntextCommentsGPT4(requestModel);
        //    }

        //    var completedTasks = await Task.WhenAll(taskList);

        //    foreach (var completedTask in completedTasks)
        //    {
        //        if (completedTask != null && completedTask.errors != null && completedTask.errors.Count > 0)
        //        {
        //            foreach (ErrorInText error in completedTask.errors)
        //            {
        //                if (!result.errors.Any(e => e.error == error.error))
        //                {
        //                    result.errors.Add(error);
        //                }
        //            }
        //            //result.errors = result.errors.Concat(completedTask.errors).ToList();
        //        }
        //    }

        //    return result;
        //}


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

        public async Task<GetReviewsModel> CreateAutomatedReview(string userId, int submissionId, string feedbackLanguage = "vn", string reviewType = "detail")
        {
            Submissions submission = await _unitOfWork.Submission.GetByIdAsync(submissionId);
            if(submission.Type == "initial.vn" || submission.Type == "initial.en")
            {
                // Clear initial submission for this student.
                submission.Type = "Submission";
                submission.UpdatedDate = DateTime.UtcNow;
                await _unitOfWork.Submission.Update(submission);
            }

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
                ReviewType = reviewType
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
