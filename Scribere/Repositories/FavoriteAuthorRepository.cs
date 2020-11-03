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

        private FavoriteAuthor NewFavoriteAuthorFromReader(SqlDataReader reader)
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
                        favoriteAuthors.Add(NewFavoriteAuthorFromReader(reader));
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
                    cmd.CommandText = @"INSERT INTO FavoriteAuthor (FavoriteUserId, SourceUserId) 
                                                OUTPUT INSERTED.ID
                                         VALUES ( @FavoriteAuthorId, @SourceUserId );";
                    DbUtils.AddParameter(cmd,"@FavoriteAuthorId", favoriteAuthor.FavoriteUserId);
                    DbUtils.AddParameter(cmd,"@SourceUserId", favoriteAuthor.SourceUserId);

                    favoriteAuthor.Id = (int)cmd.ExecuteScalar();
                }
            }
        }




        public void DeleteFavoriteAuthor(int SourceUserId, int favoriteUserId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM FavoriteAuthor WHERE FavoriteUserId = @favoriteUserId and SourceUserId = @SourceUserId;";

                    DbUtils.AddParameter(cmd,"@favoriteUserId", favoriteUserId);
                    DbUtils.AddParameter(cmd,"@SourceUserId", SourceUserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
