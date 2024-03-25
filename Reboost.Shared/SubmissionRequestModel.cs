using System;
namespace Reboost.Shared
{
    public class SubmissionRequestModel
    {
        public string feedbackType { get; set; }
        public bool hasReview { get; set; }
        public int reviewId { get; set; }
    }
}

