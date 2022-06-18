using AutoMapper;
using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;


namespace Flutter.Backend.Service.Services
{
    public class BannerService : GenericErrorTextService, IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        private readonly IUploadImageService _uploadImageService;
        private readonly ICurrentUserService _currentUserService;

        public BannerService(
            IMapper mapper,
            IConfiguration config,
            IUploadImageService uploadImageService,
            ICurrentUserService currentUserService,
            IBannerRepository bannerRepository,
            IMessageService messageService,
            IProductRepository productRepository) : base(messageService)
        {

            _bannerRepository = bannerRepository;
            _uploadImageService = uploadImageService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _config = config;
            _productRepository = productRepository;
        }

        public async Task<AppActionResultMessage<string>> CreateBannerAsync(BaseBannerRequest request)
        {
            var result = new AppActionResultMessage<string>();

            if (!ValidateStatusBanner(request.IsShowOnMobile))
            {
                return await BuildError(result, "Trạng thái banner không đúng");
            }

            if (!ObjectId.TryParse(request.IdProduct, out ObjectId objProduct))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, request.IdProduct);
            }

            var product = await _productRepository.GetAsync(p => p.Id == objProduct && p.IsShow == IsShowConstain.ACTIVE);
            if (product == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, request.IdProduct);
            }

            var banner = new Banner
            {
                Content = request.Content,
                IsShowOnMobile = request.IsShowOnMobile,
                IdProduct = objProduct
            };

            var imageResult = await _uploadImageService.UploadImage(request.ImageBanner);
            if (!imageResult.IsSuccess)
            {
                return await BuildError(result, imageResult.Message);
            }
            banner.ImageBanner = imageResult.Data;

            banner.SetFullInfor(_currentUserService.UserId, _currentUserService.UserName);
            _bannerRepository.Add(banner);

            return await BuildResult(result, banner.Id.ToString(), MSG_SAVE_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<string>> DeleteBannerAsync(string bannerId)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(bannerId, out ObjectId ObjBanner))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, bannerId);
            }

            var banner = await _bannerRepository.GetAsync(b => b.Id == ObjBanner && b.IsShowOnMobile != IsShowConstain.DELETE);
            if (banner == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, bannerId);
            }

            banner.IsShowOnMobile = IsShowConstain.DELETE;
            banner.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _bannerRepository.Update(banner, b => b.Id == banner.Id);

            return await BuildResult(result,banner.Id.ToString(), MSG_DELETE_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<IEnumerable<DtoBanner>>> GetAllBannerAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoBanner>>();

            var banners = await _bannerRepository.FindByAsync(b => b.IsShowOnMobile != IsShowConstain.DELETE);       

            var dtoBanner = _mapper.Map<IEnumerable<Banner>, IEnumerable<DtoBanner>>(banners);

            foreach (var banner in dtoBanner)
            {
                var product = await _productRepository.GetAsync(p => p.Id ==ObjectId.Parse(banner.IdProduct) && p.IsShow == IsShowConstain.ACTIVE);
                banner.ProductName = product.Name;
                banner.Price = product.FromPrice;
            }

            return await BuildResult(result, dtoBanner, MSG_FIND_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<IEnumerable<DtoBanner>>> GetAllBannerMobileAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoBanner>>();

            var banners = await _bannerRepository.FindByAsync(b => b.IsShowOnMobile == IsShowConstain.ACTIVE);

            var dtoBanner = _mapper.Map<IEnumerable<Banner>, IEnumerable<DtoBanner>>(banners);
            foreach (var banner in dtoBanner)
            {
                var product = await _productRepository.GetAsync(p => p.Id == ObjectId.Parse(banner.IdProduct) && p.IsShow == IsShowConstain.ACTIVE);
                banner.ProductName = product.Name;
                banner.Price = product.FromPrice;
            }

            return await BuildResult(result, dtoBanner, MSG_FIND_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<DtoBanner>> GetBannerAsync(string bannerId)
        {
            var result = new AppActionResultMessage<DtoBanner>();

            if (!ObjectId.TryParse(bannerId, out ObjectId ObjBanner))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, bannerId);
            }

            var banner = await _bannerRepository.GetAsync(b => b.Id == ObjBanner && b.IsShowOnMobile != IsShowConstain.DELETE);
            if (banner == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, bannerId);
            }
            var product = await _productRepository.GetAsync(p => p.Id == banner.IdProduct && p.IsShow == IsShowConstain.ACTIVE);

            var dtoBanner = _mapper.Map<Banner, DtoBanner>(banner);

            dtoBanner.ProductName = product.Name;
            dtoBanner.Price = product.FromPrice;

            return await BuildResult(result, dtoBanner, MSG_FIND_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<DtoBanner>> GetBannerMobileAsync(string bannerId)
        {
            var result = new AppActionResultMessage<DtoBanner>();

            if (!ObjectId.TryParse(bannerId, out ObjectId ObjBanner))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, bannerId);
            }

            var banner = await _bannerRepository.GetAsync(b => b.Id == ObjBanner && b.IsShowOnMobile == IsShowConstain.ACTIVE);
            if (banner == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, bannerId);
            }
            var product = await _productRepository.GetAsync(p => p.Id == banner.IdProduct && p.IsShow == IsShowConstain.ACTIVE);
            var dtoBanner = _mapper.Map<Banner, DtoBanner>(banner);

            dtoBanner.ProductName = product.Name;
            dtoBanner.Price = product.FromPrice;
            return await BuildResult(result, dtoBanner, MSG_FIND_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<string>> UpdateBannerAsync(UpdateBannerRequest request)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(request.Id, out ObjectId ObjBanner))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, request.Id);
            }

            var banner = await _bannerRepository.GetAsync(b => b.Id == ObjBanner && b.IsShowOnMobile != IsShowConstain.DELETE);
            if (banner == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, request.Id);
            }

            if (!string.IsNullOrEmpty(request.Content))
            {
                banner.Content = request.Content;
            }

            if (!string.IsNullOrEmpty(request.IdProduct))
            {
                if (!ObjectId.TryParse(request.IdProduct, out ObjectId objProduct))
                {
                    return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, request.IdProduct);
                }

                var product = await _productRepository.GetAsync(p => p.Id == objProduct && p.IsShow == IsShowConstain.ACTIVE);
                if (product == null)
                {
                    return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, request.IdProduct);
                }

                banner.IdProduct = objProduct;
            }

            if (ValidateStatusBanner(request.IsShowOnMobile))
            {
                banner.IsShowOnMobile = request.IsShowOnMobile;
            }

            if (!string.IsNullOrEmpty(request.ImageBanner) && request.ImageBanner != banner.ImageBanner)
            {
                var imageResult = await _uploadImageService.UploadImage(request.ImageBanner);
                if (!imageResult.IsSuccess)
                {
                    return await BuildError(result, imageResult.Message);
                }
                banner.ImageBanner = imageResult.Data;
            }

            banner.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _bannerRepository.Update(banner, b => b.Id == ObjBanner);

            return await BuildResult(result,banner.Id.ToString(), MSG_UPDATE_SUCCESSFULLY);
        }

        #region Private Method
        private bool ValidateStatusBanner(int IsShow)
        {
            return IsShow == IsShowConstain.ACTIVE || IsShow == IsShowConstain.INACTIVE;
        }
        #endregion Private Method
    }
}
