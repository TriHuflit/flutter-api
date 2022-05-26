using AutoMapper;
using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class BrandService : GenericErrorTextService, IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUploadImageService _uploadImageService;

        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IUploadImageService uploadImageService,
            IMessageService messageService) : base(messageService)
        {
            _brandRepository = brandRepository;
            _currentUserService = currentUserService;
            _uploadImageService = uploadImageService;
            _mapper = mapper;
        }

        public async Task<AppActionResultMessage<string>> CreateBrandAsync(BaseBrandRequest request)
        {
            var result = new AppActionResultMessage<string>();

            if(!ObjectId.TryParse(request.CategoryId,out ObjectId objCategory))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.CategoryId));
            }

            var brand = new Brand
            {
                Name = request.Name,
                CategoryId = objCategory,
                IsShow = request.IsShow,
            };

            var validateImage = await _uploadImageService.UploadImage(request.ImageBrand);
            if (!validateImage.IsSuccess)
            {
                return await BuildError(result, validateImage.Message);
            }

            brand.ImageBrand = validateImage.Data;
            brand.SetFullInfor(_currentUserService.UserId, _currentUserService.UserName);
            _brandRepository.Add(brand);

            return await BuildResult(result, brand.Id.ToString(), MSG_SAVE_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<string>> DeleteBrandAsync(string BrandId)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(BrandId, out ObjectId objBrandId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(BrandId));
            }

            var brand = await _brandRepository.GetAsync(c => c.Id == objBrandId && c.IsShow != IsShowConstain.DELETE);
            if (brand == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(BrandId));
            }

            brand.IsShow = IsShowConstain.DELETE;
            brand.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _brandRepository.Update(brand, c => c.Id == objBrandId);

            return await BuildResult(result, BrandId, MSG_DELETE_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<IEnumerable<DtoBrand>>> GetAllBrandAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoBrand>>();

            var brand = await _brandRepository.FindByAsync(_ => true);

            var dtoBrand = _mapper.Map<IEnumerable<Brand>, IEnumerable<DtoBrand>>(brand);

            return await BuildResult(result, dtoBrand, MSG_FIND_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<IEnumerable<DtoBrand>>> GetAllBrandByIdCategoryAsync(string CategoryId)
        {
            var result = new AppActionResultMessage<IEnumerable<DtoBrand>>();

            if (!ObjectId.TryParse(CategoryId, out ObjectId ObjCategoryId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(CategoryId));
            }

            var brand = await _brandRepository.FindByAsync(b => b.CategoryId == ObjCategoryId && b.IsShow != IsShowConstain.DELETE);

            var dtoBrand = _mapper.Map<IEnumerable<Brand>, IEnumerable<DtoBrand>>(brand);

            return await BuildResult(result, dtoBrand, MSG_FIND_SUCCESSFULLY);

        }

        public async Task<AppActionResultMessage<string>> UpdateBrandsAsync(UpdateBrandRequest request)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(request.Id, out ObjectId objCategoryId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.Id));
            }

            var brand = await _brandRepository.GetAsync(c => c.Id == objCategoryId && c.IsShow != IsShowConstain.DELETE);
            if (brand == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(request.Id));
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                brand.Name = request.Name;
            }

            if (IsValidateStatusCategory(request.IsShow))
            {
                brand.IsShow = request.IsShow;
            }

            if (!string.IsNullOrEmpty(request.ImageBrand) && brand.ImageBrand != request.ImageBrand)
            {
                var validateImage = await _uploadImageService.UploadImage(request.ImageBrand);
                if (!validateImage.IsSuccess)
                {
                    return await BuildError(result, validateImage.Message);
                }
                brand.ImageBrand = validateImage.Data;
            }

            brand.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _brandRepository.Update(brand, u => u.Id == objCategoryId);

            return await BuildResult(result, request.Id, MSG_UPDATE_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<DtoBrand>> GetDetailBrandByIdCategoryAsync(string CategoryId)
        {
            var result = new AppActionResultMessage<DtoBrand>();

            if (!ObjectId.TryParse(CategoryId, out ObjectId ObjCategoryId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(CategoryId));
            }

            var brand = await _brandRepository.GetAsync(b => b.CategoryId == ObjCategoryId);

            var dtoBrand = _mapper.Map<Brand,DtoBrand>(brand);

            return await BuildResult(result, dtoBrand, MSG_FIND_SUCCESSFULLY);
        }


        #region private method
        private bool IsValidateStatusCategory(int isShow)
        {
            return isShow == IsShowConstain.ACTIVE || isShow == IsShowConstain.INACTIVE;
        }
        #endregion private method
    }
}
