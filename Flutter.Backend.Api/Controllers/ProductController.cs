using Flutter.Backend.Common.Constains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Flutter.Backend.Api.Controllers
{
    /// <summary>
    /// Product Api 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/product")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.MANAGER)]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
        public ProductController(IProductServices productServices)
        {
            _productService = productServices;
        }


        /// <summary>
        /// Creates the product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProduct([Required] CreateProductRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(request);
                }

                var result = await _productService.CreateProductAsync(request);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateProduct([Required] UpdateProductRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(request);
                }

                var result = await _productService.UpdateProductAsync(request);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{productId}")]       
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(productId);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Searchs the product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchProduct(SearchRequestProduct request)
        {
            var result = await _productService.SearchProductAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllProducts([FromQuery] PaginationRequest request)
        {
            
            try
            {
                var result = await _productService.GetAllProductAsync(request);
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all-banner")]
        public async Task<IActionResult> GetAllProductsBanner()
        {

            try
            {
                var result = await _productService.GetAllProductBanner();
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Promotion Product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("promotion")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPromotionProductAsync()
        {

            try
            {
                var result = await _productService.GetProductPromotionAsync();
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("details/{productId}")]
        public async Task<IActionResult> GetProduct(string productId)
        {
            try
            {
                var result = await _productService.GetProductAsync(productId);
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Gets all products for mobile.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("mobile/all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductsMobile([FromQuery] PaginationRequest request)
        {

            try
            {
                var result = await _productService.GetAllProductMobileAsync(request);
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets all products for mobile.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("mobile/related/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsRelatedMobileAsync(string productId)
        {

            try
            {
                var result = await _productService.GetProductRelatedAsync(productId);
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets the product for mobile.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("details/mobile/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductMobile(string productId)
        {
            try
            {
                var result = await _productService.GetProductAsync(productId);
                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
