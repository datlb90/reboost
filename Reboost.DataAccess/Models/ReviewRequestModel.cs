using System;
namespace Reboost.DataAccess.Models
{
    public class ReviewRequestModel
    {
        public int RequestId { get; set; }
        public string LearnerId { get; set; }
        public string LearnerName { get; set; }
        public string RaterId { get; set; }
        public string RaterName { get; set; }
        public string RequestStatus { get; set; }
        public string AssignmentStatus { get; set; }
        public string ReviewStatus { get; set; }
        public string SubmissionStatus { get; set; }
        public int SubmissionId { get; set; }
        public int QuestionId { get; set; }
        public int DocId { get; set; }
        public int ReviewId { get; set; }
        public string PaypalTransactionId { get; set; }
        public string RaterPaypalAccount { get; set; }
        public DateTime RequestedDateTime { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime? LastReviewActivityDate { get; set; }
        public DateTime? CompletedDatetime { get; set; }
    }
}

