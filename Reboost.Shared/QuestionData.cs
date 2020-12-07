using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.Shared
{
    public class QuestionData
    {
        public int Id { get; set; }
        public string  Test {get;set;}
        public string Task { get; set; }
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string Type { get; set; }
        public string MediaUrl { get; set; }
        public string Reading { get; set; }
        public string Transcript { get; set; }

    }
}
