using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Samples : BaseEntity
    {
        public int QuestionId { get; set; }
        public string SampleText { get; set; }
        [Column(TypeName = "decimal(4,1)")]
        public decimal? BandScore { get; set; }
        public string Comment { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string Status { get; set; }
    }
}
