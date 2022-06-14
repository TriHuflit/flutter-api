using AutoMapper;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Services
{
    public class BannerService : GenericErrorTextService, IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        private readonly IUploadImageService _uploadImageService;
        private readonly ICurrentUserService _currentUserService;

        public BannerService(
            IMapper mapper,
            IConfiguration config,
            IUploadImageService uploadImageService,
            ICurrentUserService currentUserService,
            IBannerRepository bannerRepository,
            IMessageService messageService) : base(messageService)
        {

            _bannerRepository = bannerRepository;
            _uploadImageService = uploadImageService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _config = config;
        }

        public Task<AppActionResultMessage<string>> CreateBannerAsync(BaseVoucherRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<string>> DeleteBannerAsync(string bannerId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<IEnumerable<DtoBanner>>> GetAllBannerAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<IEnumerable<DtoBanner>>> GetAllBannerMobileAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<DtoBanner>> GetBannerAsync(string bannerId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<DtoBanner>> GetBannerMobileAsync(string bannerId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<string>> UpdateBannerAsync(UpdateVoucherRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
