using static Flutter.Backend.Common.Contains.MessageResContains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flutter.Backend.Service.Models.Dtos;
using AutoMapper;
using MongoDB.Bson;
using Flutter.Backend.Service.Settings;

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

        /// <summary>
        /// Add product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<DtoProduct>> Add(CreateRequestProduct request)
        {
            var result = new AppActionResultMessage<DtoProduct>();
            
            if(!ObjectId.TryParse(request.CategoryID, out ObjectId idCategory))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART,nameof(request.CategoryID));
            }
            if (!ObjectId.TryParse(request.BrandID, out ObjectId idBrand))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.BrandID));
            }


            var newProduct = new Product
            {
                CategoryID = idCategory,
                BrandID = idBrand,
                Name = request.Name,
                Description = request.Description,
                IsShow = request.IsShow,
            };






            newProduct.SetFullInfo("6215d37df635e2f104e1839a", "administator@gmail.com");

            _productRespository.Add(newProduct);
            

            var dtoProduct = _mapper.Map<Product,DtoProduct>(newProduct);

            return await BuildResult(result,dtoProduct,MSG_SAVE_SUCCESSFULLY);
        }

        /// <summary>
        /// Updates product
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<DtoProduct>> Update(UpdateRequestProduct request)
        {
            var result = new AppActionResultMessage<DtoProduct>();

            if (!ObjectId.TryParse(request.Id, out ObjectId idProduct))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.CategoryID));
            }
            if (!ObjectId.TryParse(request.CategoryID, out ObjectId idCategory))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.CategoryID));
            }
            if (!ObjectId.TryParse(request.BrandID, out ObjectId idBrand))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.BrandID));
            }

            var product = _productRespository.Get(idProduct);

            var dtoProduct = _mapper.Map<Product, DtoProduct>(product);

            return await BuildResult(result,dtoProduct, MSG_UPDATE_SUCCESSFULLY);
        }

        /// <summary>
        /// Deletes product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<AppActionResultMessage<DtoProduct>> Delete(string request)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets product by ID.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<DtoProduct>> Get(string request)
        {
            var result = new AppActionResultMessage<DtoProduct>();
            
            if(!ObjectId.TryParse(request, out ObjectId idProduct))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request));
            }
            var product =_productRespository.Get(idProduct);

            var dtoproduct = _mapper.Map<Product,DtoProduct>(product);

            return await BuildResult(result,dtoproduct, MSG_FIND_SUCCESSFULLY);
            
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Search product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<AppActionResultMessage<IList<DtoProduct>>> Search(SearchRequestProduct request)
        {
            throw new System.NotImplementedException();
        }


        #region private method
        private bool isValidUploadImage(string image)
        {

            return true;
        }
        #endregion private method
    }
}
