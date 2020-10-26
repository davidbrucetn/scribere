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
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration) { }

        private Category NewArticleFromReader(SqlDataReader reader)
        {
            Category category = new Category()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                Type = DbUtils.GetString(reader, "Type")

            };
            return category;
        }

        public List<Category> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Type FROM Category ORDER BY Type;";
                    var reader = cmd.ExecuteReader();
                    var categories = new List<Category>();
                    while (reader.Read())
                    {
                        categories.Add(NewArticleFromReader(reader));
                    }

                    reader.Close();

                    return categories;
                }
            }

        }

        public Category GetCategoryById(int categoryId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Type FROM Category WHERE Id = @categoryId;";
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    var reader = cmd.ExecuteReader();

                    Category category = null;
                    if (reader.Read())
                    {
                        category = NewArticleFromReader(reader);
                    }
                    reader.Close();
                    return category;
                }
            }
        }

        public void AddCategory(Category category)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Category ( CategoryId, Type ) 
                                                OUPUT INSERTED.Id
                                                  VALUES ( @CategoryId, @Type ) 
                                        ;";
                    cmd.Parameters.AddWithValue("@CategoryId", category.Id);
                    cmd.Parameters.AddWithValue("@Type", category.Type);

                    category.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateCategory(Category category)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Category
                                           SET Type = @Type
                                            WHERE Id = @CategoryId";

                    cmd.Parameters.AddWithValue("@CategoryId", category.Id);
                    cmd.Parameters.AddWithValue("@Type", category.Type);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteCategory(int categoryId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Category WHERE Id = @categoryId;";

                    cmd.Parameters.AddWithValue("@categoryId", categoryId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
