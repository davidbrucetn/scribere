using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface IArticleRepository
    {
        void Add(Article article);
        void DeleteArticle(int articleId);
        List<Article> GetAllArticles();
        List<Article> GetAllArticlesByUserId(int articleUserId);
        Article GetArticleByArticleId(int articleId);
        List<Article> GetFavoriteArticles(int SourceUserId);
        void UpdateArticle(Article article);
    }
}