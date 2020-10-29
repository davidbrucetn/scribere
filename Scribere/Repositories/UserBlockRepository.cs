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
    public class UserBlockRepository : BaseRepository, IUserBlockRepository
    {
        public UserBlockRepository(IConfiguration configuration) : base(configuration) { }

        private UserBlock NewUserBlockFromReader(SqlDataReader reader)
        {
            UserBlock userBlock = new UserBlock()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                SourceUserId = DbUtils.GetInt(reader, "SourceUserId"),
                BlockedUserId = DbUtils.GetInt(reader, "BlockedUserId")

            };
            return userBlock;
        }

        public List<UserBlock> GetAll(int sourceUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Type FROM UserBlock Where SourceUserId = @SourceUserId ORDER BY BlockedUserId;";
                    DbUtils.AddParameter(cmd, "@SourceUserId", sourceUserId);
                    var reader = cmd.ExecuteReader();
                    var userBlocks = new List<UserBlock>();
                    while (reader.Read())
                    {
                        userBlocks.Add(NewUserBlockFromReader(reader));
                    }

                    reader.Close();

                    return userBlocks;
                }
            }

        }



        public void AddUserBlock(UserBlock userBlock)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserBlock ( BlockedUserId, SourceUserId ) 
                                                OUPUT INSERTED.ID
                                                  VALUES ( @BlockedUserId, @SourceUserId ) 
                                        ;";
                    cmd.Parameters.AddWithValue("@BlockedUserId", userBlock.BlockedUserId);
                    cmd.Parameters.AddWithValue("@SourceUserId", userBlock.SourceUserId);

                    userBlock.Id = (int)cmd.ExecuteScalar();
                }
            }
        }




        public void DeleteUserBlock(int SourceUserId, int userBlockId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM UserBlock WHERE BlockedUserId = @BlockedUserId and SourceUserId = @SourceUserId;";

                    cmd.Parameters.AddWithValue("@BlockedUserId", userBlockId);
                    cmd.Parameters.AddWithValue("@SourceUserId", SourceUserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
