using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess.Models
{
    public class QuestionRequestModel: Questions
    {
        public virtual List<QuestionParts> QuestionParts { get; set; }
        public List<IFormFile> UploadedFile { get; set; }
    }
}
