using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IProductServices
    {
        public Product Add(CreateProduct product);

        public IEnumerable<Product> GetAll();
    }
}
