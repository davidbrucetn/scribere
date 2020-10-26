using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface IFavoriteArticleRepository
    {
        void AddFavoriteArticle(FavoriteArticle favoriteArticle);
        void DeleteFavoriteArticle(int UserId, int favoriteArticleId);
        List<FavoriteArticle> GetAll(int UserId);
    }
}