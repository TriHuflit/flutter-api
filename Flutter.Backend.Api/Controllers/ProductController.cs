using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Flutter.Backend.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
        public ProductController(IProductServices productServices)
        {
            _productService = productServices;
        }



        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProduct([Required] CreateRequestProduct request)
        {
            var result = await  _productService.CreateProductAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateProduct(UpdateRequestProduct request)
        {
            var result = await _productService.UpdateProductAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteProduct(string request)
        {
            var result =  await _productService.DeleteProductAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> SearchProduct(SearchRequestProduct request)
        {
            var result = await _productService.SearchProductAsync(request);

            return Ok(result);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("details/{productId}")]
        public async Task<IActionResult> GetProduct(string productId)
        {
            var result = await _productService.GetProductAsync(productId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
