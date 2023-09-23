using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.Shared
{
    public static class RaterStatus
    {
        public const string APPLIED = "Applied";
        public const string DOCUMENT_REQUESTED = "Document Requested";
        public const string DOCUMENT_SUBMITTED = "Document Submitted";
        public const string APPROVED = "Final Approval";
        public const string COMPLETED = "Completed";
        public const string TRAINING = "Approved for Training";
        public const string TRAINING_COMPLETED = "Training Completed";
        public const string REVISION_REQUESTED = "Revision Requested";
        public const string REVISION = "Revision";
    }
    public static class SubmissionStatus {
        public const string PENDING = "Pending";
        public const string SAVED = "Saved";
        public const string SUBMITTED = "Submitted";
        public const string REVIEW_REQUESTED = "Review Requested";
        public const string REVIEWED = "Reviewed";
        public const string COMPLETED = "Completed";
        public const string DISPUTE_REQUESTED = "Dispute Requested";
    }
    public static class ReviewRequestStatus
    {
        public const string COMPLETED = "Completed";
        public const string REVIEW_REQUESTED = "Review Requested";
        public const string RATED = "Rated";
        public const string WAITING = "Waiting";
    }
    public static class TestsName
    {
        public const string IELTS = "IELTS";
        public const string TOEFL = "TOEFL";
    }
    public static class RaterTrainingStatus
    {
        public const string APPROVED = "Approved";
        public const string IN_PROGRESS = "In Progress";
        public const string COMPLETED = "Completed";
        public const string REVISION_REQUESTED = "Revision Requested";
        public const string REVISION_COMPLETED = "Revision Completed";
    }
    public static class ReviewStatus
    {
        public const string IN_PROGRESS = "In Progress";
        public const string COMPLETED = "Completed";
        public const string RATED = "Rated";
        public const string DISPUTED = "Disputed";
    }
    public static class ReviewRequestType
    {
        public const string FREE = "Free";
        public const string PRO = "Pro";
    }
    public static class RequestAssignmentStatus
    {
        public const string ASSIGNED = "Assigned";
        public const string ACCEPTED = "Accepted";
    }
    public static class DisputeStatus
    {
        public const string OPEN = "Open";
        public const string ACCEPTED = "Accepted";
        public const string DENIED = "Denied";
        public const string REFUNDED = "Refunded";
    }
    public static class QuestionStatus
    {
        public const string ACTIVE = "Active";
        public const string CONTRIBUTED = "Contributed";
        public const string APPROVED = "Approved";
    }
    public static class RaterBalanceStatus
    {
        public const string PENDING = "Pending";
        public const string DISPUTING = "Disputing";
        public const string AVAILABLE = "Available";
        public const string REFUND = "Refund";
        public const string PAID = "Paid";
    }
    public static class PaidAmount 
    {
        public const double VALUE = 1.0;
    }

    public static class SampleStatus
    {
        public const string CONTRIBUTED = "Contributed";
        public const string APPROVED = "Approved";
    }
}
