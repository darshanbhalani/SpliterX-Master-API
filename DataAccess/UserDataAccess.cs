using Npgsql;
using API_SpliterX.Shared;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NpgsqlTypes;
using SpliterX_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace API_SpliterX.DataAccess
{
    public class UserDataAccess
    {
        private string? _connectionString;
        private IConfiguration _configuration;
        public UserDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration["DBConfigurations:ConnectionString"]!;
        }
        public dynamic logIn(string username, string password)
        {
            dynamic response = new System.Dynamic.ExpandoObject();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(DB.LogIn + "(@username,@password)", connection))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.success = reader.GetBoolean(0);
                            response.message = reader.GetString(1);
                            response.userId = !reader.IsDBNull(2) ? reader.GetInt64(2) : (long?)null;
                            response.token = generateJwtToken();
                        }
                    }
                }
            }
            return response;
        }

        public dynamic signUp(SignupRequest signupRequest)
        {
            dynamic response = new System.Dynamic.ExpandoObject();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(DB.SignUp + "(@phonenumber, @firstname, @lastname, @email, @birthdate, @gender, @password);", connection))
                {
                    command.Parameters.AddWithValue("phonenumber", signupRequest.PhoneNumber);
                    command.Parameters.AddWithValue("firstname", signupRequest.FirstName);
                    command.Parameters.AddWithValue("lastname", signupRequest.LastName);
                    command.Parameters.AddWithValue("email", signupRequest.Email);
                    command.Parameters.Add(new NpgsqlParameter("birthdate", NpgsqlDbType.Date) { Value = DateOnly.Parse(signupRequest.BirthDate) });
                    command.Parameters.AddWithValue("gender", signupRequest.Gender);
                    command.Parameters.AddWithValue("password", signupRequest.Password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.success = reader.GetBoolean(0);
                            response.message = reader.GetString(1);
                            response.userId = !reader.IsDBNull(2) ? reader.GetInt64(2) : (long?)null;
                            response.token = generateJwtToken();
                        }
                    }
                }
            }
            return response;
        }

        public dynamic getUserDetails(long id)
        {
            dynamic response = new System.Dynamic.ExpandoObject();
            response.success = false;
            UserDetails userDetails = new UserDetails();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM USER_FETCHDETAILS(@userid)", conn))
                {
                    cmd.Parameters.AddWithValue("userid", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.success = true;
                            response.message = "Data successfully fetched.";
                            response.data = new UserDetails
                            {
                                Id = reader.GetInt64(0),
                                PhoneNumber = reader.GetString(1),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3),
                                Email = reader.GetString(4),
                                BirthDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                Gender = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return response;
        }

        public dynamic updateUserDetails(UserUpdateModel data)
        {
            dynamic response = new System.Dynamic.ExpandoObject();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM USER_UPDATE", conn))
                {
                    cmd.Parameters.AddWithValue("in_userid", data.UserId);
                    cmd.Parameters.AddWithValue("in_phonenumber", (object)data.PhoneNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("in_firstname", (object)data.FirstName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("in_lastname", (object)data.LastName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("in_email", (object)data.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("in_birthdate", (object)data.BirthDate! ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("in_gender", (object)data.Gender ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            response.success = reader.GetBoolean(0);
                            response.message = reader.GetString(1);
                            response.userId = !reader.IsDBNull(2) ? reader.GetInt64(2) : (long?)null;
                            response.token = generateJwtToken();
                        }
                    }
                }
            }
            return response;
        }
        private string generateJwtToken()
        {
            var creationTime = DateTime.UtcNow;
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, "testuser"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)creationTime).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
