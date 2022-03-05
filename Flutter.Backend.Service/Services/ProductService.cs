using static Flutter.Backend.Common.Contains.MessageResContains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flutter.Backend.Service.Models.Dtos;
using AutoMapper;

namespace Flutter.Backend.Service.Services
{
    public class ProductService : GenericErrorTextService<DtoProduct>, IProductServices
    {
        private readonly IProductRespository _productRespository;
        private readonly IMapper _mapper;
       
        public ProductService(IProductRespository productRespository,
            IMapper mapper,
            IMessageService messageService) : base(messageService)
        {
            _productRespository = productRespository;   
            _mapper = mapper;
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

        public async  Task<AppActionResultMessage<IList<DtoProduct>>> GetAll()
        {
            var result = new AppActionResultMessage<IList<DtoProduct>>();
            
            var product = _productRespository.GetAll();
            if( product == null)
            {
               return await BuildError(result,ERR_MSG_NOT_FOUND);
            }
           
            var dtoProduct = _mapper.Map<IEnumerable<Product>,List<DtoProduct>>(product);
            return await BuildResult(result, dtoProduct, ERR_MSG_SUCCESSFULLY);
        }
    }
}
