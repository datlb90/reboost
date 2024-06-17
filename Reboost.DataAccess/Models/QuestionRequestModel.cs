using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Reboost.DataAccess.Entities;
using Reboost.Shared;

namespace Reboost.DataAccess.Models
{
    public class QuestionRequestModel: Questions
    {
        public int QuestionId { get; set; }
        public virtual List<QuestionParts> QuestionParts { get; set; }
        public List<IFormFile> UploadedFile { get; set; }
    }

    public class RequestReviewForWriting
    {
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public QuestionRequestModel Question { get; set; }
        public string Text { get; set; }
        public string FeedbackLanguage { get; set; }
    }
}
