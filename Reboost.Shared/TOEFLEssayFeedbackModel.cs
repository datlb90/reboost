using System;
using System.Collections.Generic;

namespace Reboost.Shared
{
    public class ImageToTopicAndEssayModel
    {
        public string topic { get; set; }
        public string essay { get; set; }
    }

    public class TOEFLEssayFeedbackModel
    {
        public string useOflanguge { get; set; }
        public string coherenceAccuracy { get; set; }
        public string developmentOrganization { get; set; }
        public string[] errors { get; set; }
        public float score { get; set; }
        public string overallFeedback { get; set; }
    }

    public class TOEFLIndependentFeedbackModel
    {
        public AutomatedFeedbackModel languageUse { get; set; }
        public AutomatedFeedbackModel organization { get; set; }
        public AutomatedFeedbackModel developmentSupport { get; set; }
        public AutomatedFeedbackModel overallFeedback { get; set; }
        public List<AutomatedFeedbackError> errors { get; set; }
    }

    public class TOEFLIntegratedFeedbackModel
    {
        public AutomatedFeedbackModel languageUse { get; set; }
        public AutomatedFeedbackModel organization { get; set; }
        public AutomatedFeedbackModel content { get; set; }
        public AutomatedFeedbackModel overallFeedback { get; set; }
        public List<AutomatedFeedbackError> errors { get; set; }
    }

    //public class NewTOEFLEssayFeedbackModel
    //{
    //    public AutomatedFeedbackModel langUse { get; set; }
    //    public AutomatedFeedbackModel organization { get; set; }
    //    public AutomatedFeedbackModel devSupport { get; set; }
    //    public AutomatedFeedbackModel overallFeedback { get; set; }
    //    public List<AutomatedFeedbackError> errors { get; set; }
    //}

    public class FeedbackModel
    {
        public string performance { get; set; }
        public string strengths { get; set; }
        public string weaknesses { get; set; }
        public string recommendations { get; set; }
    }

    public class AutomatedFeedbackModel
    {
        public FeedbackModel feedback { get; set; }
        public float score { get; set; }
        public string comment { get; set; }
        public decimal bandScore { get; set; }
    }

    public class AutomatedFeedbackError
    {
        public string issue { get; set; }
        public string type { get; set; }
        public string fix { get; set; }
    }

    public class ErrorsInText
    {
        public List<ErrorInText> errors { get; set; }
       
    }

    public class ErrorInText
    {
        public string error { get; set; }
        public int position { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string fix { get; set; }
        public string reason { get; set; }
        public string comment { get; set; }
    }

    public class EssayScoreModel
    {
        public decimal taskAchievementScore { get; set; }
        public decimal taskResponseScore { get; set; }
        public decimal coherenceScore { get; set; }
        public decimal lexicalResourceScore { get; set; }
        public decimal grammarScore { get; set; }
        public decimal overallScore { get; set; }
    }
}

