using System;
using System.Collections.Generic;
using System.Text;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess.Models
{
    public class QuestionDataModel
    {
        public List<Tests> Tests { get; set; }
        public List<Tasks> Tasks { get; set; }
        public List<String> Types { get; set; }
    }
}
