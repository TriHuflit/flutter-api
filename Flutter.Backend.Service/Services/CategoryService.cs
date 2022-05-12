using AutoMapper;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class CategoryService : GenericErrorTextService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CategoryService(ICategoryRepository categoryRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IMessageService messageService) : base(messageService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<AppActionResultMessage<IEnumerable<DtoCategory>>> GetAllCategoriesAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoCategory>>();

            var category = await _categoryRepository.GetAll();

            var dtoCategories = _mapper.Map<IEnumerable<Category>, List<DtoCategory>>(category);
            

            return await BuildResult(result, dtoCategories, MSG_FIND_SUCCESSFULLY);
        }
    }
}
