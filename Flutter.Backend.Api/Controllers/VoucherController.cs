﻿using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Api.Controllers
{
    [Route("api/voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }


        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.MANAGER)]
        public async Task<IActionResult> CreateVoucherAsync(BaseVoucherRequest request)
        {
            var result = await _voucherService.CreateVoucherAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }



        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.MANAGER)]
        public async Task<IActionResult> UpdateVoucherAsync(UpdateVoucherRequest request)
        {
            var result = await _voucherService.UpdateVoucherAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }



        [HttpDelete]
        [Route("delete/{voucherId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.MANAGER)]
        public async Task<IActionResult> DeleteVoucherAsync(string voucherId)
        {
            var result = await _voucherService.DeleteVoucherAsync(voucherId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoVoucher>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoVoucher>>), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.MANAGER)]
        public async Task<IActionResult> GetAllVoucherAsync()
        {
            var result = await _voucherService.GetAllVoucherAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("mobile/all")]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoVoucher>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoVoucher>>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllVoucherForMobileAsync()
        {
            var result = await _voucherService.GetAllVoucherMobileAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("details/{VoucherId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoVoucher>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoVoucher>), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.MANAGER)]
        public async Task<IActionResult> GetDetailVoucherAsync(string VoucherId)
        {
            var result = await _voucherService.GetVoucherAsync(VoucherId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("mobile/details/{VoucherId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoVoucher>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoVoucher>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailVoucherMobileAsync(string VoucherId)
        {
            var result = await _voucherService.GetVoucherMobileAsync(VoucherId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


    }
}
