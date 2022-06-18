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
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }


        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateNewsAsync(BaseNewsRequest request)
        {
            var result = await _newsService.CreateNewsAsync(request);
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
        public async Task<IActionResult> UpdateNewsAsync(UpdateNewsRequest request)
        {
            var result = await _newsService.UpdateNewsAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpDelete]
        [Route("delete/{newsId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteNewsAsync(string newsId)
        {
            var result = await _newsService.DeleteNewsAsync(newsId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoNews>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoNews>>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetAllNewsAsync()
        {
            var result = await _newsService.GetAllNewsAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("mobile/all")]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoNews>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoNews>>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNewsMobileAsync()
        {
            var result = await _newsService.GetAllNewsMobileAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("details/{newsId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoNews>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoNews>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetNewsAsync(string newsId)
        {
            var result = await _newsService.GetNewsAsync(newsId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("mobile/details/{newsId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoNews>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoNews>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailVoucherMobileAsync(string newsId)
        {
            var result = await _newsService.GetNewsMobileAsync(newsId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
