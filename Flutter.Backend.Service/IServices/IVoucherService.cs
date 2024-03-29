﻿using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IVoucherService
    {
        Task<AppActionResultMessage<string>> CreateVoucherAsync(BaseVoucherRequest request);

        Task<AppActionResultMessage<string>> UpdateVoucherAsync(UpdateVoucherRequest request);

        Task<AppActionResultMessage<string>> DeleteVoucherAsync(string voucherId);

        Task<AppActionResultMessage<IEnumerable<DtoVoucher>>> GetAllVoucherAsync();

        Task<AppActionResultMessage<DtoVoucher>> GetVoucherAsync(string voucherId);

        Task<AppActionResultMessage<IEnumerable<DtoVoucher>>> GetAllVoucherMobileAsync();

        Task<AppActionResultMessage<DtoVoucher>> GetVoucherMobileAsync(string voucherId);

    }
}
