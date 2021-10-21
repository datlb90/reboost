using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Articles : BaseEntity
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public DateTime PostedDate { get; set; }
        public string OriginalUrl { get; set; }
        public string? Question { get; set; }
    }
}
