using System;
using Reboost.DataAccess.Entities;

namespace Reboost.Service.ZaloPay
{
    public class VerifyPaymentModel
    {
        public int orderId { get; set; }
        public OrderStatus status { get; set; }
        public Orders order { get; set; }
        public Subscriptions subscription { get; set; }
    }

    public class ReviewFeedbackModel
    {
        public int questionId { get; set; }
        public int docId { get; set; }
        public int reviewId { get; set; }
        public int submissionId { get; set; }
    }

    public enum OrderStatus
    {
        ERROR = 0,
        COMPLETED = 1,
        PROCESSED = 2,
        REFUNED = 3
    }
}

