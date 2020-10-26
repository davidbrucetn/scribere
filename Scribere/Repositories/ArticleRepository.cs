using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Scribere.Models;
using Scribere.Utils;
using Microsoft.Data.SqlClient;
using System;

namespace Scribere.Repositories
{
    public class ArticleRepository : BaseRepository, IArticleRepository
    {
        public ArticleRepository(IConfiguration config) : base(config) { }


        private Article NewArticleFromReader(SqlDataReader reader)
        {
            Article Article = null;
            ArticleImage ArticleImage = null;
            Article = new Article()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                Heading = DbUtils.GetString(reader, "Heading"),
                Text = DbUtils.GetString(reader, "Text"),
                CreateDate = DbUtils.GetDateTime(reader, "CreateDate"),
                CategoryId = DbUtils.GetInt(reader, "CategoryId"),
                Category = new Category()
                {
                    Id = DbUtils.GetInt(reader, "CategoryId"),
                    Type = DbUtils.GetString(reader, "CategoryType")
                },
                UserId = DbUtils.GetInt(reader, "UserId"),
                UserData = new UserData()
                {
                    Id = DbUtils.GetInt(reader, "UserId"),
                    NameFirst = DbUtils.GetString(reader, "NameFirst"),
                    NameLast = DbUtils.GetString(reader, "NameLast"),
                    Pseudonym = DbUtils.GetString(reader, "Pseudonym"),
                    Email = DbUtils.GetString(reader, "Email"),
                    CreateDate = DbUtils.GetDateTime(reader, "UserCreateDate"),
                    UserLevelId = DbUtils.GetInt(reader, "UserLevelId"),
                    UserLevel = new UserLevel()
                    {
                        Id = DbUtils.GetInt(reader, "UserLevelId"),
                        Level = DbUtils.GetString(reader, "Level")
                    }

                },
                VisibilityId = DbUtils.GetInt(reader, "VisibilityId"),
            };
            if (DbUtils.IsNotDbNull(reader, "ArticleImageId"))
            {
                ArticleImage = new ArticleImage()
                {
                    Id = DbUtils.GetInt(reader, "ArticleImageId"),
                    ArticleId = DbUtils.GetInt(reader, "Id"),
                    ImageUrl = DbUtils.GetString(reader, "ArticleImageUrl")
                };
                Article.ArticleImage = ArticleImage;
            };
            
            return Article;


        }

        public List<Article> GetAllArticles()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT a.Id, a.Heading, a.Text, 
                              ai.Id AS ArticleImageId, ai.ImageUrl AS ArticleImageUrl,
                              a.CreateDate, 
                              a.CategoryId, a.UserId,a.VisibilityId,
                              c.[Type] AS CategoryType,
                              u.NameFirst, u.NameLast, u.Pseudonym, 
                              u.Email, u.CreateDate as UserCreateDate, ui.ImageUrl AS UserImageUrl,
                              u.UserLevelId, 
                              ul.[Level]
                         FROM Article a
                              LEFT JOIN Category c ON a.CategoryId = c.id
                              LEFT JOIN UserData u ON a.UserId = u.id
                              LEFT JOIN UserLevel ul ON u.UserLevelId = ul.id
							  LEFT JOIN UserImage ui ON u.Id = ui.UserId
                              LEFT JOIN ArticleImage ai ON a.Id = ai.ArticleId
                        WHERE u.IsActive = 1 
                          AND a.VisibilityId = 2
                          AND a.CreateDate < SYSDATETIME()
                     ORDER BY a.CreateDate DESC;";
                    var reader = cmd.ExecuteReader();

                    var articles = new List<Article>();

                    while (reader.Read())
                    {
                        articles.Add(NewArticleFromReader(reader));
                    }

                    reader.Close();

