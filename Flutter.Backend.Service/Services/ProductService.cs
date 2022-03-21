using AutoMapper;
using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class ProductService : GenericErrorTextService, IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IClassifyProductRepository _classifyProductRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IWaterProofRepository _waterProofRepository;
        private readonly IMapper _mapper;

        private readonly IUploadImageService _uploadImageService;

        public ProductService(IProductRepository productRepository,
            IMapper mapper,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            IWaterProofRepository waterProofRepository,
            IClassifyProductRepository classifyProductRepository,
            IUploadImageService uploadImageService,
            IMessageService messageService) : base(messageService)
        {

            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _waterProofRepository = waterProofRepository;
            _classifyProductRepository = classifyProductRepository;
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
                return await BuildError(result, validateResult.Message);
            }
            var productInfo = validateResult.Data;

            var newProduct = new Product
            {
                CategoryId = productInfo.CategoryId,
                BrandId = productInfo.BrandId,
                Crytal = request.Crytal,
                Ablert = request.Ablert,
                WaterProofId = productInfo.WaterProofId,
                Machine = request.Machine,
                Feature = request.Feature,
                Name = request.Name,
                Description = request.Description,
                Guarantee = request.Guarantee,
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

            newProduct.Thumbnail = validateImage.Data;
            newProduct.SetCreatedInFo("6215d37df635e2f104e1839a", "administator@gmail.com");

            _productRepository.Add(newProduct);

            foreach (var classifyProduct in request.classifyProducts)
            {
                if (string.IsNullOrEmpty(classifyProduct.Name))
                {
                    return await BuildError(result, ERR_MSG_EMPTY_DATA, nameof(classifyProduct.Name));
                }

                var newClassifyProduct = new ClassifyProduct
                {
                    Name = classifyProduct.Name,
                    OriginalPrice = classifyProduct.OriginalPrice,
                    PromotionPrice = classifyProduct.PromotionPrice,
                    IsShow = classifyProduct.IsShow,
                    Stock = classifyProduct.Stock,
                    ProductId = newProduct.Id,
                    
                };

                if (string.IsNullOrEmpty(classifyProduct.Image))
                {
                    return await BuildError(result, ERR_MSG_EMPTY_DATA, nameof(classifyProduct.Image));
                }

                var validateImageClassifyProduct = await _uploadImageService.UploadImage(classifyProduct.Image);
                if (!validateImageClassifyProduct.IsSuccess)
                {
                    return await BuildError(result, validateImageClassifyProduct.Message);
                }
                newClassifyProduct.Image = validateImageClassifyProduct.Data;
                newClassifyProduct.SetFullInfo("6215d37df635e2f104e1839a", "administator@gmail.com");

                _classifyProductRepository.Add(newClassifyProduct);

                if (newProduct.FromPrice == 0 && newProduct.ToPrice == 0)
                {
                    newProduct.FromPrice = newClassifyProduct.OriginalPrice;
                    newProduct.ToPrice = newClassifyProduct.OriginalPrice;
                }

                if (newProduct.FromPrice > newClassifyProduct.OriginalPrice)
                {
                    newProduct.FromPrice = newClassifyProduct.OriginalPrice;
                }

                if (newProduct.ToPrice < newClassifyProduct.OriginalPrice)
                {
                    newProduct.ToPrice = newClassifyProduct.OriginalPrice;
                }
                _productRepository.Update(newProduct, p => p.Id == newProduct.Id);
            }

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

            var product = await _productRepository.Get(p => p.Id == objProductId);
            if (product == null)
            {
                return await BuildError(result, ERR_MSG_PRODUCT_NOT_FOUND, nameof(product));
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

            if (!string.IsNullOrEmpty(request.Crytal))
            {
                product.Crytal = request.Crytal;
            }

            if (!string.IsNullOrEmpty(request.Ablert))
            {
                product.Ablert = request.Ablert;
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

            if (!string.IsNullOrEmpty(request.Thumbnail) && request.Thumbnail != product.Thumbnail)
            {
                var validateImage = await _uploadImageService.UploadImage(request.Thumbnail);
                if (!validateImage.IsSuccess)
                {
                    return await BuildError(result, validateImage.Message);
                }

                product.Thumbnail = validateImage.Data.ToString();
            }

            product.SetUpdatedInFo("6215d37df635e2f104e1839a", "administator@gmail.com");
            _productRepository.Update(product, p => p.Id == product.Id);

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

            var product = await _productRepository.Get(p => p.Id == objProductId);
            if (product == null)
            {
                return await BuildError(result, ERR_MSG_EMPTY_DATA, nameof(product));
            }

            product.IsShow = ProductConstain.DELETE;

            product.SetFullInfo("6215d37df635e2f104e1839a", "administator@gmail.com");
            _productRepository.Update(product, p => p.Id == product.Id);

            return await BuildResult(result, MSG_DELETE_SUCCESSFULLY);
        }

        /// <summary>
        /// Gets product by ID.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<DtoProductDetail>> GetProductAsync(string request)
        {
            var result = new AppActionResultMessage<DtoProductDetail>();

            if (!ObjectId.TryParse(request, out ObjectId objProductId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request));
            }

            var product = await _productRepository.Get(p => p.Id == objProductId);
            if (product == null)
            {
                return await BuildResult(result, ERR_MSG_PRODUCT_NOT_FOUND, nameof(product));
            }

            var dtoproduct = _mapper.Map<Product, DtoProductDetail>(product);

            var classifyProducts = await _classifyProductRepository.FindByAsync(c => c.ProductId == product.Id);

            if (classifyProducts != null)
            {
                var dtoClassifyProduct = _mapper.Map<IEnumerable<ClassifyProduct>, IEnumerable<DtoClassifyProduct>>(classifyProducts);
                dtoproduct.ClassifyProducts = dtoClassifyProduct;
            }

            var category = await _categoryRepository.Get(c => c.Id == product.CategoryId);
            if (category == null)
                dtoproduct.CategoryName = ProductDetailConstain.CategoryName;
            else dtoproduct.CategoryName = category.Name;

            var brand = await _brandRepository.Get(b => b.Id == product.BrandId);
            if (brand == null)
                dtoproduct.BrandName = ProductDetailConstain.BrandName;
            else dtoproduct.BrandName = brand.Name;

            var waterProof = await _waterProofRepository.Get(w => w.Id == product.WaterProofId);
            if (waterProof == null)
                dtoproduct.WaterProofName = ProductDetailConstain.WaterProofName;
            else dtoproduct.WaterProofName = waterProof.Name;

            return await BuildResult(result, dtoproduct, MSG_FIND_SUCCESSFULLY);

        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<IEnumerable<DtoProduct>>> GetAllProductAsync(PaginationRequest request)
        {
            var result = new AppActionResultMessage<IEnumerable<DtoProduct>>();
            int pageIndex = request.PageIndex > 1 ? request.PageIndex : 1;
            int pageSize = request.PageSize > 10 ? request.PageSize : 10;

            var products = await _productRepository.GetAll();
            if (products == null)
            {
                return await BuildResult(result, ERR_MSG_PRODUCTS_NOT_FOUND);
            }

            // Pagination for Product
            products = products.OrderBy(p => p.Name)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            var dtoProduct = _mapper.Map<IEnumerable<Product>, List<DtoProduct>>(products);
            return await BuildResult(result, dtoProduct, MSG_FIND_SUCCESSFULLY);
        }

        /// <summary>
        /// Search product on website.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<IEnumerable<DtoProduct>>> SearchProductAsync(SearchRequestProduct request)
        {
            var result = new AppActionResultMessage<IEnumerable<DtoProduct>>();
            int pageIndex = request.PageIndex > 1 ? request.PageIndex : 1;
            int pageSize = request.PageSize > 10 ? request.PageSize : 10;
            IEnumerable<Product> products = new List<Product>();
            Category category;
            Brand brand;

            if (string.IsNullOrEmpty(request.KeySearch)
                && string.IsNullOrEmpty(request.CategorySearch)
                && string.IsNullOrEmpty(request.BrandSearch))
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND);
            }

            if (!string.IsNullOrEmpty(request.KeySearch))
            {
                products = await _productRepository.FindByAsync(p => p.Name.ToLower().Contains(request.KeySearch.ToLower())
                && p.IsShow != ProductConstain.DELETE);
            }

            if (!string.IsNullOrEmpty(request.CategorySearch))
            {
                category = await _categoryRepository.GetAsync(c => c.Name.ToLower().Contains(request.CategorySearch.ToLower()) && c.IsShow != ProductConstain.DELETE);
                if (category != null)
                {
                    products = await _productRepository.FindByAsync(p => p.CategoryId == category.Id);
                }

            }

            if (!string.IsNullOrEmpty(request.BrandSearch))
            {
                brand = await _brandRepository.GetAsync(b => b.Name.ToLower().Contains(request.BrandSearch.ToLower()) && b.IsShow != ProductConstain.DELETE);
                if (brand != null)
                {
                    products = await _productRepository.FindByAsync(p => p.BrandId == brand.Id);

                }

            }

            // Pagination for Product
            products = products.OrderBy(p => p.Name)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();


            var dtoProduct = _mapper.Map<IEnumerable<Product>, IEnumerable<DtoProduct>>(products);

            return await BuildResult(result, dtoProduct, MSG_FIND_SUCCESSFULLY);
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

            if (!ObjectId.TryParse(request.WaterProofId, out ObjectId objWaterProofId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.WaterProofId));
            }


            var brand = await _brandRepository.FindByAsync(x => x.Id == objBrandId);
            if (brand == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(brand));
            }

            var category = await _categoryRepository.FindByAsync(x => x.Id == objCategoryId);
            if (category == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(category));
            }

            var productInfo = new ProductInfo
            {
                CategoryId = objCategoryId,
                BrandId = objBrandId,
                WaterProofId = objWaterProofId
            };

            return result.BuildResult(productInfo);
        }
        #endregion private method

        #region private class method
        private class ProductInfo
        {
            public ObjectId CategoryId { get; set; }

            public ObjectId BrandId { get; set; }

            public ObjectId WaterProofId { get; set; }
        }
        #endregion private class method
    }
}
