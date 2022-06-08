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

        public async Task<AppActionResultMessage<string>> CreateVoucherAsync(BaseVoucherRequest request)
        {
            var result = new AppActionResultMessage<string>();

            if (!ValidateIsShow(request.IsShow))
            {
                // Add message
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(request.IsShow));
            }

            if (!ObjectId.TryParse(_currentUserService.UserId, out ObjectId objUser))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(_currentUserService.UserId));
            }

            if (!ValidateVoucher(request.Type))
            {
                // Add message
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(request.Type));
            }

            if (request.DisCountAmount > 0 && request.DisCountPercent > 0)
            {
                // Add message
                return await BuildError(result, "Giảm giá phần trăm và giảm giá ... không được có cùng lúc");
            }

            if (request.DisCountAmount == 0 && request.DisCountPercent == 0)
            {
                // Add message
                return await BuildError(result, "Giảm giá phần trăm và giảm giá ... không được bỏ trống cùng lúc");
            }

            if (request.ToDate < request.FromDate)
            {
                // Add message
                return await BuildError(result, "Ngày bắt đầu không được lớn hơn ngày kết thúc");
            }

            if ((request.DisCountAmount > 0 && request.DisCountPercent > 0) && request.ToCondition < request.FromCondition)
            {
                // Add message
                return await BuildError(result, "Điều kiện tối thiệu thỏa mãn voucher không được lơn hơn điều kiện tối đa ");
            }

            var voucher = new Voucher
            {
                Title = request.Title,
                Description = request.Description,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                FromCondition = request.FromCondition,
                ToCondition = request.ToCondition,
                DisCountAmount = request.DisCountAmount,
                DisCountPercent = request.DisCountPercent,
                IsShow = request.IsShow,
                Type = request.Type
            };

            var imageVoucherImageResult = await _uploadImageService.UploadImage(request.ImageVoucher);
            if (!imageVoucherImageResult.IsSuccess)
            {
                return await BuildError(result, imageVoucherImageResult.Message);
            }
            voucher.ImageVoucher = imageVoucherImageResult.Data;

            voucher.SetFullInfor(_currentUserService.UserId, _currentUserService.UserName);
            _voucherRepository.Add(voucher);

            return await BuildResult(result, voucher.Id.ToString(), MSG_SAVE_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<string>> DeleteVoucherAsync(string VoucherId)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(_currentUserService.UserId, out ObjectId objUser))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(_currentUserService.UserId));
            }

            if (!ObjectId.TryParse(VoucherId, out ObjectId objVoucher))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(VoucherId));
            }

            var voucher = await _voucherRepository.GetAsync(v => v.Id == objVoucher && v.IsShow != IsShowConstain.DELETE);
            if (voucher == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(voucher));
            }

            voucher.IsShow = IsShowConstain.DELETE;
            voucher.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _voucherRepository.Update(voucher, v => v.Id == objVoucher);

            return await BuildResult(result, voucher.Id.ToString(), MSG_DELETE_SUCCESSFULLY);
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

        public async Task<AppActionResultMessage<string>> UpdateVoucherAsync(UpdateVoucherRequest request)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(_currentUserService.UserId, out ObjectId objUser))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(_currentUserService.UserId));
            }

            if (!ObjectId.TryParse(request.Id, out ObjectId objVoucher))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.Id));
            }

            if (request.ToDate < request.FromDate)
            {
                // Add message
                return await BuildError(result, "Ngày bắt đầu không được lớn hơn ngày kết thúc");
            }

            var voucher = await _voucherRepository.GetAsync(v => v.Id == objVoucher && v.IsShow != IsShowConstain.DELETE);
            if (voucher == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(voucher));
            }

            if (!string.IsNullOrEmpty(request.Title))
            {
                voucher.Title = request.Title;
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                voucher.Description = request.Description;
            }

            if (ValidateIsShow(request.IsShow))
            {
                voucher.IsShow = request.IsShow;
            }

            voucher.ToDate = request.ToDate;
            voucher.FromDate = request.FromDate;
            voucher.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _voucherRepository.Update(voucher, v => v.Id == objVoucher);

            return await BuildResult(result, voucher.Id.ToString(), MSG_SAVE_SUCCESSFULLY);
        }





        #region private method
        public bool ValidateIsShow(int IsShow)
        {
            return IsShow == IsShowConstain.ACTIVE || IsShow == IsShowConstain.INACTIVE;
        }

        public bool ValidateVoucher(string TypeVoucher)
        {
            return TypeVoucher == TypeVoucherConstain.VOUCHER_ORDER || TypeVoucher == TypeVoucherConstain.VOUCHER_DELEVERY;
        }
        #endregion private method

        #region private class method

        #endregion private class method
    }
}
