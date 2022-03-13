using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using Flutter.Backend.Service.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using static Flutter.Backend.Common.Contains.MessageResConstain;
using Flutter.Backend.Common.Contains;

namespace Flutter.Backend.Service.Services
{
    public class ProductService : GenericErrorTextService, IProductServices
    {
        private readonly IProductRespository _productRespository;
        private readonly IMapper _mapper;

        private readonly IUploadImageService _uploadImageService;

        public ProductService(IProductRespository productRespository,
            IMapper mapper,
            IUploadImageService uploadImageService,
            IMessageService messageService) : base(messageService)
        {
            
            _productRespository = productRespository; 
            _uploadImageService = uploadImageService;
            _mapper = mapper;          
           
        }

        /// <summary>
        /// Add product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<DtoProduct>> CreateProductAsync(CreateRequestProduct request)
        {
            var result = new AppActionResultMessage<DtoProduct>();

            if (!ObjectId.TryParse(request.CategoryId, out ObjectId objidCategory))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.CategoryId));
            }

            if (!ObjectId.TryParse(request.BrandId, out ObjectId objidBrand))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.BrandId));
            }

            if (!ObjectId.TryParse(request.CrytalId, out ObjectId objidCrytal))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.CrytalId));
            }

            if (!ObjectId.TryParse(request.WaterProofId, out ObjectId objidWaterProof))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.WaterProofId));
            }

            if (!ObjectId.TryParse(request.AblertId, out ObjectId objidAblert))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.AblertId));
            }

            var newProduct = new Product
            {
                CategoryId = objidCategory,
                BrandId = objidBrand,
                CrytalId = objidCrytal,
                AblertId = objidAblert,
                WaterProofId = objidWaterProof,
                Machine = request.Machine,
                Feature = request.Feature,
                Name = request.Name,
                Description = request.Description,
                IsShow = request.IsShow,
            };

            if (string.IsNullOrEmpty(request.Thumbnail))
            {
                return await BuildError(result,ERR_MSG_EMPTY_DATA, nameof(request.Thumbnail));
            }
            var validateImage = await _uploadImageService.UploadImage(request.Thumbnail);

            if(!validateImage.IsSuccess)
            {
                return await BuildError(result,validateImage.Message);
            }

            newProduct.Thumbnail = validateImage.Data.ToString();

            newProduct.SetFullInfo("6215d37df635e2f104e1839a", "administator@gmail.com");

            _productRespository.Add(newProduct);
            

            return await BuildResult(result,newProduct.Id.ToString(),MSG_SAVE_SUCCESSFULLY);
        }

        /// <summary>
        /// Updates product
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<DtoProduct>> UpdateProductAsync(UpdateRequestProduct request)
        {
            var result = new AppActionResultMessage<DtoProduct>();

            if (!ObjectId.TryParse(request.Id, out ObjectId objProductId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.Id));
            }

            if (!ObjectId.TryParse(request.CategoryId, out ObjectId objCategoryId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.CategoryId));
            }

            if (!ObjectId.TryParse(request.BrandId, out ObjectId objBrandId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.BrandId));
            }

            if (!ObjectId.TryParse(request.CrytalId, out ObjectId objidCrytal))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.CrytalId));
            }

            if (!ObjectId.TryParse(request.WaterProofId, out ObjectId objidWaterProof))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.WaterProofId));
            }

            if (!ObjectId.TryParse(request.AblertId, out ObjectId objidAblert))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.AblertId));
            }

            var product = await _productRespository.Get(p => p.Id == objProductId);


   








            var dtoProduct = _mapper.Map<Product, DtoProduct>(product);

            return await BuildResult(result,dtoProduct, MSG_UPDATE_SUCCESSFULLY);
        }

        /// <summary>
        /// Deletes product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<string>> DeleteProductAsync (string productId)
        {
            var result = new AppActionResultMessage<string>();

            if(!ObjectId.TryParse(productId,out ObjectId objProductId))
            {
                return await BuildError(result,ERR_MSG_ID_ISVALID_FORMART,nameof(productId));
            }

            var product = await _productRespository.Get(p => p.Id == objProductId);
            if(product == null)
            {
                return await BuildError(result,ERR_MSG_EMPTY_DATA,nameof(product));
            }

            product.IsShow = ProductConstain.DELETE;

            product.SetFullInfo("6215d37df635e2f104e1839a", "administator@gmail.com");
            _productRespository.Update(product);

            return await BuildResult(result, MSG_DELETE_SUCCESSFULLY);
        }

        /// <summary>
        /// Gets product by ID.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<DtoProduct>> GetProductAsync (string request)
        {
            var result = new AppActionResultMessage<DtoProduct>();
            
            if(!ObjectId.TryParse(request, out ObjectId objProductId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request));
            }
            var product = await _productRespository.Get(p => p.Id == objProductId);

            var dtoproduct = _mapper.Map<Product,DtoProduct>(product);

            return await BuildResult(result,dtoproduct, MSG_FIND_SUCCESSFULLY);
            
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        public async  Task<AppActionResultMessage<IList<DtoProduct>>> GetAllProductAsync()
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
        public Task<AppActionResultMessage<IList<DtoProduct>>> SearchProductAsync(SearchRequestProduct request)
        {
            throw new System.NotImplementedException();
        }


        #region private method
       
        #endregion private method
    }
}
