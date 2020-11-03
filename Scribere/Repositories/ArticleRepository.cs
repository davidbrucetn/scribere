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
                Visibility = new Visibility()
                {
                    Id = DbUtils.GetInt(reader, "VisibilityId"),
                    Type = DbUtils.GetString(reader, "VisibilityType"),
                },
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
            } else
            {
                ArticleImage = new ArticleImage()
                {
                    ArticleId = DbUtils.GetInt(reader, "Id"),
                    ImageUrl = ""
                };
                Article.ArticleImage = ArticleImage;
            }
            
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
                              ul.[Level],
                              v.Type as VisibilityType
                         FROM Article a
                              LEFT JOIN Category c ON a.CategoryId = c.id
                              LEFT JOIN UserData u ON a.UserId = u.id
                              LEFT JOIN UserLevel ul ON u.UserLevelId = ul.id
							  LEFT JOIN UserImage ui ON u.Id = ui.UserId
                              LEFT JOIN ArticleImage ai ON a.Id = ai.ArticleId
                              LEFT JOIN Visibility v on v.Id = a.VisibilityId
                        WHERE u.IsActive = 1 
                          AND a.VisibilityId = 2
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
                              ul.[Level],
                              v.Type as VisibilityType
                         FROM Article a
                              LEFT JOIN Category c ON a.CategoryId = c.id
                              LEFT JOIN UserData u ON a.UserId = u.id
                              LEFT JOIN UserImage ui on a.UserId = ui.UserId
                              LEFT JOIN UserLevel ul ON u.UserLevelId = ul.id
                              LEFT JOIN FavoriteAuthor fa on u.id = fa.FavoriteUserId
                              LEFT JOIN ArticleImage ai ON a.Id = ai.ArticleId
                              LEFT JOIN Visibility v on v.Id = a.VisibilityId
                        WHERE u.IsActive = 1 
                          AND a.VisibilityId = 2
                          AND ( fa.SourceUserId = @SourceUserId  )
                    UNION 
                    SELECT a.Id, a.Heading, a.Text, 
                              ai.Id AS ArticleImageId, ai.ImageUrl AS ArticleImageUrl,
                              a.CreateDate, 
                              a.CategoryId, a.UserId,a.VisibilityId,
                              c.[Type] AS CategoryType,
                              u.NameFirst, u.NameLast, u.Pseudonym, 
                              u.Email, u.CreateDate as UserCreateDate, ui.ImageUrl AS UserImageUrl,
                              u.UserLevelId, 
                              ul.[Level],
                              v.Type as VisibilityType
                         FROM Article a
                              LEFT JOIN Category c ON a.CategoryId = c.id
                              LEFT JOIN UserData u ON a.UserId = u.id
                              LEFT JOIN UserImage ui on a.UserId = ui.UserId
                              LEFT JOIN UserLevel ul ON u.UserLevelId = ul.id
                              LEFT JOIN FavoriteArticle far on a.Id = far.ArticleId
                              LEFT JOIN ArticleImage ai ON a.Id = ai.ArticleId
                              LEFT JOIN Visibility v on v.Id = a.VisibilityId
                        WHERE u.IsActive = 1 
                          AND a.VisibilityId = 2
                          AND ( far.UserId = @SourceUserId  )
                     ORDER BY a.CreateDate ;";

                    DbUtils.AddParameter(cmd,"@SourceUserId", SourceUserId);
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
                              ul.[Level],
                              v.Type as VisibilityType
                         FROM Article a
                              LEFT JOIN Category c ON a.CategoryId = c.id
                              LEFT JOIN UserData u ON a.UserId = u.id
                              LEFT JOIN UserImage ui on a.UserId = ui.UserId
                              LEFT JOIN UserLevel ul ON u.UserLevelId = ul.id
                              LEFT JOIN ArticleImage ai ON a.Id = ai.ArticleId
                              LEFT JOIN Visibility v on v.Id = a.VisibilityId
                        WHERE u.IsActive = 1 
                          AND a.UserId = @articleUserId
                     ORDER BY a.CreateDate DESC;";

                    DbUtils.AddParameter(cmd,"@articleUserId", articleUserId);
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
                              ul.[Level],
                              v.Type as VisibilityType
                         FROM Article a
                              LEFT JOIN Category c ON a.CategoryId = c.id
                              LEFT JOIN UserData u ON a.UserId = u.id
                              LEFT JOIN UserImage ui on a.UserId = ui.UserId
                              LEFT JOIN UserLevel ul ON u.UserLevelId = ul.id
                              LEFT JOIN ArticleImage ai ON a.Id = ai.ArticleId
                              LEFT JOIN Visibility v on v.Id = a.VisibilityId
                        WHERE u.IsActive = 1
                          AND a.id = @articleId;";

                    DbUtils.AddParameter(cmd,"@articleId", articleId);
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
                     BEGIN
                        DECLARE @ArticleImage TABLE (
                            [ArticleId] INT,
                            [ImageUrl]  VARCHAR(300)
                        )

                        INSERT INTO Article (UserId, Heading, Text, CreateDate, CategoryId, VisibilityId) 
                            OUTPUT INSERTED.ID, @ArticleImageUrl INTO @ArticleImage
                               VALUES (@UserId, @Heading, @Text, @CreateDate, @CategoryId, @VisibilityId)
                        
                       
                             INSERT INTO ArticleImage (ArticleId,ImageUrl) SELECT [ArticleId],[ImageUrl] FROM @ArticleImage WHERE [ImageUrl] <> NULL OR [ImageUrl] <> ''
                       

                        SELECT [ArticleId] from @ArticleImage
                           
                     END;"; 

                    DbUtils.AddParameter(cmd,"@Heading", article.Heading);
                    DbUtils.AddParameter(cmd,"@Text", article.Text);
                    DbUtils.AddParameter(cmd,"@ArticleImageUrl", article.ArticleImage.ImageUrl ?? (object)DBNull.Value);
                    DbUtils.AddParameter(cmd,"@CreateDate", article.CreateDate);
                    DbUtils.AddParameter(cmd,"@CategoryId", article.CategoryId);
                    DbUtils.AddParameter(cmd,"@VisibilityId", article.VisibilityId);
                    DbUtils.AddParameter(cmd,"@UserId", article.UserId);

                     var result = (int)cmd.ExecuteScalar();
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

                    DbUtils.AddParameter(cmd,"@Id", articleId);

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
                                IF EXISTS(SELECT @Id FROM ArticleImage WHERE ArticleImage.ArticleId = @Id)
	                                BEGIN 
				                        UPDATE ArticleImage SET ImageUrl=@ArticleImageURL WHERE ArticleId = @Id
	                                END
                                ELSE
                                    BEGIN
                                        INSERT INTO ArticleImage (ArticleId,ImageUrl) VALUES (@Id, @ArticleImageUrl)
                                    END 
                            END
                     END;
                    ";

                    
                    DbUtils.AddParameter(cmd,"@Heading", article.Heading);
                    DbUtils.AddParameter(cmd,"@Text", article.Text);
                    DbUtils.AddParameter(cmd,"@ArticleImageUrl", article.ArticleImage.ImageUrl ?? (object)DBNull.Value);
                    DbUtils.AddParameter(cmd,"@CreateDate", article.CreateDate);
                    DbUtils.AddParameter(cmd,"@CategoryId", article.CategoryId);
                    DbUtils.AddParameter(cmd,"@VisibilityId", article.VisibilityId);
                    DbUtils.AddParameter(cmd,"@UserId", article.UserId);
                    DbUtils.AddParameter(cmd,"@Id", article.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
