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
    public class FavoriteArticleRepository : BaseRepository, IFavoriteArticleRepository
    {
        public FavoriteArticleRepository(IConfiguration configuration) : base(configuration) { }

        private FavoriteArticle NewArticleFromReader(SqlDataReader reader)
        {
            FavoriteArticle favoriteArticle = new FavoriteArticle()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                UserId = DbUtils.GetInt(reader, "UserId"),
                ArticleId = DbUtils.GetInt(reader, "ArticleId")

            };
            return favoriteArticle;
        }

        public List<FavoriteArticle> GetAll(int UserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Type FROM FavoriteArticle WHERE UserId = @UserId ORDER BY ArticleId;";

                    DbUtils.AddParameter(cmd, "@UserId", UserId);

                    var reader = cmd.ExecuteReader();
                    var favoriteArticles = new List<FavoriteArticle>();
                    while (reader.Read())
                    {
                        favoriteArticles.Add(NewArticleFromReader(reader));
                    }

                    reader.Close();

                    return favoriteArticles;
                }
            }

        }



        public void AddFavoriteArticle(FavoriteArticle favoriteArticle)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO FavoriteArticle ( ArticleId, UserId ) 
                                                OUPUT INSERTED.Id
                                                  VALUES ( @FavoriteArticleId, @UserId ) 
                                        ;";
                    cmd.Parameters.AddWithValue("@FavoriteArticleId", favoriteArticle.ArticleId);
                    cmd.Parameters.AddWithValue("@UserId", favoriteArticle.UserId);

                    favoriteArticle.Id = (int)cmd.ExecuteScalar();
                }
            }
        }




        public void DeleteFavoriteArticle(int UserId, int favoriteArticleId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM FavoriteArticle WHERE Id = @favoriteArticleId and UserId = @UserId;";

                    cmd.Parameters.AddWithValue("@favoriteArticleId", favoriteArticleId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
