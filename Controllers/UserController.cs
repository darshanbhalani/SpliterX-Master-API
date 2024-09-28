using API_SpliterX.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpliterX_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpliterX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserDataAccess UDA;
        public UserController(IConfiguration configuration)
        {
            UDA = new UserDataAccess(configuration);
        }

        [HttpPost("LogIn")]
        public IActionResult Login([FromBody] LoginRequest data)
        {
            var response = UDA.logIn(data.PhoneNumberOrEmail, data.Password);
            if (response == null) {
                return BadRequest();
            }
            else
            {
                if (response!.success)
                {
                    return Ok(new
                    {
                        success = true,
                        message = response.message!,
                        data = new {userId = response.userId!,token = response.token!},
                        error = ""
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        message = "",
                        data = "",
                        error = response.message!
                    });
                }
            }
        }


        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] SignupRequest data)
        {
            var response = UDA.signUp(data);
            if (response == null)
            {
                return BadRequest();
            }
            else
            {
                if (response!.success)
                {
                    return Ok(new
                    {
                        success = true,
                        message = response.message!,
                        data = new { userId = response.userId!, token = response.token! },
                        error = ""
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        message = "",
                        data = "",
                        error = response.message!
                    });
                }
            }
        }

        [Authorize]
        [HttpGet("GetUserDetails")]
        public IActionResult GetUserDetails(long userId)
        {
            var response = UDA.getUserDetails(userId);
            if (response == null)
            {
                return BadRequest();
            }
            else
            {
                if (response!.success)
                {
                    return Ok(new
                    {
                        success = true,
                        message = response.message!,
                        data = response.data,
                        error = ""
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        message = "",
                        data = "",
                        error = response.message!
                    });
                }
            }
        }

        
        [HttpPost("UpdateUserDetails")]
        public IActionResult UpdateUserDetails(UserUpdateModel data)
        {
            var response = UDA.updateUserDetails(data);
            if (response == null)
            {
                return BadRequest();
            }
            else
            {
                if (response!.success)
                {
                    return Ok(new
                    {
                        success = true,
                        message = response.message!,
                        data = new { userId = response.userId!, token = response.token! },
                        error = ""
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        message = "",
                        data = "",
                        error = response.message!
                    });
                }
            }
        }
    }
}
