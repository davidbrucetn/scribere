using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Scribere.Models;
using Scribere.Utils;
using Microsoft.Data.SqlClient;

namespace Scribere.Repositories
{
    public class UserDataRepository : BaseRepository, IUserDataRepository
    {
        public UserDataRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserData> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT u.id, u.FirebaseUserId,  u.NameFirst, u.NameLast, u.Pseudonym, u.Email,u.City, u.State, 
                                               u.CountryId, c.Name,
                                               u.Created_at, u.UserLevelId, u.IsActive,
                                               ut.Level AS UserLevelName,
                                               ui.ImageUrl, ui.Id as UserImageId, u.AllowMessaging
                                          FROM UserData u
                                     LEFT JOIN Country c ON c.Id = u.CountryId
                                     LEFT JOIN UserLevel ut ON u.UserLevelId = ut.id
                                     LEFT JOIN UserImage ui ON u.Id = ui.UserId
                                         WHERE IsActive = 1
                                      ORDER BY u.Pseudonym";

                    var reader = cmd.ExecuteReader();

                    var users = new List<UserData>();

                    while (reader.Read())
                    {
                        users.Add(new UserData()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                            Email = DbUtils.GetString(reader, "Email"),
                            NameFirst = DbUtils.GetString(reader, "NameFirst"),
                            NameLast = DbUtils.GetString(reader, "NameLast"),
                            City = DbUtils.GetString(reader, "City"),
                            State = DbUtils.GetString(reader, "State"),
                            CountryId = DbUtils.GetInt(reader, "CountryId"),
                            Pseudonym = DbUtils.GetString(reader, "Pseudonym"),
                            Created_at = reader.GetDateTime(reader.GetOrdinal("Created_at")),
                            UserLevelId = DbUtils.GetInt(reader, "UserLevelId"),
                            IsActive = DbUtils.GetInt(reader, "IsActive"),
                            AllowMessaging = DbUtils.GetInt(reader, "AllowMessaging"),
                            UserLevel = new UserLevel()
                            {
                                Id = DbUtils.GetInt(reader, "UserLevelId"),
                                Level = DbUtils.GetString(reader, "UserLevelName")
                            },
                            UserImage = new UserImage()
                            {
                                Id = DbUtils.GetInt(reader, "UserImageId"),
                                UserId = DbUtils.GetInt(reader, "Id"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                            },
                            Country = new Country()
                            {
                                Id = DbUtils.GetInt(reader, "CountryId"),
                                Name = DbUtils.GetString(reader, "Name")
                            },
                        });
                    }

