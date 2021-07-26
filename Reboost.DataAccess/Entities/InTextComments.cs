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
        public double TopPosition { get; set; }
        public string Data { get; set; }
        [NotMapped]
        public string Uuid { get; set; }

        public virtual Annotations Annotation { get; set; }
    }
    public class InsertCommentModel
    {
        public Annotations Annotation { get; set; }
        public InTextComments Comment { get; set; }
    }
    public class DeleteCommentModel
    {
        public int id { get; set; }
    }
    public class UpdateStatusModel
    {
        public string status { get; set; }
        public string note { get; set; }
    }
}
