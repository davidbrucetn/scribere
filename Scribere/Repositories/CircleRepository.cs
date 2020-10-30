using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Scribere.Models;
using Scribere.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scribere.Repositories
{
    public class CircleRepository : BaseRepository, ICircleRepository
    {
        public CircleRepository(IConfiguration configuration) : base(configuration) { }

        private Circle NewCircleFromReader(SqlDataReader reader)
        {
            Circle circle = new Circle()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                UserId = DbUtils.GetInt(reader, "UserId"),
                FriendId = DbUtils.GetInt(reader, "FriendId")

            };
            return circle;
        }

        public List<Circle> GetAllCircles(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, UserId, FriendId FROM Circle Where UserId = @userId ORDER BY FriendId;";

                    DbUtils.AddParameter(cmd, "@userId", userId);

                    var reader = cmd.ExecuteReader();
                    var circles = new List<Circle>();
                    while (reader.Read())
                    {
                        circles.Add(NewCircleFromReader(reader));
                    }

                    reader.Close();

                    return circles;
                }
            }

        }



        public void AddCircle(Circle circle)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Circle (FriendId, UserId) 
                                                OUTPUT INSERTED.ID
                                         VALUES ( @CircleId, @UserId );";
                    cmd.Parameters.AddWithValue("@CircleId", circle.FriendId);
                    cmd.Parameters.AddWithValue("@UserId", circle.UserId);

                    circle.Id = (int)cmd.ExecuteScalar();
                }
            }
        }




        public void DeleteCircle(int userId, int friendId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Circle WHERE FriendId = @friendId and UserId = @UserId;";

                    cmd.Parameters.AddWithValue("@friendId", friendId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
