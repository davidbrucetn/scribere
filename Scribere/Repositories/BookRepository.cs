using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scribere.Models;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Scribere.Utils;

namespace Scribere.Repositories
{
    public class BookRepository : BaseRepository, IBookRepository
    {
        public BookRepository(IConfiguration configuration) : base(configuration) { }

        private Book NewArticleFromReader(SqlDataReader reader)
        {
            Book book = new Book()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                ArticleId = DbUtils.GetInt(reader, "ArticleId"),
                ISBN = DbUtils.GetString(reader, "ISBN"),
                LCCN = DbUtils.GetString(reader, "LCCN"),
                Title = DbUtils.GetString(reader, "Title"),
                Author = DbUtils.GetString(reader, "Author"),
            };
            return book;
        }

        public List<Book> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select Id, ArticleId, ISBN, LCCN, Title, Author FROM BOOK Order By Title;";
                    var reader = cmd.ExecuteReader();
                    var books = new List<Book>();
                    while (reader.Read())
                    {
                        books.Add(NewArticleFromReader(reader));
                    }

                    reader.Close();

                    return books;
                }
            }

        }

        public Book GetBookByArticleId(int articleId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select Id, ArticleId, ISBN, LCCN, Title, Author FROM BOOK WHERE ArticleId = @articleId;";
                    cmd.Parameters.AddWithValue("@articleId", articleId);
                    var reader = cmd.ExecuteReader();

                    Book book = null;
                    if (reader.Read())
                    {
                        book = NewArticleFromReader(reader);
                    }
                    reader.Close();
                    return book;
                }
            }
        }

        public void AddBook(Book book)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Book ( ArticleId, ISBN, LCCN, Title, Author ) 
                                                OUPUT INSERTED.Id
                                                  VALUES ( @ArticleId, @ISBN, @LCCN, @Title, @Author ) 
                                        ;";
                    cmd.Parameters.AddWithValue("@ArticleId", book.ArticleId);
                    cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                    cmd.Parameters.AddWithValue("@LCCN", book.LCCN);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);

                    book.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateBook(Book book)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Book
                                           SET
                                               ArticleId = @ArticleId,
                                                    ISBN = @ISBN,
                                                    LCCN = @LCCN, 
                                                   Title = @Title, 
                                                  Author = @Author
                                            WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", book.Id);
                    cmd.Parameters.AddWithValue("@ArticleId", book.ArticleId);
                    cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                    cmd.Parameters.AddWithValue("@LCCN", book.LCCN);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);


                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteBook(int bookId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Book WHERE Id = @BookId;";

                    cmd.Parameters.AddWithValue("@BookId", bookId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
