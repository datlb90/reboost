using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.Shared
{
    public static class RaterStatus
    {
        public const string APPROVED = "Approved";
        public const string COMPLETED = "Completed";
        public const string TRAINING = "Training";
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
    }
}
