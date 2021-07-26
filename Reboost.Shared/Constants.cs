using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.Shared
{
    public static class RaterStatus
    {
        public const string APPROVED = "Final Approval";
        public const string COMPLETED = "Completed";
        public const string TRAINING = "Training";
        public const string TRAININGCOMPLETED = "TrainingCompleted";
        public const string REVISIONREQUESTED = "RevisionRequested";
        public const string REVISIONCOMPLETED = "RevisionCompleted";
        public const string REVISION = "Revision";
    }
    public static class SubmissionStatus {
        public const string PENDING = "Pending";
        public const string SUBMITTED = "Submitted";
        public const string REVIEW_REQUESTED = "Review Requested";
        public const string REVIEWED = "Reviewed";
        public const string COMPLETED = "Completed";
    }
    public static class ReviewRequestStatus
    {
        public const string COMPLETED = "Completed";
        public const string REVIEW_REQUETED = "Review Requested";
        public const string RATED = "Rated";
        public const string WAITING = "Waiting";
    }
    public static class TestsName
    {
        public const string IELTS = "IELTS";
        public const string TOEFL = "TOEFL";
    }
    public static class RaterReviewsTrainingStatus
    {
        public const string Submitted = "Submitted";
        public const string REVISION = "Revision";
    }
    public static class ReviewStatus
    {
        public const string IN_PROGRESS = "In Progress";
        public const string COMPLETED = "Completed";
    }
    public static class ReviewRequestType
    {
        public const string FREE = "Free";
        public const string PRO = "Pro";
    }
}
