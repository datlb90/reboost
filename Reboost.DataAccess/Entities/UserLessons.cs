using System;
namespace Reboost.DataAccess.Entities
{
    public class UserLessons : BaseEntity
    {
        public string UserId { get; set; }
        public int LessonId { get; set; }
        public string SubmittedText { get; set; }
        public string Feedback { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

