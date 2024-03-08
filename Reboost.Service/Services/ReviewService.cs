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

namespace Reboost.Service.Services
{
    public interface IReviewService
    {
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
        IMailService mailService;
        //IPaymentService paymentService;
        IUserService userService;
        IConfiguration configuration;
        IChatGPTService chatGPTService;
        public ReviewService(IUnitOfWork unitOfWork,
            IMailService _mailService,
            //IPaymentService _paymentService,
            IUserService _userService,
            IChatGPTService _chatGPTService,
            IConfiguration _configuration) : base(unitOfWork)
        {
            mailService = _mailService;
            //paymentService = _paymentService;
            userService = _userService;
            chatGPTService = _chatGPTService;
            configuration = _configuration;
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
        public async Task<GetReviewsModel> CreateAutomatedReview(string userId, int submissionId, string feedbackLanguage = "vn")
        {
            Submissions submission = await _unitOfWork.Submission.GetByIdAsync(submissionId);
            QuestionModel question = await _unitOfWork.Questions.GetByIdAsync(submission.QuestionId);
            Documents document = await _unitOfWork.Documents.GetByIdAsync(submission.DocId);
            var questionContent = StripHTML(question.QuestionsPart.Find(p => p.Name == "Question").Content);
            NewTOEFLEssayFeedbackModel toeflFeedback = new NewTOEFLEssayFeedbackModel();
            NewIELTSEssayFeedbackModel ieltsFeedback = new NewIELTSEssayFeedbackModel();
            if (question.Section == "Independent Writing" || question.Section == "Integrated Writing")
            {
                toeflFeedback = await chatGPTService.GetTOEFLIndependentEssayFeedback(questionContent, document.Text, feedbackLanguage);
            }
            else if (question.Section == "Integrated Writing")
            {
                toeflFeedback = await chatGPTService.GetTOEFLIntegratedEssayFeedback(questionContent, document.Text, feedbackLanguage);
            }
            else
            {
                ieltsFeedback = await chatGPTService.GetIELTSEssayFeedback(questionContent, document.Text, feedbackLanguage);
            }
            var rubrics = await _unitOfWork.Rubrics.GetByQuestionId(question.Id);
            List<ReviewData> reviewDataList = new List<ReviewData>();
            decimal? finalScore = null;
            foreach (RubricsModel criteria in rubrics)
            {
                string shouldbe = "nên được thay bằng";
                if(feedbackLanguage == "vn")
                {
                    shouldbe = "should be";
                }
                    
                ReviewData reviewData = new ReviewData();
                reviewData.CriteriaId = (int)criteria.Id;
                if (question.Test == "TOEFL")
                {
                    switch (criteria.Name)
                    {
                        case "Use of Language":
                            reviewData.Comment = toeflFeedback.useOflanguge.comment;
                            reviewData.Score = (decimal)Math.Floor(toeflFeedback.useOflanguge.score);
                            break;
                        case "Coherence & Accuracy":
                            reviewData.Comment = toeflFeedback.coherenceAccuracy.comment;
                            reviewData.Score = (decimal)Math.Floor(toeflFeedback.coherenceAccuracy.score);
                            break;
                        case "Development & Organization":
                            reviewData.Comment = toeflFeedback.developmentOrganization.comment;
                            reviewData.Score = (decimal)Math.Floor(toeflFeedback.developmentOrganization.score);
                            break;
                        case "Critical Errors":
                            string errors = "";
                            foreach(AutomatedFeedbackError error in toeflFeedback.errors)
                            {
                                if(!String.IsNullOrEmpty(error.issue))
                                    errors += "- '" + error.issue + "'";
                                if (!String.IsNullOrEmpty(error.fix))
                                    errors += " " + shouldbe + " '" + error.fix + "'";
                                if (!String.IsNullOrEmpty(error.type))
                                    errors += " (" + error.type + " )";
                                errors += Environment.NewLine;
                            }
                            reviewData.Comment = errors;
                            reviewData.Score = 0;
                            break;
                        case "Overall Score & Feedback":
                            reviewData.Comment = toeflFeedback.overallFeedback.comment;
                            reviewData.Score = (decimal)toeflFeedback.overallFeedback.score;
                            finalScore = reviewData.Score;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (criteria.Name)
                    {
                        case "Task Achievement":
                            reviewData.Comment = ieltsFeedback.taskAchievement.comment;
                            reviewData.Score = (decimal)Math.Floor(ieltsFeedback.taskAchievement.score);
                            break;
                        case "Coherence & Cohesion":
                            reviewData.Comment = ieltsFeedback.coherence.comment;
                            reviewData.Score = (decimal)Math.Floor(ieltsFeedback.coherence.score);
                            break;
                        case "Lexical Resource":
                            reviewData.Comment = ieltsFeedback.lexicalResource.comment;
                            reviewData.Score = (decimal)Math.Floor(ieltsFeedback.lexicalResource.score);
                            break;
                        case "Grammatical Range & Accuracy":
                            reviewData.Comment = ieltsFeedback.grammar.comment;
                            reviewData.Score = (decimal)Math.Floor(ieltsFeedback.grammar.score);
                            break;
                        case "Critical Errors":
                            string errors = "";
                            foreach (AutomatedFeedbackError error in ieltsFeedback.errors)
                            {
                                if (!String.IsNullOrEmpty(error.issue))
                                    errors += "- '" + error.issue + "'";
                                if (!String.IsNullOrEmpty(error.fix))
                                    errors += " "+ shouldbe +" '" + error.fix + "'";
                                if (!String.IsNullOrEmpty(error.type))
                                    errors += " (" + error.type + " )";
                                errors += Environment.NewLine;
                            }
                            reviewData.Comment = errors;
                            reviewData.Score = 0;
                            break;
                        case "Overall Score & Feedback":
                            reviewData.Comment = ieltsFeedback.overallFeedback.comment;
                            reviewData.Score = (decimal)ieltsFeedback.overallFeedback.score;
                            finalScore = reviewData.Score;
                            break;
                        default:
                            break;
                    }
                }

                reviewDataList.Add(reviewData);
            }

            Reviews review = new Reviews
            {
                RequestId = 0,
                ReviewerId = "AI",
                RevieweeId = userId,
                SubmissionId = submissionId,
                FinalScore = finalScore,
                Status = ReviewStatus.COMPLETED,
                TimeSpentInSeconds = 0,
                LastActivityDate = DateTime.UtcNow,
                ReviewData = reviewDataList,
            };

            // Update the submission status
            submission.Status = SubmissionStatus.COMPLETED;
            submission.UpdatedDate = DateTime.UtcNow;
            await _unitOfWork.Submission.Update(submission);

            // Create the review with review data
            var newReview =  await _unitOfWork.Review.Create(review);
            GetReviewsModel result = new GetReviewsModel();
            result.QuestionId = submission.QuestionId;
            result.Review = newReview;
            result.ReviewId = newReview.Id;
            result.DocId = document.Id;
            return result;
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
        public async Task<Reviews> SaveFeedback(int reviewId, List<ReviewData> data)
        { 
            var rs = await _unitOfWork.Review.SaveFeedback(reviewId, data);

            if(rs == null) 
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
