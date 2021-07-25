using System;
using System.Collections.Generic;
using System.Text;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess.Models
{
    public class CreatedProRequestModel
    {
        public Raters Rater { get; set; }
        public ReviewRequests Request { get; set; }
    }
}
