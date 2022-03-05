using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.DAL.Implementations;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRespository _productRespository;

        public ProductServices(IProductRespository productRespository)
        {
            _productRespository = productRespository;
        }

        public Product Add(CreateProduct product)
        {

            var newProduct = new Product
            {
                Name = product.Name,
            };
            _productRespository.Add(newProduct);

            return newProduct;
        }
    }
}
