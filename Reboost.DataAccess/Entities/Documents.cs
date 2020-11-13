using System;
using System.ComponentModel.DataAnnotations;

namespace Reboost.DataAccess.Entities
{
    public class Documents : BaseEntity
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string Status { get; set; }
        public int PageCount { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
