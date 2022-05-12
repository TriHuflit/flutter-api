using AutoMapper;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class BrandService : GenericErrorTextService, IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public BrandService(IBrandRepository brandRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IMessageService messageService) : base(messageService)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<AppActionResultMessage<IEnumerable<DtoBrand>>> GetAllBrandByIdCategoryAsync(string CategoryId)
        {
            var result = new AppActionResultMessage<IEnumerable<DtoBrand>>();

            if(!ObjectId.TryParse(CategoryId,out ObjectId ObjCategoryId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(CategoryId));
            }

            var brand = await _brandRepository.FindByAsync(b=>b.CategoryId == ObjCategoryId);

            var dtoBrand = _mapper.Map<IEnumerable<Brand>, IEnumerable<DtoBrand>>(brand);

            return await BuildResult(result,dtoBrand, MSG_FIND_SUCCESSFULLY);
            
        }
    }
}
