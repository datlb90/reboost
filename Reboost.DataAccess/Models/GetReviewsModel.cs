using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Reboost.DataAccess.Models
{
    public class GetReviewsModel
    {
        public ReviewRequests ReviewRequest { get; set; }
        public Raters? Rater { get; set; }
        public Reviews Review { get; set; }
        public Submissions Submission { get; set; }
        public int ReviewId { get; set; }
        public String QuestionName { get; set; }
        public String TestSection { get; set; }
        public String QuestionType { get; set; }
        public String Error { get; set; }
    }
}
