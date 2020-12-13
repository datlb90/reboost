using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.Shared
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Section { get; set; }
        public string Test { get; set; }
        public string Type { get; set; }
        public bool Sample { get; set; }
        public string AverageScore { get; set; }
        public int Submission { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public string Status { get; set; }
        public string Direction { get; set; }
        public string UserId { get; set; }

        public List<QuestionPartModel> QuestionsPart { get; set; }

    }

    public class SummaryPerUser
    {
        public string Section { get; set; }
        public int Count { get; set; }
    }
    public class TestForCurrentUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
        public DateTime UpdateDate { get; set; }
        public string TestSectionName { get; set; }
        public string TestName { get; set; }
    }
    public class SampleForQuestion
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string SampleText { get; set; }
        public decimal? BandScore { get; set; }
        public string Comment { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string Title { get; set; }
    }
}
