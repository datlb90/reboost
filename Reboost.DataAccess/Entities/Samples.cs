using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Samples : BaseEntity
    {
        public int QuestionId { get; set; }
        public string SampleText { get; set; }
        public decimal? BandScore { get; set; }
        public string? Comment { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string? Status { get; set; }
    }
}
