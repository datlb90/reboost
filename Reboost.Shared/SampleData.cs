using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.Shared
{
    public class SampleData
    {
        public int Id { get; set; }
        public int QuestionId {get;set;}
        public string SampleText { get; set; }
        public decimal? Rate { get; set; }
        public string Comment { get; set; }
    }
}
