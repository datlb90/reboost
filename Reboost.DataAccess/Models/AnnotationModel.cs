using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class AnnotationModel
    {
        public IEnumerable<Annotations> Annotations { get; set; }
        public IEnumerable<InTextComments> Comments { get; set; }
    }
}
