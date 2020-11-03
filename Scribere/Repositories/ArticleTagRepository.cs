using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Scribere.Models;
using Scribere.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scribere.Repositories
{
    public class ArticleTagRepository : BaseRepository,  IArticleTagRepository
    {
        public ArticleTagRepository(IConfiguration configuration) : base(configuration) { }

        private Tag NewTagFromReader(SqlDataReader reader)
        {
            Tag tag = new Tag()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                Title = DbUtils.GetString(reader, "Title")

            };
            return tag;
        }

        private ArticleTag NewArticleTagFromReader(SqlDataReader reader)
        {
            ArticleTag articleTag = new ArticleTag()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                ArticleId = DbUtils.GetInt(reader, "ArticleId"),
                TagId = DbUtils.GetInt(reader, "TagId")

            };
            return articleTag;
        }

        public List<Tag> GetAll(int articleId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT t.Id, t.Title 
                                        FROM Tag t
                                       LEFT JOIN ArticleTag at ON t.Id = at.TagId
                                    Where at.ArticleId = @articleId 
                                        ORDER BY t.Title;";

                    DbUtils.AddParameter(cmd, "@articleId", articleId);

                    var reader = cmd.ExecuteReader();
                    var tags = new List<Tag>();
                    while (reader.Read())
                    {
                        tags.Add(NewTagFromReader(reader));
                    }

                    reader.Close();

                    return tags;
                }
            }

        }

        public ArticleTag GetArticleTag(int articleTagId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT at.Id, at.ArticleTag, at.TagId 
                                        FROM ArticleTag at
                                    Where at.Id = @articleTagId;";

                    DbUtils.AddParameter(cmd, "@articleTagId", articleTagId);

                    var reader = cmd.ExecuteReader();
                    ArticleTag articleTag = null;
                    if (reader.Read())
                    {
                        articleTag = NewArticleTagFromReader(reader);
                    }
                    reader.Close();
                    return articleTag;
                }
            }

        }



        public void AddArticleTag(ArticleTag articleTag)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO ArticleTag ( TagId, ArticleId ) 
                                                OUTPUT INSERTED.ID
                                                  VALUES ( @TagId, @ArticleId );";
                    DbUtils.AddParameter(cmd,"@TagId", articleTag.TagId);
                    DbUtils.AddParameter(cmd,"@ArticleId", articleTag.ArticleId);

                    articleTag.Id = (int)cmd.ExecuteScalar();
                }
            }
        }



        public void DeleteArticleTag(int articleTagId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM ArticleTag WHERE ArticleId = @articleTagId;";

                    DbUtils.AddParameter(cmd,"@articleTagId", articleTagId);


                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
