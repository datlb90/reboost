using System;
namespace Reboost.Shared
{
    public class FeedbackRequestModel
    {
        public string userId { get; set; }
        public int questionId { get; set; }
        public int reviewId { get; set; }
        public string topic { get; set; }
        public string essay { get; set; }
        public string task { get; set; }
        public bool hasGrade { get; set; }
        public string chartFileName { get; set; }
        public string feedbackLanguage { get; set; }
        public string chartDescription { get; set; }
    }
}

