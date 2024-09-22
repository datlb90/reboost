using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reboost.DataAccess.Entities
{

    public class Courses : BaseEntity
    {
        public Courses()
        {
            this.Chapters = new HashSet<Chapters>();
        }

        public int TaskId { get; set; }
        public string Title { get; set; }
        public string TitleVn { get; set; }
        public string UrlTitle { get; set; }
        public string Description { get; set; }
        public string DescriptionVn { get; set; }
        public string Overview { get; set; }
        public string OverviewVn { get; set; }
        public int Price { get; set; }
        public int OriginalPrice { get; set; }
        public int StudyHours { get; set; }
        public string Tag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<Chapters> Chapters { get; set; }
    }

    public class Chapters : BaseEntity
    {
        public Chapters()
        {
            this.Lessons = new HashSet<Lessons>();
        }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string TitleVn { get; set; }
        public int Order { get; set; }

        public virtual Courses Course { get; set; }
        public virtual ICollection<Lessons> Lessons { get; set; }
    }

    public class Lessons : BaseEntity
    {
        [ForeignKey("Chapter")]
        public int ChapterId { get; set; }
        public string Title { get; set; }
        public string TitleVn { get; set; }
        public string UrlTitle { get; set; }
        public string Content { get; set; }
        public string ContentVn { get; set; }
        public int? QuestionId { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Chapters Chapter { get; set; }
    }
}

