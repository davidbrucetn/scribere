﻿using Microsoft.Data.SqlClient;
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

        private FavoriteArticle NewFavoriteArticleFromReader(SqlDataReader reader)
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
                    cmd.CommandText = "SELECT Id, UserId, ArticleId FROM FavoriteArticle WHERE UserId = @UserId ORDER BY ArticleId;";

                    DbUtils.AddParameter(cmd, "@UserId", UserId);

                    var reader = cmd.ExecuteReader();
                    var favoriteArticles = new List<FavoriteArticle>();
                    while (reader.Read())
                    {
                        favoriteArticles.Add(NewFavoriteArticleFromReader(reader));
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
                                                OUTPUT INSERTED.ID
                                                  VALUES ( @FavoriteArticleId, @UserId );";
                    DbUtils.AddParameter(cmd,"@FavoriteArticleId", favoriteArticle.ArticleId);
                    DbUtils.AddParameter(cmd,"@UserId", favoriteArticle.UserId);

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
                        DELETE FROM FavoriteArticle WHERE ArticleId = @favoriteArticleId and UserId = @UserId;";

                    DbUtils.AddParameter(cmd,"@favoriteArticleId", favoriteArticleId);
                    DbUtils.AddParameter(cmd,"@UserId", UserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
