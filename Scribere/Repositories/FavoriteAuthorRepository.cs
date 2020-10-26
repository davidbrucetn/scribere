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
    public class FavoriteAuthorRepository : BaseRepository, IFavoriteAuthorRepository
    {
        public FavoriteAuthorRepository(IConfiguration configuration) : base(configuration) { }

        private FavoriteAuthor NewArticleFromReader(SqlDataReader reader)
        {
            FavoriteAuthor favoriteAuthor = new FavoriteAuthor()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                SourceUserId = DbUtils.GetInt(reader, "SourceUserId"),
                FavoriteUserId = DbUtils.GetInt(reader, "FavoriteUserId")

            };
            return favoriteAuthor;
        }

        public List<FavoriteAuthor> GetAll(int sourceUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, SourceUserId, FavoriteUserId FROM FavoriteAuthor Where SourceUserId = @sourceUserId ORDER BY FavoriteUserId;";

                    DbUtils.AddParameter(cmd, "@sourceUserId", sourceUserId);

                    var reader = cmd.ExecuteReader();
                    var favoriteAuthors = new List<FavoriteAuthor>();
                    while (reader.Read())
                    {
                        favoriteAuthors.Add(NewArticleFromReader(reader));
                    }

                    reader.Close();

                    return favoriteAuthors;
                }
            }

        }



        public void AddFavoriteAuthor(FavoriteAuthor favoriteAuthor)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO FavoriteAuthor ( FavoriteUserId, SourceUserId ) 
                                                OUPUT INSERTED.Id
                                                  VALUES ( @FavoriteAuthorId, @SourceUserId ) 
                                        ;";
                    cmd.Parameters.AddWithValue("@FavoriteAuthorId", favoriteAuthor.FavoriteUserId);
                    cmd.Parameters.AddWithValue("@SourceUserId", favoriteAuthor.SourceUserId);

                    favoriteAuthor.Id = (int)cmd.ExecuteScalar();
                }
            }
        }




        public void DeleteFavoriteAuthor(int SourceUserId, int favoriteAuthorId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM FavoriteAuthor WHERE Id = @favoriteAuthorId and SourceUserId = @SourceUserId;";

                    cmd.Parameters.AddWithValue("@favoriteAuthorId", favoriteAuthorId);
                    cmd.Parameters.AddWithValue("@SourceUserId", SourceUserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
