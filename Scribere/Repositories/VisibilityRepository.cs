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
    public class VisibilityRepository : BaseRepository, IVisibilityRepository
    {
        public VisibilityRepository(IConfiguration configuration) : base(configuration) { }

        private Visibility NewVisibilityFromReader(SqlDataReader reader)
        {
            Visibility visibility = new Visibility()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                Type = DbUtils.GetString(reader, "Type")

            };
            return visibility;
        }

        public List<Visibility> GetAllVisibilities()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Type FROM Visibility ORDER BY Type;";
                    var reader = cmd.ExecuteReader();
                    var visibilities = new List<Visibility>();
                    while (reader.Read())
                    {
                        visibilities.Add(NewVisibilityFromReader(reader));
                    }

                    reader.Close();

                    return visibilities;
                }
            }

        }

        public Visibility GetVisibilityById(int visibilityId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Type FROM Visibility WHERE Id = @visibilityId;";
                    DbUtils.AddParameter(cmd,"@visibilityId", visibilityId);
                    var reader = cmd.ExecuteReader();

                    Visibility visibility = null;
                    if (reader.Read())
                    {
                        visibility = NewVisibilityFromReader(reader);
                    }
                    reader.Close();
                    return visibility;
                }
            }
        }

        public void AddVisibility(Visibility visibility)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Visibility ( Type ) 
                                                OUTPUT INSERTED.ID
                                                  VALUES ( @Type );";
                    DbUtils.AddParameter(cmd,"@Type", visibility.Type);

                    visibility.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateVisibility(Visibility visibility)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Visibility
                                           SET Type = @Type
                                            WHERE Id = @VisibilityId";

                    DbUtils.AddParameter(cmd,"@VisibilityId", visibility.Id);
                    DbUtils.AddParameter(cmd,"@Type", visibility.Type);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteVisibility(int visibilityId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM VisibilityId WHERE Id = @visibilityId;";

                    DbUtils.AddParameter(cmd,"@visibilityId", visibilityId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
