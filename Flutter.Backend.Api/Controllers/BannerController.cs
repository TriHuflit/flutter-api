using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Api.Controllers
{
    [Route("api/banner")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private IBannerService _bannerSerivce;
        public BannerController(IBannerService bannerSerivce)
        {
            _bannerSerivce = bannerSerivce;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateBannerAsync(BaseBannerRequest request)
        {
            var result = await _bannerSerivce.CreateBannerAsync(request);
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
        public async Task<IActionResult> UpdateBannerAsync(UpdateBannerRequest request)
        {
            var result = await _bannerSerivce.UpdateBannerAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpDelete]
        [Route("delete/{bannerId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBannerAsync(string bannerId)
        {
            var result = await _bannerSerivce.DeleteBannerAsync(bannerId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoBanner>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoBanner>>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetAllBannerAsync()
        {
            var result = await _bannerSerivce.GetAllBannerAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("mobile/all")]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoBanner>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoBanner>>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBannerMobileAsync()
        {
            var result = await _bannerSerivce.GetAllBannerMobileAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("details/{bannerId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoBanner>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoBanner>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetBannerAsync(string bannerId)
        {
            var result = await _bannerSerivce.GetBannerAsync(bannerId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("mobile/details/{bannerId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoVoucher>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoVoucher>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailVoucherMobileAsync(string bannerId)
        {
            var result = await _bannerSerivce.GetBannerMobileAsync(bannerId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
