using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Photo : BaseEntity
    {
        public int UserId { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
        public string FileType { get; set; }
        public Rater Rater { get; set; }

    }
}
