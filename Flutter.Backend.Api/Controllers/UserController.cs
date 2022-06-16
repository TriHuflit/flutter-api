using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Flutter.Backend.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.USER)]
        [Route("/update-info")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoUser>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateInfoUserAsync(UpdateUserRequest request)
        {
            var result = await _userService.UpdateInfoUserAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.USER)]
        [Route("/update-avatar")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoUser>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAvatarUserAsync(UpdateAvatarRequest request)
        {
            var result = await _userService.UpdateAvatarAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.USER)]
        [Route("/update-password")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoUser>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePasswordUserAsync(UpdatePasswordRequest request)
        {
            var result = await _userService.UpdatePassWordAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
