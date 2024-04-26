using System;
namespace Reboost.Shared
{
    public class CriteriaFeedbackModel
    {
        public string task { get; set; }
        public string topic { get; set; }
        public string chartName { get; set; }
        public string chartDescription { get; set; }
        public string essay { get; set; }
        public string criteriaName { get; set; }
        public string feedbackLanguage { get; set; }
        public decimal grammarScore { get; set; }
    }

    public class ErrorFeedbackModel
    {
        public string essay { get; set; }
        public string feedbackLanguage { get; set; }
    }
}

