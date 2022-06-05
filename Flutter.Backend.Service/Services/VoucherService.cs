using AutoMapper;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Services
{
    public class VoucherService : GenericErrorTextService, IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;

        private readonly IMapper _mapper;

        private readonly IUploadImageService _uploadImageService;
        private readonly ICurrentUserService _currentUserService;

        public VoucherService(
            IMapper mapper,
            IUploadImageService uploadImageService,
            ICurrentUserService currentUserService,
            IVoucherRepository voucherRepository,
            IMessageService messageService) : base(messageService)
        {

            _voucherRepository = voucherRepository;
            _uploadImageService = uploadImageService;
            _currentUserService = currentUserService;
            _mapper = mapper;

        }

        public Task<AppActionResultMessage<string>> CreateVoucherAsync(BaseVoucherRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<string>> DeleteVoucherAsync(string productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<IEnumerable<DtoVoucher>>> GetAllVoucherAsync(PaginationRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<IEnumerable<DtoVoucher>>> GetAllVoucherMobileAsync(PaginationRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<DtoVoucher>> GetVoucherAsync(string productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<DtoVoucher>> GetVoucherMobileAsync(string productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<string>> UpdateVoucherAsync(UpdateVoucherRequest request)
        {
            throw new System.NotImplementedException();
        }





        #region private method

        #endregion private method

        #region private class method

        #endregion private class method
    }
}
