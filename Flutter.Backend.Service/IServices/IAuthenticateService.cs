using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IAuthenticateService
    {
        Task<AppActionResultMessage<DtoAuthent>> AuthendicateUserAsync(AuthendicateRequest request);

        Task<AppActionResultMessage<DtoAuthent>> AuthendicateAdminAsync(AuthendicateRequest request);

        Task<AppActionResultMessage<string>> RegisterAsync(RegisterRequest request);

        Task<AppActionResultMessage<string>> ComfirmEmailAsync(string UserId);

        Task<AppActionResultMessage<DtoRefreshToken>> RefreshTokenAsync(string refreshToken);
    }
}
