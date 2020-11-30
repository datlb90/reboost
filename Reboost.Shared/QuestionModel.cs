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

    public class SummeryPerUser
    {
        public string Section { get; set; }
        public int Count { get; set; }
    }
}
