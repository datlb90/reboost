using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class Annotations : BaseEntity
    {
        [Column("DocId")]
        public int DocumentId { get; set; }
        public int ReviewId { get; set; }
        public string Type { get; set; }
        [Column("PageNumber")]
        public int PageNum { get; set; }
        [Column("TopPosition")]
        public double Top { get; set; }
        public string Color { get; set; }
        public string Data { get; set; }
        [NotMapped]
        public string Uuid { get; set; }


        public virtual ICollection<InTextComments> InTextComments { get; set; }
    }
}
