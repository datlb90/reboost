using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IArticlesService
    {
        Task<Articles> CreateNewArticleAsync(CreateArticlesModel articles);
        Task<List<GetArticlesModel>> GetAllArticlesAsync();
        Task<IEnumerable<ArticleLabels>> GetAllArticleLabelsAsync();
        Task<IEnumerable<ArticleRelations>> GetAllArticleRelationsAsync();
        Task<GetArticlesModel> GetArticleById(int id, string userId);
        Task<Articles> UpdateArticleAsync(int id, CreateArticlesModel data);
        Task<Articles> DeleteArticleAsync(int id);
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

        public async Task<List<GetArticlesModel>> GetAllArticlesAsync()
        {
            return await _unitOfWork.Articles.GetAllArticlesAsync();
        }

        public async Task<IEnumerable<ArticleLabels>> GetAllArticleLabelsAsync()
        {
            return await _unitOfWork.Articles.GetAllArticleLabelsAsync();
        }
        public async Task<IEnumerable<ArticleRelations>> GetAllArticleRelationsAsync()
        {
            return await _unitOfWork.Articles.GetAllArticleRelationsAsync();
        }
        public async Task<GetArticlesModel> GetArticleById(int id, string userId)
        {
            return await _unitOfWork.Articles.GetArticleById(id, userId);
        }

        public async Task<Articles> UpdateArticleAsync(int id, CreateArticlesModel data)
        {
            var rs = await _unitOfWork.Articles.UpdateArticleAsync(id, data);
            if (rs == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Article not exists!");
            }
            return rs;
        }

        public async Task<Articles> DeleteArticleAsync(int id)
        {
            var rs = await _unitOfWork.Articles.DeleteArticleAsync(id);
            if (rs == null)
            {
                throw new AppException(ErrorCode.InvalidArgument, "Article not exists!");
            }
            return rs;
        }
    }
}
