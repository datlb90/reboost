using System;
using System.Collections.Generic;
using System.Text;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess.Models
{
    public class ReviewResponseModel
    {
        public Raters? Rater { get; set; }
        public Reviews Reviews { get; set; }
        public Boolean SendEmail { get; set; } = false;
        public String EmailSubject { get; set; }
        public String EmailContent { get; set; }
        public String RaterEmail { get; set; }
    }
}
