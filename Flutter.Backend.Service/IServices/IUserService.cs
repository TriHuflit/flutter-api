using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IUserService
    {
        Task<AppActionResultMessage<DtoUser>> GetInfoUserAsync();

        Task<AppActionResultMessage<string>> UpdateInfoUserAsync(UpdateUserRequest request);


        Task<AppActionResultMessage<string>> UpdatePassWordAsync(UpdatePasswordRequest request);


        Task<AppActionResultMessage<string>> UpdateAvatarAsync(UpdateAvatarRequest request);
    }
}
