using System;
using System.Collections.Generic;

namespace Reboost.Shared
{
    public class CriteriaFeedback
    {
        public int criteriaId { get; set; }
        public string name { get; set; }
        public decimal mark { get; set; }
        public string comment { get; set; }
    }

    public class FeedbackRubric
    {
        public int criteriaId { get; set; }
        public string name { get; set; }
        public int order { get; set; }
    }
}

