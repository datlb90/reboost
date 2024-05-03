using System;
namespace Reboost.Shared
{
    public class ExperimentFeedbackModel
    {
        public string task { get; set; }
        public string topic { get; set; }
        public string chartName { get; set; }
        public string chartDescription { get; set; }
        public string essay { get; set; }
        public string criteriaName { get; set; }
        public string feedbackLanguage { get; set; }
        public string prompt { get; set; }
    }

    public class CriteriaFeedbackModel
    {
        public string task { get; set; }
        public string topic { get; set; }
        public string chartName { get; set; }
        public string chartDescription { get; set; }
        public string essay { get; set; }
        public string criteriaName { get; set; }
        public string feedbackLanguage { get; set; }
    }

    public class EssayFeedbackModel
    {
        // criteria feedback
        public string task { get; set; }
        public string topic { get; set; }
        public string chartName { get; set; }
        public string chartDescription { get; set; }
        public string essay { get; set; }
        public string criteriaName { get; set; }
        public string feedbackLanguage { get; set; }

        // criteria model
        public int criteriaId { get; set; }
        public int order { get; set; }
    }

    public class EssayFeedback
    {
        public string criteriaName { get; set; }
        public int criteriaId { get; set; }
        public decimal mark { get; set; }
        public string comment { get; set; }
        public int order { get; set; }
        // essay score
        public decimal taskAchievementScore { get; set; }
        public decimal taskResponseScore { get; set; }
        public decimal coherenceScore { get; set; }
        public decimal lexicalResourceScore { get; set; }
        public decimal grammarScore { get; set; }
        public decimal overallScore { get; set; }
    }

    public class ErrorFeedbackModel
    {
        public string essay { get; set; }
        public string feedbackLanguage { get; set; }
    }
}

