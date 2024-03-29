﻿using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Flutter.Backend.Api.Controllers
{
    /// <summary>
    /// Auth Api
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/authenticate")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUserAsync(RegisterRequest request)
        {
            try
            {
                var result = await _authenticateService.RegisterAsync(request);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        [Route("comfirm-email/{userId}")]
        public async Task<IActionResult> ComfirmAccountByEmailAsync(string userId)
        {
            try
            {
                var result = await _authenticateService.ComfirmEmailAsync(userId);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Logins the user for mobile.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUserAsync(AuthendicateRequest request)
        {
            try
            {
                var result = await _authenticateService.AuthendicateUserAsync(request);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        /// <summary>
        /// Logins the admin.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("system-login")]
        public async Task<IActionResult> LoginAdminAsync(AuthendicateRequest request)
        {
            try
            {
                var result = await _authenticateService.AuthendicateAdminAsync(request);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }
            
        /// <summary>
        /// Refreshes the token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                var result = await _authenticateService.RefreshTokenAsync(request.RefreshToken);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("send-email/reset-password")]
        public async Task<IActionResult> SendEmailResetPassAsync(SendEmailResetPassRequest request)
        {
            try
            {
                var result = await _authenticateService.SendEmailResetPassAsync(request);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("comfirm-reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                var result = await _authenticateService.ResetPasswordAsync(request);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
