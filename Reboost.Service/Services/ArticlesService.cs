using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IArticlesService
    {
        Task<Articles> CreateNewArticleAsync(CreateArticlesModel articles);
        Task<IEnumerable<Articles>> GetAllArticlesAsync();
        Task<IEnumerable<ArticleLabels>> GetAllArticleLabelsAsync();
        Task<IEnumerable<ArticleRelations>> GetAllArticleRelationsAsync();
    }
    public class ArticlesService : BaseService, IArticlesService
    {
        public ArticlesService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<Articles> CreateNewArticleAsync(CreateArticlesModel articles)
        {
            return await _unitOfWork.Articles.CreateNewArticleAsync(articles);
        }

        public async Task<IEnumerable<Articles>> GetAllArticlesAsync()
        {
            return await _unitOfWork.Articles.GetAllAsync();
        }

        public async Task<IEnumerable<ArticleLabels>> GetAllArticleLabelsAsync()
        {
            return await _unitOfWork.Articles.GetAllArticleLabelsAsync();
        }
        public async Task<IEnumerable<ArticleRelations>> GetAllArticleRelationsAsync()
        {
            return await _unitOfWork.Articles.GetAllArticleRelationsAsync();
        }
    }
}