                    return articles;
                }
            }
        }

        public List<Article> GetFavoriteArticles(int SourceUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT a.Id, a.Heading, a.Text, 
                              ai.Id AS ArticleImageId, ai.ImageUrl AS ArticleImageUrl,
                              a.CreateDate, 
                              a.CategoryId, a.UserId,a.VisibilityId,
                              c.[Type] AS CategoryType,
                              u.NameFirst, u.NameLast, u.Pseudonym, 
                              u.Email, u.CreateDate as UserCreateDate, ui.ImageUrl AS UserImageUrl,
                              u.UserLevelId, 
                              ul.[Level]
                         FROM Article a
                              LEFT JOIN Category c ON a.CategoryId = c.id
                              LEFT JOIN UserData u ON a.UserId = u.id
                              LEFT JOIN UserImage ui on a.UserId = ui.UserId
                              LEFT JOIN UserLevel ul ON u.UserLevelId = ul.id
                              LEFT JOIN FavoriteAuthor fa on u.id = fa.FavoriteUserId
                              LEFT JOIN FavoriteArticle far on u.id = far.ArticleId
                              LEFT JOIN ArticleImage ai ON a.Id = ai.ArticleId
                        WHERE u.IsActive = 1 
                          AND a.VisibilityId = 2
                          AND a.CreateDate < SYSDATETIME()
                          AND (fa.SourceUserId = @SourceUserId )
                     ORDER BY a.CreateDate DESC";

                    cmd.Parameters.AddWithValue("@SourceUserId", SourceUserId);
                    var reader = cmd.ExecuteReader();

                    var articles = new List<Article>();

                    while (reader.Read())
                    {
                        articles.Add(NewArticleFromReader(reader));
                    }

                    reader.Close();

                    return articles;
                }
            }
        }

        public List<Article> GetAllArticlesByUserId(int articleUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT a.Id, a.Heading, a.Text, 
                              ai.Id AS ArticleImageId, ai.ImageUrl AS ArticleImageUrl,
                              a.CreateDate, 
                              a.CategoryId, a.UserId,a.VisibilityId,
                              c.[Type] AS CategoryType,
                              u.NameFirst, u.NameLast, u.Pseudonym, 
                              u.Email, u.CreateDate as UserCreateDate, ui.ImageUrl AS UserImageUrl,
                              u.UserLevelId, 
                              ul.[Level]
                         FROM Article a
                              LEFT JOIN Category c ON a.CategoryId = c.id
                              LEFT JOIN UserData u ON a.UserId = u.id
                              LEFT JOIN UserImage ui on a.UserId = ui.UserId
                              LEFT JOIN UserLevel ul ON u.UserLevelId = ul.id
                              LEFT JOIN ArticleImage ai ON a.Id = ai.ArticleId
                        WHERE u.IsActive = 1 
                          AND a.VisibilityId = 2
                          AND a.UserId = @articleUserId
                     ORDER BY a.CreateDate DESC;";

                    cmd.Parameters.AddWithValue("@articleUserId", articleUserId);
                    var reader = cmd.ExecuteReader();

                    var articles = new List<Article>();
                    while (reader.Read())
                    {
                        articles.Add(NewArticleFromReader(reader));
                    }
                    reader.Close();
                    return articles;
                }
            }
        }

        public Article GetArticleByArticleId(int articleId)
        
        
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT a.Id, a.Heading, a.Text, 
                              ai.Id AS ArticleImageId, ai.ImageUrl AS ArticleImageUrl,
                              a.CreateDate, 
                              a.CategoryId, a.UserId,a.VisibilityId,
                              c.[Type] AS CategoryType,
                              u.NameFirst, u.NameLast, u.Pseudonym, 
                              u.Email, u.CreateDate as UserCreateDate, ui.ImageUrl AS UserImageUrl,
                              u.UserLevelId, 
                              ul.[Level]
                         FROM Article a
                              LEFT JOIN Category c ON a.CategoryId = c.id
                              LEFT JOIN UserData u ON a.UserId = u.id
                              LEFT JOIN UserImage ui on a.UserId = ui.UserId
                              LEFT JOIN UserLevel ul ON u.UserLevelId = ul.id
                              LEFT JOIN ArticleImage ai ON a.Id = ai.ArticleId
                        WHERE u.IsActive = 1
                          AND a.id = @articleId;";

                    cmd.Parameters.AddWithValue("@articleId", articleId);
                    var reader = cmd.ExecuteReader();

                    Article article = null;
                    if (reader.Read())
                    {
                        article = NewArticleFromReader(reader);
                    }
                    reader.Close();
                    return article;
                }
            }
        }

        public void Add(Article article)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DECLARE @ArticleImage TABLE (
                            [ArticleId] INT,
                            [ImageUrl]  VARCHAR(300)
                        )

                        INSERT INTO Article (UserId, Heading, Text, CategoryId, CreateDate, VisibilityId) 
                            OUTPUT INSERTED.id, @ArticleImageUrl INTO @ArticleImage
                               VALUES (@Heading, @Text,@CreateDate, @CategoryId, @UserId)

                        INSERT INTO ArticleImage (ArticleId,ImageUrl) SELECT [ArticleId],[ImageUrl] FROM @ArticleImage;
                    ";
                    cmd.Parameters.AddWithValue("@Heading", article.Heading);
                    cmd.Parameters.AddWithValue("@Text", article.Text);
                    cmd.Parameters.AddWithValue("@ArticleImageUrl", article.ArticleImage.ImageUrl ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreateDate", article.CreateDate);
                    cmd.Parameters.AddWithValue("@CategoryId", article.CategoryId);
                    cmd.Parameters.AddWithValue("@VisibilityId", article.VisibilityId);
                    cmd.Parameters.AddWithValue("@UserId", article.UserId);

                    article.Id = (int)cmd.ExecuteScalar();
                }
            }
        }



        public void DeleteArticle(int articleId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM ArticleTag WHERE ArticleId = @Id;
                        DELETE FROM Comment WHERE ArticleId = @Id;
                        DELETE FROM ArticleImage WHERE ArticleID = @Id;
                        DELETE FROM Article WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", articleId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateArticle(Article article)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                     BEGIN
      
                       UPDATE Article
                                 SET
                                    Heading = @Heading,
                                    Article.Text = @Text,
                                    CreateDate = @CreateDate, 
                                    CategoryId = @CategoryId, 
                                    VisibilityId = @VisibilityId,
                                    UserId = @UserId
                       WHERE Id = @Id

                            If @ArticleImageUrl IS NOT NULL
                            BEGIN
                                        UPDATE ArticleImage
                                            SET
                                                ImageUrl = @ArticleImageUrl
                                        WHERE ArticleId = @Id
                            END
                     END;
                    ";

                    cmd.Parameters.AddWithValue("@Heading", article.Heading);
                    cmd.Parameters.AddWithValue("@Text", article.Text);
                    cmd.Parameters.AddWithValue("@ArticleImageUrl", article.ArticleImage.ImageUrl ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreateDate", article.CreateDate);
                    cmd.Parameters.AddWithValue("@CategoryId", article.CategoryId);
                    cmd.Parameters.AddWithValue("@VisibilityId", article.VisibilityId);
                    cmd.Parameters.AddWithValue("@UserId", article.UserId);
                    cmd.Parameters.AddWithValue("@Id", article.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
