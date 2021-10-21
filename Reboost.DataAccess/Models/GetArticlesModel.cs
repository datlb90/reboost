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
        public List<string> Labels { get; set; }
        public string Author { get; set; }
        public DateTime PostedDate { get; set; }
        public string OriginalUrl { get; set; }
        public string Question { get; set; }
        public List<int>? RelatedArticles { get; set; }
    }
}
