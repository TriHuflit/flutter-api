using Flutter.Backend.Common.Constains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Flutter.Backend.Api.Controllers
{
    [Route("api/brand")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.MANAGER)]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }


        [HttpGet]
        [Route("details/{CategoryId}")]
        public async Task<IActionResult> GetDetailByCategory(string CategoryId)
        {
            var result = await _brandService.GetDetailBrandByIdCategoryAsync(CategoryId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBrandAsync()
        {
            var result = await _brandService.GetAllBrandAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("all/{CategoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBrandByCategoryIdAsync(string CategoryId)
        {
            var result = await _brandService.GetAllBrandByIdCategoryAsync(CategoryId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBrandAsync(BaseBrandRequest request)
        {
            var result = await _brandService.CreateBrandAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateBrandsAsync(UpdateBrandRequest request)
        {
            var result = await _brandService.UpdateBrandsAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{BrandId}")]
        public async Task<IActionResult> DeleteBrandAsync(string BrandId)
        {
            var result = await _brandService.DeleteBrandAsync(BrandId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
