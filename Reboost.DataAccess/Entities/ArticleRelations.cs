using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.DataAccess.Entities
{
    public class ArticleRelations : BaseEntity
    {
        public int ArticleId { get; set; }
        public int RelatedArticle { get; set; }
    }
}
