using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Scribere.Models;
using Scribere.Utils;
using Microsoft.Data.SqlClient;


namespace Scribere.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration configuration) : base(configuration) { }

        private Comment NewCommentFromReader(SqlDataReader reader)
        {
            UserImage UserImage = null;
            Comment comment = new Comment()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                ArticleId = DbUtils.GetInt(reader, "ArticleId"),
                UserId = DbUtils.GetInt(reader, "UserId"),
                Text = DbUtils.GetString(reader, "Text"),
                CreateDate = DbUtils.GetDateTime(reader, "CreateDate"),
                UserData = new UserData()
                {
                    Id = DbUtils.GetInt(reader, "UserId"),
                    NameFirst = DbUtils.GetString(reader, "NameFirst"),
                    NameLast = DbUtils.GetString(reader, "NameLast"),
                    Pseudonym = DbUtils.GetString(reader, "Pseudonym"),
                    Email = DbUtils.GetString(reader, "Email"),
                }
            };
            if (DbUtils.IsNotDbNull(reader, "UserImageId"))
            {
                UserImage = new UserImage()
                {
                    Id = DbUtils.GetInt(reader, "UserImageId"),
                    UserId = DbUtils.GetInt(reader, "UserId"),
                    ImageUrl = DbUtils.GetString(reader, "UserImageUrl")
                };
                comment.UserData.UserImage = UserImage;
            }

            return comment;
        }

        public List<Comment> GetAllCommentsByArticleId(int articleId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select c.Id, c.ArticleId, c.UserId, c.Text, c.CreateDate,
                                        u.NameFirst, u.NameLast, u.Pseudonym, u.Email,
                                        ui.Id as UserImageId, ui.ImageUrl as UserImageUrl
                                            FROM Comment c
                                        LEFT JOIN Article a on a.Id = c.ArticleId
                                        LEFT JOIN UserData u on u.Id = c.UserId
                                        LEFT JOIN UserImage ui on u.Id = ui.UserId
                                    WHERE a.Id = @articleId
                                        Order By c.CreateDate;";
                    cmd.Parameters.AddWithValue("@articleId", articleId);
                    var reader = cmd.ExecuteReader();
                    var books = new List<Comment>();
                    while (reader.Read())
                    {
                        books.Add(NewCommentFromReader(reader));
                    }

                    reader.Close();

                    return books;
                }
            }

        }

        public Comment GetCommentByCommentId(int commentId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select c.Id, c.ArticleId, c.UserId, c.Text, c.CreateDate,
                                        u.NameFirst, u.NameLast, u.Pseudonym, u.Email,
                                        ui.Id as UserImageId, ui.ImageUrl as UserImageUrl
                                            FROM Comment c
                                        LEFT JOIN Article a on a.Id = c.ArticleId
                                        LEFT JOIN UserData u on u.Id = c.UserId
                                        LEFT JOIN UserImage ui on u.Id = ui.UserId
                                   WHERE c.Id = @commentId;";
                    cmd.Parameters.AddWithValue("@commentId", commentId);
                    var reader = cmd.ExecuteReader();

                    Comment comment = null;
                    if (reader.Read())
                    {
                        comment = NewCommentFromReader(reader);
                    }
                    reader.Close();
                    return comment;
                }
            }
        }

        public void AddComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Comment ( ArticleId, UserId, Text, CreateDate ) 
                                                OUTPUT INSERTED.ID
                                                  VALUES ( @ArticleId, @UserId, @Text, @CreateDate ) 
                                        ;";
                    cmd.Parameters.AddWithValue("@ArticleId", comment.ArticleId);
                    cmd.Parameters.AddWithValue("@UserId", comment.UserId);
                    cmd.Parameters.AddWithValue("@Text", comment.Text);
                    cmd.Parameters.AddWithValue("@CreateDate", comment.CreateDate);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Comment
                                           SET
                                               ArticleId = @ArticleId,
                                                    UserId = @UserId,
                                                    Text = @Text, 
                                                   CreateDate = @CreateDate
                                            WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", comment.Id);
                    cmd.Parameters.AddWithValue("@ArticleId", comment.ArticleId);
                    cmd.Parameters.AddWithValue("@UserId", comment.UserId);
                    cmd.Parameters.AddWithValue("@Text", comment.Text);
                    cmd.Parameters.AddWithValue("@CreateDate", comment.CreateDate);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteComment(int commentId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Comment WHERE Id = @CommentId;";

                    cmd.Parameters.AddWithValue("@CommentId", commentId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
