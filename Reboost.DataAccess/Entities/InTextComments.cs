using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class InTextComments: BaseEntity 
    {
        public int  Id { get; set; }

        [ForeignKey("Annotations")]
        public int AnnotationId { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public int TopPosition { get; set; }
    }
}
