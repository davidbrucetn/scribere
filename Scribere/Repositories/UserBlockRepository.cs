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

        public List<UserBlock> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, SourceUserId, BlockedUserId FROM UserBlock ORDER BY BlockedUserId;";

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
                                                OUTPUT INSERTED.ID
                                                  VALUES ( @BlockedUserId, @SourceUserId );";
                    DbUtils.AddParameter(cmd,"@BlockedUserId", userBlock.BlockedUserId);
                    DbUtils.AddParameter(cmd,"@SourceUserId", userBlock.SourceUserId);

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

                    DbUtils.AddParameter(cmd,"@BlockedUserId", userBlockId);
                    DbUtils.AddParameter(cmd,"@SourceUserId", SourceUserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
