using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class InTextComments : BaseEntity
    {
        public int AnnotationId { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public int TopPosition { get; set; }
        public virtual Annotations Annotations { get; set; }
    }
}