                    reader.Close();
                    return users;
                }
            }
        }


        public List<UserData> GetDeactivated()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT u.id, u.FirebaseUserId,  u.NameFirst, u.NameLast, u.Pseudonym, u.Email,u.City, u.State, 
                                               u.CountryId, c.Name,
                                               u.Created_at, u.UserLevelId, u.IsActive,
                                               ut.Level AS UserLevelName,
                                               ui.ImageUrl, ui.Id as UserImageId, u.AllowMessaging
                                          FROM UserData u
                                             LEFT JOIN Country c ON c.Id = u.CountryId
                                             LEFT JOIN UserLevel ut ON u.UserLevelId = ut.id
                                             LEFT JOIN UserImage ui ON u.Id = ui.UserId                                         
                                        WHERE IsActive = 0
                                      ORDER BY u.Pseudonym";
                    var reader = cmd.ExecuteReader();

                    var users = new List<UserData>();

                    while (reader.Read())
                    {
                        users.Add(new UserData()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                            Email = DbUtils.GetString(reader, "Email"),
                            NameFirst = DbUtils.GetString(reader, "NameFirst"),
                            NameLast = DbUtils.GetString(reader, "NameLast"),
                            City = DbUtils.GetString(reader, "City"),
                            State = DbUtils.GetString(reader, "State"),
                            CountryId = DbUtils.GetInt(reader, "CountryId"),
                            Pseudonym = DbUtils.GetString(reader, "Pseudonym"),
                            Created_at = reader.GetDateTime(reader.GetOrdinal("Created_at")),
                            UserLevelId = DbUtils.GetInt(reader, "UserLevelId"),
                            IsActive = DbUtils.GetInt(reader, "IsActive"),
                            AllowMessaging = DbUtils.GetInt(reader, "AllowMessaging"),
                            UserLevel = new UserLevel()
                            {
                                Id = DbUtils.GetInt(reader, "UserLevelId"),
                                Level = DbUtils.GetString(reader, "UserLevelName")
                            },
                            UserImage = new UserImage()
                            {
                                Id = DbUtils.GetInt(reader, "UserImageId"),
                                UserId = DbUtils.GetInt(reader, "Id"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                            },
                            Country = new Country()
                            {
                                Id = DbUtils.GetInt(reader, "CountryId"),
                                Name = DbUtils.GetString(reader, "Name")
                            },
                        });
                    }

                    reader.Close();
                    return users;
                }
            }
        }

        public UserData GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirebaseUserId,  u.NameFirst, u.NameLast, u.Pseudonym, u.Email,u.City, u.State, 
                                               u.CountryId, c.Name,
                                               u.Created_at, u.UserLevelId, u.IsActive,
                                               ut.Level AS UserLevelName,
                                               ui.ImageUrl, ui.Id as UserImageId, u.AllowMessaging
                                          FROM UserData u
                                     LEFT JOIN Country c ON c.Id = u.CountryId
                                     LEFT JOIN UserLevel ut ON u.UserLevelId = ut.id
                                     LEFT JOIN UserImage ui ON u.Id = ui.UserId
                                         WHERE u.id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    UserData UserData = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        UserData = new UserData()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                            Email = DbUtils.GetString(reader, "Email"),
                            NameFirst = DbUtils.GetString(reader, "NameFirst"),
                            NameLast = DbUtils.GetString(reader, "NameLast"),
                            City = DbUtils.GetString(reader, "City"),
                            State = DbUtils.GetString(reader, "State"),
                            CountryId = DbUtils.GetInt(reader, "CountryId"),
                            Pseudonym = DbUtils.GetString(reader, "Pseudonym"),
                            Created_at = reader.GetDateTime(reader.GetOrdinal("Created_at")),
                            UserLevelId = DbUtils.GetInt(reader, "UserLevelId"),
                            IsActive = DbUtils.GetInt(reader, "IsActive"),
                            AllowMessaging = DbUtils.GetInt(reader, "AllowMessaging"),
                            UserLevel = new UserLevel()
                            {
                                Id = DbUtils.GetInt(reader, "UserLevelId"),
                                Level = DbUtils.GetString(reader, "UserLevelName")
                            },
                            UserImage = new UserImage()
                            {
                                Id = DbUtils.GetInt(reader, "UserImageId"),
                                UserId = DbUtils.GetInt(reader, "Id"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                            },
                            Country = new Country()
                            {
                                Id = DbUtils.GetInt(reader, "CountryId"),
                                Name = DbUtils.GetString(reader, "Name")
                            },
                        };
                    }

                    reader.Close();

                    return UserData;
                }
            }
        }

        public UserData GetByFirebaseUserId(string firebaseUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT u.id, u.FirebaseUserId,  u.NameFirst, u.NameLast, u.Pseudonym, u.Email,u.City, u.State, 
                                               u.CountryId, c.Name,
                                               u.Created_at, u.UserLevelId, u.IsActive,
                                               ut.Level AS UserLevelName,
                                               ui.ImageUrl, ui.Id as UserImageId, u.AllowMessaging
                                          FROM UserData u
                                     LEFT JOIN Country c ON c.Id = u.CountryId
                                     LEFT JOIN UserLevel ut ON u.UserLevelId = ut.id
                                     LEFT JOIN UserImage ui ON u.Id = ui.UserId
                         WHERE FirebaseUserId = @FirebaseuserId";

                    DbUtils.AddParameter(cmd, "@FirebaseUserId", firebaseUserId);

                    UserData UserData = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        UserData = new UserData()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                            Email = DbUtils.GetString(reader, "Email"),
                            NameFirst = DbUtils.GetString(reader, "NameFirst"),
                            NameLast = DbUtils.GetString(reader, "NameLast"),
                            City = DbUtils.GetString(reader, "City"),
                            State = DbUtils.GetString(reader, "State"),
                            CountryId = DbUtils.GetInt(reader, "CountryId"),
                            Pseudonym = DbUtils.GetString(reader, "Pseudonym"),
                            Created_at = reader.GetDateTime(reader.GetOrdinal("Created_at")),
                            UserLevelId = DbUtils.GetInt(reader, "UserLevelId"),
                            IsActive = DbUtils.GetInt(reader, "IsActive"),
                            AllowMessaging = DbUtils.GetInt(reader, "AllowMessaging"),
                            UserLevel = new UserLevel()
                            {
                                Id = DbUtils.GetInt(reader, "UserLevelId"),
                                Level = DbUtils.GetString(reader, "UserLevelName")
                            },
                            Country = new Country()
                            {
                                Id = DbUtils.GetInt(reader, "CountryId"),
                                Name = DbUtils.GetString(reader, "Name")
                            }

                        };
                        if (DbUtils.IsNotDbNull(reader, "UserImageId"))
                        {
                            UserData.UserImage = new UserImage()
                            {
                                Id = DbUtils.GetInt(reader, "UserImageId"),
                                UserId = DbUtils.GetInt(reader, "Id"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                            };
                        };

                    
                    }
                    reader.Close();

                    return UserData;
                }
            }
        }

        //Soft delete, sends user to "deactivated" page
        public void DeleteUser(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DECLARE @Admins INT
                            SELECT @Admins = Count(*)
                            FROM UserData
                            WHERE UserLevelId = 1 AND IsActive = 1
                            IF @Admins = 1 AND (SELECT UserLevelId FROM UserData WHERE id = @id) = 1
                                THROW 51000, 'There must be at least 1 admin', 1
                            ELSE
                                UPDATE UserData
                                SET IsActive = @IsActive
                                WHERE id = @id";

                    cmd.Parameters.AddWithValue("@IsActive", 1);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ReactivateUser(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE UserData
                                           SET IsActive = @IsActive
                                        WHERE id = @id";

                    cmd.Parameters.AddWithValue("@IsActive", 0);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Add(UserData UserData)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserData (FirebaseUserId, NameFirst, NameLast, Pseudonym, 
                                                                 Email, Created_at, UserLevelId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@FirebaseUserId, @NameFirst, @NameLast, @Pseudonym, 
                                                @Email, @Created_at, @UserLevelId)";
                    DbUtils.AddParameter(cmd, "@FirebaseUserId", UserData.FirebaseUserId);
                    DbUtils.AddParameter(cmd, "@NameFirst", UserData.NameFirst);
                    DbUtils.AddParameter(cmd, "@NameLast", UserData.NameLast);
                    DbUtils.AddParameter(cmd, "@Pseudonym", UserData.Pseudonym);
                    DbUtils.AddParameter(cmd, "@Email", UserData.Email);
                    DbUtils.AddParameter(cmd, "@Created_at", UserData.Created_at);
                    DbUtils.AddParameter(cmd, "@UserLevelId", UserData.UserLevelId);

                    UserData.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateUser(UserData user)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DECLARE @Admins INT
                        SELECT @Admins = Count(*)
                        FROM UserData
                        WHERE UserLevelId = 1 AND IsActive = 0
                        IF @Admins = 1 
                            IF (SELECT UserLevelId FROM UserData WHERE id = @id) = 1 AND @UserLevelId = 2
                                THROW 51000, 'There must be at least 1 admin', 1
                            ELSE
                                UPDATE UserData
	                            SET UserLevelId = @UserLevelId	   
                                WHERE Id = @id                              
                        ELSE
                            UPDATE UserData
	                        SET UserLevelId = @UserLevelId	   
                            WHERE Id = @id ";

                    /* IF there is only one admin in database 
                           IF editing an admin and changing their UserLevel to Author
                               THROW an error
                           ELSE
                               update user
                       ELSE
                           update user */

                    cmd.Parameters.AddWithValue("@UserLevelId", user.UserLevelId);
                    cmd.Parameters.AddWithValue("@id", user.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
