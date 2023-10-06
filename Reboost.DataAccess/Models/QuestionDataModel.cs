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

        // To Be Done: Update QuestionDataModel
        //public int TestId { get; set; }
        //public string TestName { get; set; }
        //public int TaskId { get; set; }
        //public string TaskName { get; set; }
        //public string TypeName { get; set; }
    }
}
