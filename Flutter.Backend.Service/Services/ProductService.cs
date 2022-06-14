using AutoMapper;
using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Flutter.Backend.Service.Models.Responses;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private readonly IMapper _mapper;

        private readonly IUploadImageService _uploadImageService;
        private readonly ICurrentUserService _currentUserService;

        public ProductService(IProductRepository productRepository,
            IMapper mapper,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            IClassifyProductRepository classifyProductRepository,
            IUploadImageService uploadImageService,
            ICurrentUserService currentUserService,
            IMessageService messageService) : base(messageService)
        {

            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _classifyProductRepository = classifyProductRepository;
            _uploadImageService = uploadImageService;
            _currentUserService = currentUserService;
            _mapper = mapper;

        }

        /// <summary>
        /// Add product.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<string>> CreateProductAsync(CreateProductRequest request)
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
                WaterProof = request.WaterProof,
                Machine = request.Machine,
                Feature = request.Feature,
                MadeIn = request.MadeIn,
                Name = request.Name,
                Description = request.Description,
                Guarantee = request.Guarantee,
                IsShow = request.IsShow,
            };

            var validateImage = await _uploadImageService.UploadImage(request.Thumbnail);

            if (!validateImage.IsSuccess)
            {
                return await BuildError(result, validateImage.Message);
            }

            newProduct.Thumbnail = validateImage.Data;
            newProduct.SetFullInfor(_currentUserService.UserId, _currentUserService.UserName);

            _productRepository.Add(newProduct);

            foreach (var classifyProduct in request.ClassifyProducts)
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
                newClassifyProduct.SetFullInfor(_currentUserService.UserId, _currentUserService.UserName);

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
        public async Task<AppActionResultMessage<string>> UpdateProductAsync(UpdateProductRequest request)
        {
            var result = new AppActionResultMessage<string>();

            // Update Product
            if (!ObjectId.TryParse(request.Id, out ObjectId objProductId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.Id));
            }

            var product = await _productRepository.Get(p => p.Id == objProductId && p.IsShow != IsShowConstain.DELETE);
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

            if (!string.IsNullOrEmpty(request.WaterProof))
            {
                product.WaterProof = request.WaterProof;
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

            if (IsValidateStatusProduct(request.IsShow))
            {
                product.IsShow = request.IsShow;
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

            product.Guarantee = request.Guarantee;
            var classifyProducts = await _classifyProductRepository.FindByAsync(c => c.ProductId == objProductId
                    && c.IsShow != IsShowConstain.DELETE);
            foreach (var item in classifyProducts)
            {
                item.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
                item.IsShow = IsShowConstain.DELETE;
                _classifyProductRepository.Update(item, i => i.Id == item.Id);
            }


            // Update or Create new classifyProduct
            foreach (var classifyProduct in request.ClassifyProducts)
            {

                if (classifyProduct.Id != null)
                {
                    if (!ObjectId.TryParse(classifyProduct.Id, out ObjectId objClassPro))
                    {
                        return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(classifyProduct.Id));
                    }

                    foreach (var item in classifyProducts)
                    {
                        if (item.Id == objClassPro)
                        {
                            item.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
                            item.IsShow = classifyProduct.IsShow;
                            _classifyProductRepository.Update(item, i => i.Id == objClassPro);
                        }
                    }
                    var oldClassifyProduct = await _classifyProductRepository.GetAsync(o => o.Id == objClassPro
                      && o.ProductId == objProductId && o.IsShow != IsShowConstain.DELETE);

                    if (oldClassifyProduct == null)
                    {
                        return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(oldClassifyProduct));
                    }

                    if (!string.IsNullOrEmpty(classifyProduct.Name))
                    {
                        oldClassifyProduct.Name = classifyProduct.Name;
                    }

                    if (!string.IsNullOrEmpty(classifyProduct.Image) && classifyProduct.Image != oldClassifyProduct.Image)
                    {
                        var validateImage = await _uploadImageService.UploadImage(classifyProduct.Image);
                        if (!validateImage.IsSuccess)
                        {
                            return await BuildError(result, validateImage.Message);
                        }

                        oldClassifyProduct.Image = validateImage.Data.ToString();
                    }

                    if (!IsValidateStatusProduct(classifyProduct.IsShow))
                    {
                        return await BuildError(result, "Trạng thái loại sản phẩm không hợp lệ");
                    }

                    oldClassifyProduct.IsShow = classifyProduct.IsShow;
                    oldClassifyProduct.OriginalPrice = classifyProduct.OriginalPrice;
                    oldClassifyProduct.PromotionPrice = classifyProduct.PromotionPrice;
                    oldClassifyProduct.Stock = classifyProduct.Stock;
                    oldClassifyProduct.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
                    _classifyProductRepository.Update(oldClassifyProduct, o => o.Id == oldClassifyProduct.Id);

                    if (product.FromPrice == 0 && product.ToPrice == 0)
                    {
                        product.FromPrice = oldClassifyProduct.OriginalPrice;
                        product.ToPrice = 0;
                    }

                    if (product.FromPrice > oldClassifyProduct.OriginalPrice)
                    {
                        product.FromPrice = oldClassifyProduct.OriginalPrice;
                    }

                    if (product.ToPrice < oldClassifyProduct.OriginalPrice)
                    {
                        product.ToPrice = oldClassifyProduct.OriginalPrice;
                    }

                }
                else
                {
                    //classifyProduct Create new
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
                        ProductId = objProductId,

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
                    newClassifyProduct.SetFullInfor(_currentUserService.UserId, _currentUserService.UserName);
                    _classifyProductRepository.Add(newClassifyProduct);

                    if (product.FromPrice == 0 && product.ToPrice == 0)
                    {
                        product.FromPrice = newClassifyProduct.OriginalPrice;
                        product.ToPrice = 0;
                    }

                    if (product.FromPrice > newClassifyProduct.OriginalPrice)
                    {
                        product.FromPrice = newClassifyProduct.OriginalPrice;
                    }

                    if (product.ToPrice < newClassifyProduct.OriginalPrice)
                    {
                        product.ToPrice = newClassifyProduct.OriginalPrice;
                    }
                }
            }
            product.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
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

            var product = await _productRepository.Get(p => p.Id == objProductId && p.IsShow != IsShowConstain.DELETE);
            if (product == null)
            {
                return await BuildError(result, ERR_MSG_EMPTY_DATA, nameof(product));
            }

            product.IsShow = IsShowConstain.DELETE;

            product.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _productRepository.Update(product, p => p.Id == product.Id);

            var listClassifyProduct = await _classifyProductRepository.FindByAsync(c => c.ProductId == product.Id && c.IsShow != IsShowConstain.DELETE);
            foreach (var item in listClassifyProduct)
            {
                item.IsShow = IsShowConstain.DELETE;
                item.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
                _classifyProductRepository.Update(item, c => c.Id == item.Id);
            }

            return await BuildResult(result, productId, MSG_DELETE_SUCCESSFULLY);
        }

        /// <summary>
        /// Gets product by ID.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<DtoProductDetail>> GetProductAsync(string productId)
        {
            var result = new AppActionResultMessage<DtoProductDetail>();

            if (!ObjectId.TryParse(productId, out ObjectId objProductId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(productId));
            }

            var product = await _productRepository.Get(p => p.Id == objProductId && p.IsShow != IsShowConstain.DELETE);
            if (product == null)
            {
                return await BuildError(result, ERR_MSG_PRODUCT_NOT_FOUND, nameof(product));
            }

            var dtoproduct = _mapper.Map<Product, DtoProductDetail>(product);

            var classifyProducts = await _classifyProductRepository.FindByAsync(c => c.ProductId == product.Id && c.IsShow != IsShowConstain.DELETE);

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


            return await BuildResult(result, dtoproduct, MSG_FIND_SUCCESSFULLY);

        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<SearchResultData>> GetAllProductAsync(PaginationRequest request)
        {
            var result = new AppActionResultMessage<SearchResultData>();
            int pageIndex = request.PageIndex > 1 ? request.PageIndex : 1;
            int pageSize = request.PageSize > 6 ? request.PageSize : 6;
            int Count = 0;
            IEnumerable<Product> products;
            var dtoProducts = new List<DtoProduct>();
            products = await _productRepository.FindByAsync(p => p.IsShow != IsShowConstain.DELETE);

            Count = products.Count() / pageSize;
            if (products.Count() / pageSize != 0)
            {
                Count++;
            }
            // Pagination for Product
            products = products.OrderBy(p => p.Name).ToList();
            dtoProducts = _mapper.Map<IEnumerable<Product>, List<DtoProduct>>(products);

            foreach (var dtoProduct in dtoProducts)
            {
                var category = await _categoryRepository.GetAsync(c => c.Id == ObjectId.Parse(dtoProduct.CategoryId) && c.IsShow != IsShowConstain.DELETE);
                if (category != null)
                {
                    dtoProduct.CategoryName = category.Name;
                }

                var brand = await _brandRepository.GetAsync(b => b.Id == ObjectId.Parse(dtoProduct.BrandId) && b.IsShow != IsShowConstain.DELETE);
                if (brand != null)
                {
                    dtoProduct.BrandName = brand.Name;
                }

            }

            var searchResultData = new SearchResultData
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ListProduct = dtoProducts,
            };

            return await BuildResult(result, searchResultData, MSG_FIND_SUCCESSFULLY);
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
            var paramKey = request.KeySearch ?? string.Empty;
            var paramCategory = request.CategorySearch ?? string.Empty;
            var paramBrand = request.BrandSearch ?? string.Empty;
            var paramFilterPrice = request.SortPrice != -1 && request.SortPrice != 1 ?  0 : request.SortPrice ;

            Expression<Func<Product, bool>> UserGuideFilter = ex => string.IsNullOrEmpty(paramKey) || (ex.Name != null && ex.Name.ToLower().Contains(paramKey));
            var finalFilter = Builders<Product>.Filter.Where(UserGuideFilter);


            if (!string.IsNullOrEmpty(paramCategory))
            {
                if (!ObjectId.TryParse(paramCategory, out ObjectId objCategory))
                {
                    return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, paramCategory);
                }
                var categoryFilter = Builders<Product>.Filter.Where(ex => ex.CategoryId == objCategory);
                finalFilter = Builders<Product>.Filter.And(finalFilter, categoryFilter);

            }

            if (!string.IsNullOrEmpty(paramBrand))
            {
                if (!ObjectId.TryParse(paramBrand, out ObjectId objBrand))
                {
                    return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, paramBrand);
                }
                var brandFilter = Builders<Product>.Filter.Where(ex => ex.BrandId == objBrand);
                finalFilter = Builders<Product>.Filter.And(finalFilter, brandFilter);

            }
      
            var products = await _productRepository.FindByAsync(finalFilter);
            // Pagination for Product
            if (paramFilterPrice == 1)
            {
                products = products.OrderBy(p => p.FromPrice)
                      .Skip((pageIndex - 1) * pageSize)
                      .Take(pageSize)
                      .ToList();
            }

            if (paramFilterPrice == -1)
            {
                products = products.OrderByDescending(p => p.FromPrice)
                      .Skip((pageIndex - 1) * pageSize)
                      .Take(pageSize)
                      .ToList();
            }
          
            var dtoProduct = _mapper.Map<IEnumerable<Product>, IEnumerable<DtoProduct>>(products);

            return await BuildResult(result, dtoProduct, MSG_FIND_SUCCESSFULLY);
        }

        /// <summary>
        /// Gets all product mobile asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<SearchResultData>> GetAllProductMobileAsync(PaginationRequest request)
        {
            var result = new AppActionResultMessage<SearchResultData>();
            int pageIndex = request.PageIndex > 1 ? request.PageIndex : 1;
            int pageSize = request.PageSize > 10 ? request.PageSize : 10;
            IEnumerable<Product> products;
            var dtoProducts = new List<DtoProduct>();

            products = await _productRepository.FindByAsync(p => p.IsShow == IsShowConstain.ACTIVE);
            // Pagination for Product
            products = products.OrderBy(p => p.Name)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            dtoProducts = _mapper.Map<IEnumerable<Product>, List<DtoProduct>>(products);

            foreach (var dtoProduct in dtoProducts)
            {
                var category = await _categoryRepository.GetAsync(c => c.Id == ObjectId.Parse(dtoProduct.CategoryId) && c.IsShow != IsShowConstain.DELETE);
                if (category != null)
                {
                    dtoProduct.CategoryName = category.Name;
                }

                var brand = await _brandRepository.GetAsync(b => b.Id == ObjectId.Parse(dtoProduct.BrandId) && b.IsShow != IsShowConstain.DELETE);
                if (brand != null)
                {
                    dtoProduct.BrandName = brand.Name;
                }
            }

            var searchResultData = new SearchResultData
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ListProduct = dtoProducts
            };

            return await BuildResult(result, searchResultData, MSG_FIND_SUCCESSFULLY);
        }

        /// <summary>
        /// Gets the product mobile asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<DtoProductDetail>> GetProductMobileAsync(string productId)
        {
            var result = new AppActionResultMessage<DtoProductDetail>();

            if (!ObjectId.TryParse(productId, out ObjectId objProductId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(productId));
            }

            var product = await _productRepository.Get(p => p.Id == objProductId && p.IsShow == IsShowConstain.ACTIVE);
            if (product == null)
            {
                return await BuildResult(result, ERR_MSG_PRODUCT_NOT_FOUND, nameof(product));
            }

            var dtoproduct = _mapper.Map<Product, DtoProductDetail>(product);

            var classifyProducts = await _classifyProductRepository.FindByAsync(c => c.ProductId == product.Id && c.IsShow == IsShowConstain.ACTIVE);

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


            return await BuildResult(result, dtoproduct, MSG_FIND_SUCCESSFULLY);
        }

        #region private method
        private async Task<AppActionResultMessage<ProductInfo>> ValidateProductAsync(BaseProductRequest request)
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
            };

            return result.BuildResult(productInfo);
        }

        private bool IsValidateStatusProduct(int isShow)
        {
            return isShow == IsShowConstain.ACTIVE || isShow == IsShowConstain.INACTIVE;
        }

        public async Task<AppActionResultMessage<SearchResultData>> GetProductPromotionAsync()
        {
            var result = new AppActionResultMessage<SearchResultData>();
            var products = new List<Product>();
            for (int i = 0; i < 4; i++)
            {
                var product = (from p in await _productRepository.GetAll()
                               join c in await _classifyProductRepository.GetAll()
                               on p.Id equals c.ProductId
                               where p.IsShow == IsShowConstain.ACTIVE
                               && c.PromotionPrice != 0 && products.All(r => r.Id != p.Id)
                               select p).FirstOrDefault();

                products.Add(product);
            }


            var dtoProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<DtoProduct>>(products);

            foreach (var dtoProduct in dtoProducts)
            {
                var category = await _categoryRepository.GetAsync(c => c.Id == ObjectId.Parse(dtoProduct.CategoryId) && c.IsShow != IsShowConstain.DELETE);
                if (category != null)
                {
                    dtoProduct.CategoryName = category.Name;
                }

                var brand = await _brandRepository.GetAsync(b => b.Id == ObjectId.Parse(dtoProduct.BrandId) && b.IsShow != IsShowConstain.DELETE);
                if (brand != null)
                {
                    dtoProduct.BrandName = brand.Name;
                }

            }

            var searchResultData = new SearchResultData
            {
                PageIndex = 1,
                PageSize = 4,
                ListProduct = dtoProducts,
            };

            return await BuildResult(result, searchResultData, MSG_FIND_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<SearchResultData>> GetProductBestSellerAsync()
        {
            var result = new AppActionResultMessage<SearchResultData>();

            var products = (from d in await _productRepository.GetAll()
                            join c in await _classifyProductRepository.GetAll()
                            on d.Id equals c.ProductId
                            where d.IsShow == IsShowConstain.ACTIVE
                            select d).Take(10).ToList();

            var dtoProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<DtoProduct>>(products);

            foreach (var dtoProduct in dtoProducts)
            {
                var category = await _categoryRepository.GetAsync(c => c.Id == ObjectId.Parse(dtoProduct.CategoryId) && c.IsShow != IsShowConstain.DELETE);
                if (category != null)
                {
                    dtoProduct.CategoryName = category.Name;
                }

                var brand = await _brandRepository.GetAsync(b => b.Id == ObjectId.Parse(dtoProduct.BrandId) && b.IsShow != IsShowConstain.DELETE);
                if (brand != null)
                {
                    dtoProduct.BrandName = brand.Name;
                }

            }

            var searchResultData = new SearchResultData
            {
                PageIndex = 1,
                PageSize = 10,
                ListProduct = dtoProducts,
            };

            return await BuildResult(result, searchResultData, MSG_FIND_SUCCESSFULLY);
        }


        #endregion private method

        #region private class method
        private class ProductInfo
        {
            public ObjectId CategoryId { get; set; }

            public ObjectId BrandId { get; set; }

        }
        #endregion private class method
    }
}
