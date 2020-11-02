using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Scribere.Models;
using Scribere.Utils;
using Microsoft.Data.SqlClient;
using System;

namespace Scribere.Repositories
{
    public class UserDataRepository : BaseRepository, IUserDataRepository
    {
        public UserDataRepository(IConfiguration configuration) : base(configuration) { }


        private UserData NewUserDataFromReader(SqlDataReader reader)
        {
            UserData UserData = null;
            UserImage UserImage = null;

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
                CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                UserLevelId = DbUtils.GetInt(reader, "UserLevelId"),
                IsActive = DbUtils.GetInt(reader, "IsActive"),
                Bio = DbUtils.GetString(reader, "Bio"),
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
                },
            };
            if (DbUtils.IsNotDbNull(reader, "UserImageId"))
            {
                UserImage = new UserImage()
                {
                    Id = DbUtils.GetInt(reader, "UserImageId"),
                    UserId = DbUtils.GetInt(reader, "Id"),
                    ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                };
                UserData.UserImage = UserImage;

            } else
            {
                UserImage = new UserImage()
                {
                    UserId = DbUtils.GetInt(reader, "Id"),
                    ImageUrl = ""
                };
                UserData.UserImage = UserImage;
            };

            return UserData;


        }

        public List<UserData> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT u.id, u.FirebaseUserId,  u.NameFirst, u.NameLast, u.Pseudonym, u.Bio, u.Email,u.City, u.State, 
                                               u.CountryId, c.Name,
                                               u.CreateDate, u.UserLevelId, u.IsActive,
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
                        users.Add(NewUserDataFromReader(reader));
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
                    cmd.CommandText = @"SELECT u.id, u.FirebaseUserId,  u.NameFirst, u.NameLast, u.Pseudonym, u.Bio, u.Email,u.City, u.State, 
                                               u.CountryId, c.Name,
                                               u.CreateDate, u.UserLevelId, u.IsActive,
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
                        users.Add(NewUserDataFromReader(reader));
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
                       SELECT u.id, u.FirebaseUserId,  u.NameFirst, u.NameLast, u.Pseudonym, u.Bio, u.Email,u.City, u.State, 
                                               u.CountryId, c.Name,
                                               u.CreateDate, u.UserLevelId, u.IsActive,
                                               ut.Level AS UserLevelName,
                                               ui.ImageUrl, ui.Id as UserImageId, u.AllowMessaging
                                          FROM UserData u
                                     LEFT JOIN Country c ON c.Id = u.CountryId
                                     LEFT JOIN UserLevel ut ON u.UserLevelId = ut.id
                                     LEFT JOIN UserImage ui ON u.Id = ui.UserId
                                         WHERE u.id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    UserData userData = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userData = NewUserDataFromReader(reader);
                    }

                    reader.Close();
                    return userData;
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
                        SELECT u.id, u.FirebaseUserId,  u.NameFirst, u.NameLast, u.Pseudonym, u.Bio, u.Email,u.City, u.State, 
                                               u.CountryId, c.Name,
                                               u.CreateDate, u.UserLevelId, u.IsActive,
                                               ut.Level AS UserLevelName,
                                               ui.ImageUrl, ui.Id as UserImageId, u.AllowMessaging
                                          FROM UserData u
                                     LEFT JOIN Country c ON c.Id = u.CountryId
                                     LEFT JOIN UserLevel ut ON u.UserLevelId = ut.id
                                     LEFT JOIN UserImage ui ON u.Id = ui.UserId
                         WHERE FirebaseUserId = @FirebaseuserId";

                    DbUtils.AddParameter(cmd, "@FirebaseUserId", firebaseUserId);

                    UserData userData = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userData = NewUserDataFromReader(reader);
                    }

                    reader.Close();
                    return userData;
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
                    cmd.CommandText = @"
                      BEGIN
                        DECLARE @UserImage TABLE (
                            [UserId] INT,
                            [ImageUrl]  VARCHAR(300)
                        )
                                        
                            INSERT INTO UserData (FirebaseUserId, NameFirst, NameLast, Pseudonym, Bio, Email, City, State, CountryId,
                                CreateDate, UserLevelId, IsActive, AllowMessaging )
                            OUTPUT INSERTED.ID, @UserImageUrl INTO @UserImage
                            VALUES (@FirebaseUserId, @NameFirst, @NameLast, @Pseudonym, @Bio, @Email, @City, @State, @CountryId,
                                @CreateDate, @UserLevelId, @IsActive, @AllowMessaging)

                      
                            INSERT INTO UserImage (UserId,ImageUrl) SELECT [UserId],[ImageUrl] FROM @UserImage WHERE [ImageUrl] <> NULL OR [ImageUrl] <> ''
                            
                      
                       
                            SELECT [UserId] from @UserImage                           
                        
                      END;
                        ";
                    DbUtils.AddParameter(cmd, "@FirebaseUserId", UserData.FirebaseUserId);
                    DbUtils.AddParameter(cmd, "@NameFirst", UserData.NameFirst);
                    DbUtils.AddParameter(cmd, "@NameLast", UserData.NameLast);
                    DbUtils.AddParameter(cmd, "@Pseudonym", UserData.Pseudonym);
                    DbUtils.AddParameter(cmd, "@Bio", UserData.Bio);
                    DbUtils.AddParameter(cmd, "@Email", UserData.Email);
                    DbUtils.AddParameter(cmd, "@City", UserData.City);
                    DbUtils.AddParameter(cmd, "@State", UserData.State);
                    DbUtils.AddParameter(cmd, "@CountryId", UserData.CountryId);
                    DbUtils.AddParameter(cmd, "@CreateDate", UserData.CreateDate);
                    DbUtils.AddParameter(cmd, "@UserLevelId", UserData.UserLevelId);
                    DbUtils.AddParameter(cmd, "@IsActive", UserData.IsActive);
                    DbUtils.AddParameter(cmd, "@AllowMessaging", UserData.AllowMessaging);
                    DbUtils.AddParameter(cmd, "@UserImageUrl", UserData.UserImage.ImageUrl);


                    UserData.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateUser(UserData userData)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                             UPDATE UserData
	                                SET 
                                      NameFirst = @NameFirst,
                                       NameLast = @NameLast,
                                      Pseudonym = @Pseudonym,
                                          Email = @Email,
                                           City = @City,
                                          State = @State,
                                      CountryId = @CountryId,
                                    UserLevelId = @UserLevelId,
                                       IsActive = @IsActive,
                                 AllowMessaging = @AllowMessaging,
                                            Bio = @Bio
                                    WHERE Id = @id                              
 
                            If @UserImageURL IS NOT NULL
                            BEGIN
                                IF EXISTS(SELECT @Id FROM UserImage WHERE UserImage.UserId = @Id)
	                                BEGIN 
				                        UPDATE UserImage SET ImageUrl=@UserImageURL WHERE UserId = @Id
	                                END
                                ELSE
                                    BEGIN
                                        INSERT INTO UserImage (UserId,ImageUrl) VALUES (@Id, @UserImageURL)
                                    END 
                            END
                    ;";

                    
                    DbUtils.AddParameter(cmd, "@Id", userData.Id);
                    DbUtils.AddParameter(cmd, "@FirebaseUserId", userData.FirebaseUserId);
                    DbUtils.AddParameter(cmd, "@NameFirst", userData.NameFirst);
                    DbUtils.AddParameter(cmd, "@NameLast", userData.NameLast);
                    DbUtils.AddParameter(cmd, "@Pseudonym", userData.Pseudonym);
                    DbUtils.AddParameter(cmd, "@Bio", userData.Bio);
                    DbUtils.AddParameter(cmd, "@Email", userData.Email);
                    DbUtils.AddParameter(cmd, "@City", userData.City);
                    DbUtils.AddParameter(cmd, "@State", userData.State);
                    DbUtils.AddParameter(cmd, "@CountryId", userData.CountryId);
                    DbUtils.AddParameter(cmd, "@CreateDate", userData.CreateDate);
                    DbUtils.AddParameter(cmd, "@UserLevelId", userData.UserLevelId);
                    DbUtils.AddParameter(cmd, "@IsActive", userData.IsActive);
                    DbUtils.AddParameter(cmd, "@AllowMessaging", userData.AllowMessaging);
                    DbUtils.AddParameter(cmd, "@UserImageURL", userData.UserImage.ImageUrl);
                    
                    


                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
