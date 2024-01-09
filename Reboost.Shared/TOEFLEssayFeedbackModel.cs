using System;
using System.Collections.Generic;

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

    public class NewTOEFLEssayFeedbackModel
    {
        public AutomatedFeedbackModel useOflanguge { get; set; }
        public AutomatedFeedbackModel coherenceAccuracy { get; set; }
        public AutomatedFeedbackModel developmentOrganization { get; set; }
        public AutomatedFeedbackModel overallFeedback { get; set; }
        public List<AutomatedFeedbackError> errors { get; set; }
    }

    public class AutomatedFeedbackModel
    {
        public string comment { get; set; }
        public float score { get; set; }
    }

    public class AutomatedFeedbackError
    {
        public string issue { get; set; }
        public string type { get; set; }
        public string fix { get; set; }
    }
}

