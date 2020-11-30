using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class ReviewModel
    {
        public int reviewId { get; set; }
        public string reviewerId { get; set; }
        public string revieweeId { get; set; }
        public string reviewStatus { get; set; }
        public int submissionId { get; set; }
        public int RequestId { get; set; }
        public int questionId { get; set; }
        public string testName { get; set; }
        public string sectionName { get; set; }
        public string taskName { get; set; }
        public int documentId { get; set; }
        public string fileName { get; set; }
        public byte[] docData { get; set; }
        public Rubrics rubric { get; set; }
        public List<QuestionParts> questionParts { get; set; }
        public List<ReviewData> reviewData { get; set; } 
        public List<Annotations> Annotations { get; set; }
    }
}
