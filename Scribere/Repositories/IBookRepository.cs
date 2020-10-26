using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface IBookRepository
    {
        void AddBook(Book book);
        void DeleteBook(int bookId);
        List<Book> GetAll();
        Book GetBookByArticleId(int articleId);
        void UpdateBook(Book book);
    }
}