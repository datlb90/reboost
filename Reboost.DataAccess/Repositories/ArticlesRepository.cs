using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface IArticlesRepository : IRepository<Articles>
    {
        Task<Articles> CreateNewArticleAsync(CreateArticlesModel data);
        Task<List<GetArticlesModel>> GetAllArticlesAsync();
        Task<IEnumerable<ArticleLabels>> GetAllArticleLabelsAsync();
        Task<IEnumerable<ArticleRelations>> GetAllArticleRelationsAsync();
        Task<GetArticlesModel> GetArticleById(int id, string userId);
        Task<Articles> UpdateArticleAsync(int id, CreateArticlesModel data);
        Task<Articles> DeleteArticleAsync(int id);
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

        public async Task<List<GetArticlesModel>> GetAllArticlesAsync()
        {
            var listArticles = await db.Articles.ToListAsync();

            List<GetArticlesModel> result = new List<GetArticlesModel>();
            for(int i =0; i<listArticles.Count; i++)
            {
                var author = await db.Users.Where(u => u.Id == listArticles[i].Author).FirstOrDefaultAsync();
                var labels = await db.ArticleLabels.Where(l => l.ArticleId == listArticles[i].Id).ToListAsync();
                var relations = await db.ArticleRelations.Where(r => r.ArticleId == listArticles[i].Id).ToListAsync();
                result.Add(new GetArticlesModel
                {
                    Id = listArticles[i].Id,
                    Title = listArticles[i].Title,
                    Category = listArticles[i].Category,
                    Author = author,
                    AuthorId = listArticles[i].Author,
                    PostedDate = listArticles[i].PostedDate,
                    OriginalUrl = listArticles[i].OriginalUrl,
                    Question = listArticles[i].Question,
                    Labels = labels,
                    RelatedArticles = relations,
                    Views = listArticles[i].Views
                });
            }

            return result;
        }
        
        public async Task<GetArticlesModel> GetArticleById(int id, string userId)
        {
            var articles = await db.Articles.FindAsync(id);

            GetArticlesModel result = new GetArticlesModel();
            var author = await db.Users.Where(u => u.Id == articles.Author).FirstOrDefaultAsync();
            var labels = await db.ArticleLabels.Where(l => l.ArticleId == articles.Id).ToListAsync();
            var relations = await db.ArticleRelations.Where(r => r.ArticleId == articles.Id).ToListAsync();
            result = new GetArticlesModel
            {
                Id = articles.Id,
                Title = articles.Title,
                Category = articles.Category,
                Author = author,
                AuthorId = articles.Author,
                PostedDate = articles.PostedDate,
                OriginalUrl = articles.OriginalUrl,
                Question = articles.Question,
                Labels = labels,
                RelatedArticles = relations,
                Views = articles.Views
            };

            if(author?.Id != userId)
            {
                articles.Views++;
                await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IEnumerable<ArticleLabels>> GetAllArticleLabelsAsync()
        {
            return await db.ArticleLabels.ToListAsync();
        }

        public async Task<IEnumerable<ArticleRelations>> GetAllArticleRelationsAsync()
        {
            return await db.ArticleRelations.ToListAsync();
        }

        public async Task<Articles> UpdateArticleAsync(int id,CreateArticlesModel data)
        {
            var exist = await db.Articles.FindAsync(id);
            if (exist!= null)
            {
                exist.Author = data.Author;
                exist.Category = data.Category;
                exist.OriginalUrl = data.OriginalUrl;
                exist.Question = data.Question;
                exist.Title = data.Title;

                var labels = await db.ArticleLabels.Where(a => a.ArticleId == id).ToListAsync();
                db.ArticleLabels.RemoveRange(labels);

                var relations = await db.ArticleRelations.Where(r => r.ArticleId == id).ToListAsync();
                db.ArticleRelations.RemoveRange(relations);

                if (data.Labels != null)
                {
                    List<ArticleLabels> listLabels = new List<ArticleLabels>();
                    data.Labels.ForEach(r =>
                    {
                        listLabels.Add(new ArticleLabels
                        {
                            Name = r,
                            ArticleId = exist.Id
                        });
                    });
                    await db.ArticleLabels.AddRangeAsync(listLabels);
                }

                if (data.RelatedArticles != null)
                {
                    List<ArticleRelations> listRelations = new List<ArticleRelations>();
                    data.RelatedArticles.ForEach(r =>
                    {
                        listRelations.Add(new ArticleRelations
                        {
                            ArticleId = exist.Id,
                            RelatedArticle = r,
                        });
                    });

                    await db.ArticleRelations.AddRangeAsync(listRelations);
                }
                await db.SaveChangesAsync();

                return exist;
            }
            else
            {
                return null;
            }
        }

        public async Task<Articles> DeleteArticleAsync(int id)
        {
            var exist = await db.Articles.FindAsync(id);
            if (exist != null)
            {         
                var labels = await db.ArticleLabels.Where(a => a.ArticleId == id).ToListAsync();
                db.ArticleLabels.RemoveRange(labels);

                var relations = await db.ArticleRelations.Where(r => r.ArticleId == id).ToListAsync();
                db.ArticleRelations.RemoveRange(relations);

                db.Articles.Remove(exist);
                await db.SaveChangesAsync();
                return exist;
            }
            else
            {
                return null;
            }
        }
    }
}
