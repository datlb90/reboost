using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class DocumentRequestModel: Entities.Documents
    {
        public string UserId { get; set; }
        public int QuestionId { get; set; }
        public int TimeSpentInSeconds { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
