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
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration configuration) : base(configuration) { }

        private Tag NewTagFromReader(SqlDataReader reader)
        {
            Tag tag = new Tag()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                Title = DbUtils.GetString(reader, "Title")

            };
            return tag;
        }

        public List<Tag> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Title FROM Tag ORDER BY Title;";
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

        public Tag GetTagById(int tagId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Title FROM Tag WHERE Id = @tagId;";
                    cmd.Parameters.AddWithValue("@tagId", tagId);
                    var reader = cmd.ExecuteReader();

                    Tag tag = null;
                    if (reader.Read())
                    {
                        tag = NewTagFromReader(reader);
                    }
                    reader.Close();
                    return tag;
                }
            }
        }

        public void AddTag(Tag tag)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Tag ( TagId, Title ) 
                                                OUTPUT INSERTED.ID
                                                  VALUES ( @TagId, @Title ) 
                                        ;";
                    cmd.Parameters.AddWithValue("@TagId", tag.Id);
                    cmd.Parameters.AddWithValue("@Title", tag.Title);

                    tag.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Tag
                                           SET Title = @Title
                                            WHERE Id = @TagId";

                    cmd.Parameters.AddWithValue("@TagId", tag.Id);
                    cmd.Parameters.AddWithValue("@Title", tag.Title);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteTag(int tagId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Tag WHERE Id = @tagId;";

                    cmd.Parameters.AddWithValue("@tagId", tagId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
