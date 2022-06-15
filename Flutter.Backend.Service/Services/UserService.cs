using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Services
{
    public class UserService : GenericErrorTextService , IUserService
    {
        private readonly IAppUserRepository _appUserRepository;

        public UserService(
            IAppUserRepository appUserRepository,
             IMessageService messageService): base(messageService)
        {
            _appUserRepository = appUserRepository;
        }

        public Task<AppActionResultMessage<DtoUser>> GetInfoUserAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<string>> UpdateAvatarAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<string>> UpdateInfoUserAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<AppActionResultMessage<string>> UpdatePassWordAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
