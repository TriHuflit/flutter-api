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
        private readonly IValidationService _validationService;

        public ProductService(IProductRespository productRespository,
            IMapper mapper,
            IValidationService validationService,
            IMessageService messageService) : base(messageService)
        {
            _productRespository = productRespository;   
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<AppActionResultMessage<DtoProduct>> Add(CreateProduct request)
        {
            var result = new AppActionResultMessage<DtoProduct>();
            if (!_validationService.ValidateEmailFormat(request.Name))
            {
                return await BuildError(result,ERR_MSG_EMAIL_ISVALID_FORMART, nameof(request.Name));
            }


            var newProduct = new Product
            {
                Name = request.Name,
            };
            _productRespository.Add(newProduct);

            var dtoProduct = _mapper.Map<Product,DtoProduct>(newProduct);

            return await BuildResult(result,dtoProduct,MSG_SAVE_SUCCESSFULLY);
        }

        public async  Task<AppActionResultMessage<IList<DtoProduct>>> GetAll()
        {
            var result = new AppActionResultMessage<IList<DtoProduct>>();
           
            var product = _productRespository.GetAll();
            if( product == null)
            {
               return await BuildError(result,ERR_MSG_PRODUCTS_NOT_FOUND);
            }
           
            var dtoProduct = _mapper.Map<IEnumerable<Product>,List<DtoProduct>>(product);
            return await BuildResult(result, dtoProduct, MSG_FIND_SUCCESSFULLY);
        }
    }
}
