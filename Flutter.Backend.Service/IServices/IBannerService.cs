using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IBannerService
    {
        Task<AppActionResultMessage<string>> CreateBannerAsync(BaseVoucherRequest request);

        Task<AppActionResultMessage<string>> UpdateBannerAsync(UpdateVoucherRequest request);

        Task<AppActionResultMessage<string>> DeleteBannerAsync(string bannerId);

        Task<AppActionResultMessage<IEnumerable<DtoBanner>>> GetAllBannerAsync();

        Task<AppActionResultMessage<DtoBanner>> GetBannerAsync(string bannerId);

        Task<AppActionResultMessage<IEnumerable<DtoBanner>>> GetAllBannerMobileAsync();

        Task<AppActionResultMessage<DtoBanner>> GetBannerMobileAsync(string bannerId);

    }
}
