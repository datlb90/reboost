using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class InTextComments : BaseEntity
    {
        public int AnnotationId { get; set; }
        public string Text { get; set; }
        [Column("Comment")]
        public string Content { get; set; }
        [Column("TopPosition")]
        public int TopPosition { get; set; }
        public string Data { get; set; }
        [NotMapped]
        public string Uuid { get; set; }

        public virtual Annotations Annotation { get; set; }
    }
}
