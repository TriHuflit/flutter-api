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
        public ActionResult CreateProduct(CreateProduct request)
        {
           var result =  _productService.Add(request);

           return Ok(result.Result);
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var result = _productService.GetAll().Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
