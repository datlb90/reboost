using System;
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
}

