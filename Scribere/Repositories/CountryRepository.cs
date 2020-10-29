using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scribere.Models;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Scribere.Utils;


namespace Scribere.Repositories
{
    public class CountryRepository : BaseRepository, ICountryRepository
    {
        public CountryRepository(IConfiguration configuration) : base(configuration) { }

        private Country NewCountryFromReader(SqlDataReader reader)
        {
            Country country = new Country()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                Name = DbUtils.GetString(reader, "Name")

            };
            return country;
        }

        public List<Country> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Country ORDER BY Name;";
                    var reader = cmd.ExecuteReader();
                    var countries = new List<Country>();
                    while (reader.Read())
                    {
                        countries.Add(NewCountryFromReader(reader));
                    }

                    reader.Close();

                    return countries;
                }
            }

        }

        public Country GetCountryById(int countryId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Country WHERE Id = @countryId;";
                    cmd.Parameters.AddWithValue("@countryId", countryId);
                    var reader = cmd.ExecuteReader();

                    Country country = null;
                    if (reader.Read())
                    {
                        country = NewCountryFromReader(reader);
                    }
                    reader.Close();
                    return country;
                }
            }
        }

        public void AddCountry(Country country)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Country ( Name ) 
                                                OUPUT INSERTED.ID
                                                  VALUES ( @Name ) 
                                        ;";
                    cmd.Parameters.AddWithValue("@Name", country.Name);

                    country.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateCountry(Country country)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Country
                                           SET Name = @Name
                                            WHERE Id = @CountryId";

                    cmd.Parameters.AddWithValue("@CountryId", country.Id);
                    cmd.Parameters.AddWithValue("@Name", country.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteCountry(int countryId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Country WHERE Id = @countryId;";

                    cmd.Parameters.AddWithValue("@countryId", countryId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
