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
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IUserService _userService;
        public StaffController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.ADMIN)]
        [Route("create")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStaffAsync(CreateStaffRequest request)
        {
            var result = await _userService.CreateStaffAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.ADMIN)]
        [Route("update-role")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRoleAsync(UpdateRoleRequest request)
        {
            var result = await _userService.UpdateRoleAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.ADMIN)]
        [Route("block/{idStaff}")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BlockStaffAsync(string idStaff)
        {
            var result = await _userService.BlockStaffAsync(idStaff);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.ADMIN)]
        [Route("get-all-staff")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoStaff>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoStaff>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllStaffAsync()
        {
            var result = await _userService.GettAllStaffAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.ADMIN)]
        [Route("get-details-staff/{idStaff}")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoStaff>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoStaff>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GettDetailStaffAsync(string idStaff)
        {
            var result = await _userService.GettDetailStaffAsync(idStaff);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }



    }
}
