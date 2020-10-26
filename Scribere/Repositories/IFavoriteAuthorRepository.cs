using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface IFavoriteAuthorRepository
    {
        void AddFavoriteAuthor(FavoriteAuthor favoriteAuthor);
        void DeleteFavoriteAuthor(int SourceUserId, int favoriteAuthorId);
        List<FavoriteAuthor> GetAll(int sourceUserId);
    }
}