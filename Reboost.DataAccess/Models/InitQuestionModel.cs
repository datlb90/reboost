using System;
using Reboost.Shared;
using System.Collections.Generic;

namespace Reboost.DataAccess.Models
{
    public class InitQuestionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Section { get; set; }
        public int TaskId { get; set; }
        public string Test { get; set; }
        public int TestId { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public bool Sample { get; set; }
        public string AverageScore { get; set; }
        public int Submission { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public string Status { get; set; }
        public string Difficulty { get; set; }
        public string Direction { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public List<QuestionPartModel> QuestionsPart { get; set; }
        public List<SampleForQuestion> Samples { get; set; }
        public List<RubricsModel> Rubrics { get; set; }
    }
}

