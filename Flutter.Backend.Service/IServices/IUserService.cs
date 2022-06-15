using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IUserService
    {
        Task<AppActionResultMessage<DtoUser>> GetInfoUserAsync();

        Task<AppActionResultMessage<string>> UpdateInfoUserAsync();


        Task<AppActionResultMessage<string>> UpdatePassWordAsync();


        Task<AppActionResultMessage<string>> UpdateAvatarAsync();
    }
}
