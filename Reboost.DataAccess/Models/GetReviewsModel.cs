using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Reboost.DataAccess.Models
{
    public class GetReviewsModel
    {
        public int Id { get; set; }
        public ReviewRequests ReviewRequest { get; set; }
        public Raters? Rater { get; set; }
        public Reviews Review { get; set; }
        public Submissions Submission { get; set; }
        public int QuestionId { get; set; }
        public int ReviewId { get; set; }
        public int DocId { get; set; }
        public string Status { get; set; }
        public string QuestionName { get; set; }
        public string Test { get; set; }
        public string TestSection { get; set; }
        public string QuestionType { get; set; }
        public string Error { get; set; }
        public string Rating { get; set; }
        public string ReviewType { get; set; }
    }

    public class InitialSubmissionModel
    {
        public int id { get; set; }
        public string feedbackLanguage { get; set; }
    }
}
