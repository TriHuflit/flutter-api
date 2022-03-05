using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flutter.Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
        public ProductController(IProductServices productServices)
        {
            _productService = productServices;
        }



        [HttpPost]
        public ActionResult CreateProduct(CreateProduct product)
        {
           var result =  _productService.Add(product);

           return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var result = _productService.GetAll().Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Detail);
            }
            return Ok(result);
        }
    }
}
