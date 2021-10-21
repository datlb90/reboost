using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Reboost.DataAccess.Repositories
{
    public interface IArticlesRepository : IRepository<Articles>
    {
        Task<Articles> CreateNewArticleAsync(CreateArticlesModel data);
        Task<IEnumerable<ArticleLabels>> GetAllArticleLabelsAsync();
        Task<IEnumerable<ArticleRelations>> GetAllArticleRelationsAsync();
    }
    public class ArticlesRepository : BaseRepository<Articles>, IArticlesRepository
    {
        private ReboostDbContext db => context as ReboostDbContext;
        public ArticlesRepository(ReboostDbContext context) : base(context)
        { }

        public async Task<Articles> CreateNewArticleAsync(CreateArticlesModel data)
        {
            Articles art = new Articles {
                Title = data.Title,
                Category = data.Category,
                Author = data.Author,
                PostedDate = data.PostedDate,
                OriginalUrl = data.OriginalUrl,
                Question = data.Question
            };

            await db.Articles.AddAsync(art);

            await db.SaveChangesAsync();
            if (art != null)
            {
                if(data.Labels != null)
                {
                    List<ArticleLabels> listLabels = new List<ArticleLabels>();
                    data.Labels.ForEach(r =>
                    {
                        listLabels.Add(new ArticleLabels
                        {
                            Name = r,
                            ArticleId = art.Id
                        });
                    });
                    await db.ArticleLabels.AddRangeAsync(listLabels);
                }

                if(data.RelatedArticles != null)
                {
                    List<ArticleRelations> listRelations = new List<ArticleRelations>();
                    data.RelatedArticles.ForEach(r =>
                    {
                        listRelations.Add(new ArticleRelations
                        {
                            ArticleId = art.Id,
                            RelatedArticle = r,
                        });
                    });

                    await db.ArticleRelations.AddRangeAsync(listRelations);
                }
                await db.SaveChangesAsync();
            }

            return art;
        }

        public async Task<IEnumerable<ArticleLabels>> GetAllArticleLabelsAsync()
        {
            return await db.ArticleLabels.ToListAsync();
        }

        public async Task<IEnumerable<ArticleRelations>> GetAllArticleRelationsAsync()
        {
            return await db.ArticleRelations.ToListAsync();
        }
    }
}
