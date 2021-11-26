using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Models
{
    public class GetArticlesModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public List<ArticleLabels> Labels { get; set; }
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public DateTime PostedDate { get; set; }
        public string OriginalUrl { get; set; }
        public string Question { get; set; }
        public List<ArticleRelations>? RelatedArticles { get; set; }
        public int Views { get; set; }
    }
}
