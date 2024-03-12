using System;
using System.Collections.Generic;

namespace Reboost.Shared
{
    public class IELTSEssayFeedbackModel
    {
        public string taskAchievement { get; set; }
        public string coherence { get; set; }
        public string lexicalResource { get; set; }
        public string grammar { get; set; }
        public string[] errors { get; set; }
        public float score { get; set; }
        public string overallFeedback { get; set; }
    }

    public class IELTSTask1FeedbackModel
    {
        public AutomatedFeedbackModel taskAchievement { get; set; }
        public AutomatedFeedbackModel coherence { get; set; }
        public AutomatedFeedbackModel lexicalResource { get; set; }
        public AutomatedFeedbackModel grammar { get; set; }
        public List<AutomatedFeedbackError> errors { get; set; }
        public AutomatedFeedbackModel overallFeedback { get; set; }
    }

    public class IELTSTask2FeedbackModel
    {
        public AutomatedFeedbackModel taskResponse { get; set; }
        public AutomatedFeedbackModel coherence { get; set; }
        public AutomatedFeedbackModel lexicalResource { get; set; }
        public AutomatedFeedbackModel grammar { get; set; }
        public List<AutomatedFeedbackError> errors { get; set; }
        public AutomatedFeedbackModel overallFeedback { get; set; }
    }
}

