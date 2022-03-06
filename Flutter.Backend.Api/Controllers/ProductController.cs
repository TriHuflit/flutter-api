using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult CreateProduct(CreateRequestProduct request)
        {
            var result =  _productService.Add(request).Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        public ActionResult UpdateProduct(UpdateRequestProduct request)
        {
            var result = _productService.Update(request).Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete")]
        public ActionResult DeleteProduct(string request)
        {
            var result = _productService.Delete(request).Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("search")]
        public ActionResult SearchProduct(SearchRequestProduct request)
        {
            var result = _productService.Search(request).Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllProducts()
        {
            var result = _productService.GetAll().Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("details/{productId}")]
        public IActionResult GetProduct(string productId)
        {
            var result = _productService.Get(productId).Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
