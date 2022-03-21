using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class AuthendicateService : GenericErrorTextService, IAuthenticateService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IConfiguration _config;

        public AuthendicateService(
            IAppUserRepository appUserRepository,
            IConfiguration config,
            IMessageService messageService) : base(messageService)
        {
            _appUserRepository = appUserRepository;
            _config = config;
        }

        public async Task<AppActionResultMessage<DtoAppUser>> AuthendicateAsync(AuthendicateRequest request)
        {
            var result = new AppActionResultMessage<DtoAppUser>();

            var user = await _appUserRepository.GetAsync(u => u.UserName == request.UserName
                && u.HashPassword == request.Password);

            if (user == null && user.IsActive == AuthendicateConstain.DELETE)
            {
                return await BuildResult(result, ERR_MSG_USERNAME_PASSWORD_INCORRECT);
            }

            if (!user.IsEmailConfirmed)
            {
                return await BuildResult(result, ERR_MSG_EMAIL_IS_NOT_CONFIRM);
            }

            if (user.IsActive == AuthendicateConstain.INACTIVE)
            {
                return await BuildResult(result, ERR_MSG_ACCOUNT_INACTIVE);
            }

            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(ClaimTypes.Role, user.Role.ToString()),
               new Claim(ClaimTypes.Name, user.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            var dtoAppUser = new DtoAppUser();

            dtoAppUser.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return await BuildError(result,MSG_LOGIN_SUCCESSFULLY,dtoAppUser);
        }

        public Task<AppActionResultMessage<string>> RegisterAsync(RegisterRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
