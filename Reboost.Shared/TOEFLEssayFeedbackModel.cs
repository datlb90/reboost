using System;
namespace Reboost.Shared
{
    public class TOEFLEssayFeedbackModel
    {
        public string useOflanguge { get; set; }
        public string coherenceAccuracy { get; set; }
        public string developmentOrganization { get; set; }
        public string[] errors { get; set; }
        public float score { get; set; }
        public string overallFeedback { get; set; }
    }
}

