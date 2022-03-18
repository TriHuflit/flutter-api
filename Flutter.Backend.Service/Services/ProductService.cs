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
        private readonly ICategoryRespository _categoryRespository;
        private readonly IBrandRespository _brandRespository;
        private readonly IMapper _mapper;

        private readonly IUploadImageService _uploadImageService;

        public ProductService(IProductRespository productRespository,
            IMapper mapper,
            ICategoryRespository categoryRespository,
            IBrandRespository brandRespository,
            IUploadImageService uploadImageService,
            IMessageService messageService) : base(messageService)
        {

            _productRespository = productRespository;
            _categoryRespository = categoryRespository;
            _brandRespository = brandRespository;
            _uploadImageService = uploadImageService;
            _mapper = mapper;

        }

        /// <summary>
        /// Add product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<string>> CreateProductAsync(CreateRequestProduct request)
        {
            var result = new AppActionResultMessage<string>();

            var validateResult = await ValidateProductAsync(request);
            if (!validateResult.IsSuccess)
            {
                
            }
            var productInfo = validateResult.Data;

            var newProduct = new Product
            {
                CategoryId = productInfo.CategoryId,
                BrandId = productInfo.BrandId,
                CrytalId = productInfo.CrytalId,
                AblertId = productInfo.AblertId,
                WaterProofId = productInfo.WaterProofId,
                Machine = request.Machine,
                Feature = request.Feature,
                Name = request.Name,
                Description = request.Description,
                IsShow = request.IsShow,
            };

            if (string.IsNullOrEmpty(request.Thumbnail))
            {
                return await BuildError(result, ERR_MSG_EMPTY_DATA, nameof(request.Thumbnail));
            }
            var validateImage = await _uploadImageService.UploadImage(request.Thumbnail);

            if (!validateImage.IsSuccess)
            {
                return await BuildError(result, validateImage.Message);
            }

            newProduct.Thumbnail = validateImage.Data.ToString();
            newProduct.SetCreatedInFo("6215d37df635e2f104e1839a", "administator@gmail.com");

            _productRespository.Add(newProduct);


            return await BuildResult(result, newProduct.Id.ToString(), MSG_SAVE_SUCCESSFULLY);
        }

        /// <summary>
        /// Updates product
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<string>> UpdateProductAsync(UpdateRequestProduct request)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(request.Id, out ObjectId objProductId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.Id));
            }

            var product = await _productRespository.Get(p => p.Id == objProductId);
            if (product == null)
            {
                return await BuildError(result, ERR_MSG_EMPTY_DATA, nameof(product));
            }

            var validateResult = await ValidateProductAsync(request);
            if (!validateResult.IsSuccess)
            {
                return result.BuildError(validateResult.Message);
            }

            var productInfo = validateResult.Data;

            if (!string.IsNullOrEmpty(request.CategoryId))
            {
                product.CategoryId = productInfo.CategoryId;
            }

            if (!string.IsNullOrEmpty(request.BrandId))
            {
                product.BrandId = productInfo.BrandId;
            }

            if (!string.IsNullOrEmpty(request.CrytalId))
            {
                product.CrytalId = productInfo.CrytalId;
            }

            if (!string.IsNullOrEmpty(request.AblertId))
            {
                product.AblertId = productInfo.AblertId;
            }

            if (!string.IsNullOrEmpty(request.WaterProofId))
            {
                product.WaterProofId = productInfo.WaterProofId;
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                product.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                product.Description = request.Description;
            }

            if (request.Feature.Count > 0)
            {
                product.Feature = request.Feature;
            }

            if (!string.IsNullOrEmpty(request.Machine))
            {
                product.Machine = request.Machine;
            }

            if (!string.IsNullOrEmpty(request.MadeIn))
            {
                product.MadeIn = request.MadeIn;
            }

            if (request.IsShow.HasValue)
            {
                product.IsShow = request.IsShow;
            }

            if (request.Guarantee.HasValue)
            {
                product.Guarantee = request.Guarantee;
            }

            

            product.SetUpdatedInFo("6215d37df635e2f104e1839a", "administator@gmail.com");
            _productRespository.Update(product);

            return await BuildResult(result, product.Id.ToString(), MSG_UPDATE_SUCCESSFULLY);
        }

        /// <summary>
        /// Deletes product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<string>> DeleteProductAsync(string productId)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(productId, out ObjectId objProductId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(productId));
            }

            var product = await _productRespository.Get(p => p.Id == objProductId);
            if (product == null)
            {
                return await BuildError(result, ERR_MSG_EMPTY_DATA, nameof(product));
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
        public async Task<AppActionResultMessage<DtoProduct>> GetProductAsync(string request)
        {
            var result = new AppActionResultMessage<DtoProduct>();

            if (!ObjectId.TryParse(request, out ObjectId objProductId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request));
            }

            var product = await _productRespository.Get(p => p.Id == objProductId);
            if (product == null)
            {
                return await BuildResult(result, ERR_MSG_PRODUCT_NOT_FOUND, nameof(product));
            }

            var dtoproduct = _mapper.Map<Product, DtoProduct>(product);
            return await BuildResult(result, dtoproduct, MSG_FIND_SUCCESSFULLY);

        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<IList<DtoProduct>>> GetAllProductAsync()
        {
            var result = new AppActionResultMessage<IList<DtoProduct>>();

            var product = await _productRespository.GetAll();
            if (product == null)
            {
                return await BuildResult(result, ERR_MSG_PRODUCTS_NOT_FOUND);
            }

            var dtoProduct = _mapper.Map<IEnumerable<Product>, List<DtoProduct>>(product);
            return await BuildResult(result, dtoProduct, MSG_FIND_SUCCESSFULLY);
        }

        /// <summary>
        /// Search product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<IList<DtoProduct>>> SearchProductAsync(SearchRequestProduct request)
        {
            var result = new AppActionResultMessage<IList<DtoProduct>>();
            int pageIndex = request.PageIndex;
            int pageSize = request.PageSize;
            string keySearch = request.KeySearch.ToLower();
            var categories = await _categoryRespository.FindByAsync(c => c.Name.ToLower().Contains(keySearch));
            if(categories != null)
            {
               
            }
            var brand = await _brandRespository.FindByAsync(b => b.Name.ToLower().Contains(keySearch));

            var product = await _productRespository.FindByAsync(p => p.Name.ToLower().Contains(keySearch));

            

            return await BuildResult(result, MSG_FIND_SUCCESSFULLY);
        }


        #region private method
        private async Task<AppActionResultMessage<ProductInfo>> ValidateProductAsync(BaseRequestProduct request)
        {
            var result = new AppActionResultMessage<ProductInfo>();
           
            if (!ObjectId.TryParse(request.CategoryId, out ObjectId objCategoryId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.CategoryId));
            }

            if (!ObjectId.TryParse(request.BrandId, out ObjectId objBrandId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.BrandId));
            }

            if (!ObjectId.TryParse(request.CrytalId, out ObjectId objCrytalId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.CrytalId));
            }

            if (!ObjectId.TryParse(request.WaterProofId, out ObjectId objWaterProofId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.WaterProofId));
            }

            if (!ObjectId.TryParse(request.AblertId, out ObjectId objAblertId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.AblertId));
            }


            //var brand = await _brandRespository.FindByAsync(x => x.Id == objBrandId);
            //if(brand == null)
            //{
            //    return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(brand));
            //}

            //var category = await _categoryRespository.FindByAsync(x => x.Id == objCategoryId);
            //if (category == null)
            //{
            //    return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(category));
            //}

            var productInfo = new ProductInfo
            {
                CategoryId = objCategoryId,
                BrandId = objBrandId,
                AblertId = objAblertId,
                WaterProofId = objWaterProofId,
                CrytalId = objCrytalId,
            };

            return  result.BuildResult(productInfo);
        }
        #endregion private method

        #region private class method
        private class ProductInfo
        {
            public ObjectId CategoryId { get; set; }

            public ObjectId BrandId { get; set; }

            public ObjectId CrytalId { get; set; }

            public ObjectId WaterProofId { get; set; }

            public ObjectId AblertId { get; set; }
        }
        #endregion private class method
    }
}
