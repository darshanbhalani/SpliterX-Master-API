using Npgsql;
using SpliterX_API.Models;

namespace API_SpliterX.DataAccess
{
    public class RoomDataAccess
    {
        private string? _connectionString;
        private IConfiguration _configuration;
        public RoomDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration["DBConfigurations:ConnectionString"]!;
        }
        public dynamic getRoomSatistics(long roomId)
        {
            dynamic response = new System.Dynamic.ExpandoObject();
            return response;
        }

        public dynamic getRoomSatisticsOfUser(long roomId, long userId)
        {
            dynamic response = new System.Dynamic.ExpandoObject();
            return response;
        }

        public dynamic createRoom(RoomCreateRequest data)
        {
            dynamic response = new System.Dynamic.ExpandoObject();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM room_create(@in_adminid, @in_name, @in_description);", connection))
                {
                    command.Parameters.AddWithValue("in_adminid", data.AdminId);
                    command.Parameters.AddWithValue("in_name", data.Name);
                    command.Parameters.AddWithValue("in_description", data.Description);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.success = reader.GetBoolean(0);
                            response.message = reader.GetString(1);
                        }
                    }
                }
            }
            return response;
        }

        public dynamic fetchAllRooms(long userId)
        {
            dynamic response = new System.Dynamic.ExpandoObject();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM room_fetchall(@in_id);", connection))
                {
                    command.Parameters.AddWithValue("in_id", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        var rooms = new List<RoomFetchAllResponse>();
                        while (reader.Read())
                        {
                            rooms.Add(new RoomFetchAllResponse
                            {
                                Id = reader.GetInt64(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                AdminName = reader.GetString(3),
                                AdminId = reader.GetInt64(4),
                                CreatedOn = reader.GetDateTime(5),
                                TotalMembers = reader.GetInt64(6)
                            });
                            response.success = true;
                            response.message = "Data successfully fetched.";
                        }
                        response.data = rooms;
                    }
                }
            }
            return response;
        }

        public dynamic deleteRoom(long roomId)
        {
            dynamic response = new System.Dynamic.ExpandoObject();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM room_delete(@in_id);", connection))
                {
                    command.Parameters.AddWithValue("in_id", roomId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.success = reader.GetBoolean(0);
                            response.message = reader.GetString(1);
                        }
                    }
                }
            }
            return response;
        }


        public dynamic updateRoom(RoomUpdateRequest data)
        {
            dynamic response = new System.Dynamic.ExpandoObject();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM room_update(@in_id, @in_roomname, @in_description, @in_adminid);", connection))
                {
                    command.Parameters.AddWithValue("in_id", data.RoomId);
                    command.Parameters.AddWithValue("in_roomname", string.IsNullOrEmpty(data.Name) ? (object)DBNull.Value : data.Name);
                    command.Parameters.AddWithValue("in_description", string.IsNullOrEmpty(data.Description) ? (object)DBNull.Value : data.Description);
                    command.Parameters.AddWithValue("in_adminid", data.AdminId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.success = reader.GetBoolean(0);
                            response.message = reader.GetString(1);
                        }
                    }
                }
            }
            return response;
        }
        public dynamic addMember(RoomAddMemberRequest data)
        {
            dynamic response = new System.Dynamic.ExpandoObject();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM room_addmember(@in_id, @in_roomid);", connection))
                {
                    command.Parameters.AddWithValue("in_id", data.UserId);
                    command.Parameters.AddWithValue("in_roomid", data.RoomId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.success = reader.GetBoolean(0);
                            response.message = reader.GetString(1);
                        }
                    }
                }
            }
            return response;
        }

        public dynamic removeMember(RoomRemoveMemberRequest data)
        {
            dynamic response = new System.Dynamic.ExpandoObject();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM room_removemember(@in_id, @in_roomid);", connection))
                {
                    command.Parameters.AddWithValue("in_id", data.UserId);
                    command.Parameters.AddWithValue("in_roomid", data.RoomId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.success = reader.GetBoolean(0);
                            response.message = reader.GetString(1);
                        }
                    }
                }
            }
            return response;
        }

        public dynamic changeAdmin(long adminId, long roomId)
        {
            dynamic response = new System.Dynamic.ExpandoObject();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM room_changeadmin(@in_id, @in_roomid);", connection))
                {
                    command.Parameters.AddWithValue("in_id", adminId);
                    command.Parameters.AddWithValue("in_roomid", roomId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.success = reader.GetBoolean(0);
                            response.message = reader.GetString(1);
                        }
                    }
                }
            }
            return response;
        }

    }
}
