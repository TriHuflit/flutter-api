using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IUserService
    {
        Task<AppActionResultMessage<DtoUser>> GetInfoUserAsync();

        Task<AppActionResultMessage<string>> UpdateInfoUserAsync(UpdateUserRequest request);


        Task<AppActionResultMessage<string>> UpdatePassWordAsync(UpdatePasswordRequest request);

        Task<AppActionResultMessage<string>> UpdateAvatarAsync(UpdateAvatarRequest request);


        Task<AppActionResultMessage<string>> CreateStaffAsync(CreateStaffRequest request);

        Task<AppActionResultMessage<string>> BlockStaffAsync(string idStaff);

        Task<AppActionResultMessage<string>> UpdateRoleAsync(UpdateRoleRequest request);

        Task<AppActionResultMessage<IEnumerable<DtoStaff>>> GettAllStaffAsync();

        Task<AppActionResultMessage<DtoStaff>> GettDetailStaffAsync(string idStaff);
    }
}
