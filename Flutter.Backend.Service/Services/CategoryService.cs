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
    public class CategoryService : GenericErrorTextService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUploadImageService _uploadImageService;

        private readonly IMapper _mapper;


        public CategoryService(ICategoryRepository categoryRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IMessageService messageService,
            IUploadImageService uploadImageService) : base(messageService)
        {
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
            _uploadImageService = uploadImageService;

            _mapper = mapper;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<string>> CreateCategoriesAsync(BaseCategoryRequest request)
        {
            var result = new AppActionResultMessage<string>();

            var category = new Category
            {
                Name = request.Name,
                IsShow = request.IsShow,
            };

            var validateImage = await _uploadImageService.UploadImage(request.ImageCategory);
            if (!validateImage.IsSuccess)
            {
                return await BuildError(result, validateImage.Message);
            }

            category.ImageCategory = validateImage.Data;
            category.SetFullInfor(_currentUserService.UserId, _currentUserService.UserName);
            _categoryRepository.Add(category);

            return await BuildResult(result, category.Id.ToString(), MSG_SAVE_SUCCESSFULLY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<string>> DeleteCategoriesAsync(string CategoryId)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(CategoryId, out ObjectId objCategoryId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(CategoryId));
            }

            var category = await _categoryRepository.GetAsync(c => c.Id == objCategoryId && c.IsShow != IsShowConstain.DELETE);
            if (category == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(CategoryId));
            }

            category.IsShow = IsShowConstain.DELETE;
            category.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _categoryRepository.Update(category, c => c.Id == objCategoryId);

            return await BuildResult(result, CategoryId, MSG_DELETE_SUCCESSFULLY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<IEnumerable<DtoCategory>>> GetAllCategoriesAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoCategory>>();

            var category = await _categoryRepository.FindByAsync(c=>c.IsShow != IsShowConstain.DELETE);

            var dtoCategories = _mapper.Map<IEnumerable<Category>, List<DtoCategory>>(category);


            return await BuildResult(result, dtoCategories, MSG_FIND_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<DtoCategory>> GetDetailCategoryAsync(string CategoryId)
        {
            var result = new AppActionResultMessage<DtoCategory>();

            if(!ObjectId.TryParse(CategoryId,out ObjectId objCategory))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(CategoryId));
            }

            var category = await _categoryRepository.GetAsync(c =>c.Id == objCategory  && c.IsShow != IsShowConstain.DELETE);
            if(category == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(category));
            }
            var dtoCategory = _mapper.Map<Category, DtoCategory>(category);


            return await BuildResult(result, dtoCategory, MSG_FIND_SUCCESSFULLY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<AppActionResultMessage<string>> UpdateCategoriesAsync(UpdateCategoryRequest request)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(request.Id, out ObjectId objCategoryId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.Id));
            }

            var category = await _categoryRepository.GetAsync(c => c.Id == objCategoryId && c.IsShow != IsShowConstain.DELETE);
            if (category == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(request.Id));
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                category.Name = request.Name;
            }

            if (IsValidateStatusCategory(request.IsShow))
            {
                category.IsShow = request.IsShow;
            }

            if (!string.IsNullOrEmpty(request.ImageCategory) && category.ImageCategory != request.ImageCategory)
            {
                var validateImage = await _uploadImageService.UploadImage(request.ImageCategory);
                if (!validateImage.IsSuccess)
                {
                    return await BuildError(result, validateImage.Message);
                }
                category.ImageCategory = validateImage.Data;
            }

            category.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _categoryRepository.Update(category, u => u.Id == objCategoryId);

            return await BuildResult(result, request.Id, MSG_UPDATE_SUCCESSFULLY);
        }

        #region private method
        private bool IsValidateStatusCategory(int isShow)
        {
            return isShow == IsShowConstain.ACTIVE || isShow == IsShowConstain.INACTIVE;
        }
        #endregion private method
    }
}
