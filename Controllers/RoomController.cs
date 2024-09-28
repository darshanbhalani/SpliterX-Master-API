using API_SpliterX.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SpliterX_API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpliterX_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private RoomDataAccess RDA;
        public RoomController(IConfiguration configuration)
        {
            RDA = new RoomDataAccess(configuration);
        }

        [HttpPost("CreateRoom")]
        public ActionResult CreateRoom([FromBody] RoomCreateRequest roomCreateRequest)
        {
            var response = RDA.createRoom(roomCreateRequest);
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
                        data = "",
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

        [HttpGet("fetchallrooms/{userId}")]
        public ActionResult FetchAllRooms(long userId)
        {
            var response = RDA.fetchAllRooms(userId);
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

        [HttpDelete("DeleteRoom/{roomId}")]
        public ActionResult DeleteRoom(long roomId)
        {
            var response = RDA.deleteRoom(roomId);
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
                        data = "",
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

        [HttpPut("UpdateRoomDetails")]
        public ActionResult UpdateRoomDetails(RoomUpdateRequest roomUpdateRequest)
        {
            var response = RDA.updateRoom(roomUpdateRequest);
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
                        data = "",
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

        [HttpPost("AddMember")]
        public ActionResult AddMember([FromBody] RoomAddMemberRequest roomAddMemberRequest)
        {
            var response = RDA.addMember(roomAddMemberRequest);
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
                        data = "",
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

        [HttpPost("RemoveMember")]
        public ActionResult RemoveMember([FromBody] RoomRemoveMemberRequest roomRemoveMemberRequest)
        {
            var response = RDA.removeMember(roomRemoveMemberRequest);
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
                        data = "",
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


        [HttpPut("ChangeAdmin")]
        public ActionResult ChangeAdmin(long adminId, long roomId)
        {
            var response = RDA.changeAdmin(adminId, roomId);
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
                        data = "",
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
