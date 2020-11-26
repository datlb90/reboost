using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class RaterCredentials : BaseEntity
    {
        public int RaterId { get; set; }
        public int TestId { get; set; }
        public string CredentialType { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string FileExtension { get; set; }
        public virtual Raters Rater { get; set; }
        public virtual Tests Test { get; set; }
    }
}
