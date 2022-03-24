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
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class AuthendicateService : GenericErrorTextService, IAuthenticateService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly ITemplateSendMailRepository _templateSendMailRepository;
        private readonly IConfiguration _config;

        private readonly IValidationService _validationService;
        private readonly ISendMailService _sendMailService;

        public AuthendicateService(
            IAppUserRepository appUserRepository,
            IConfiguration config,
            IValidationService validationService,
            ISendMailService sendMailService,
            ITemplateSendMailRepository templateSendMailRepository,
            IMessageService messageService) : base(messageService)
        {
            _appUserRepository = appUserRepository;
            _config = config;
            _validationService = validationService;
            _sendMailService = sendMailService;
            _templateSendMailRepository = templateSendMailRepository;
        }

        public async Task<AppActionResultMessage<DtoAuthent>> AuthendicateAsync(AuthendicateRequest request)
        {
            var result = new AppActionResultMessage<DtoAuthent>();

            string hashPassword = HashPassWord(request.Password);

            var user = await _appUserRepository.GetAsync(u => u.UserName == request.UserName
                && u.HashPassword == hashPassword);

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

            var token = GenerateJwtToken(user.Id.ToString(), user.UserName);

            var refreshToken = GenerateRefresh(user.Id.ToString(), user.UserName);
            user.RefreshToken = refreshToken;
            user.SetUpdatedInFo(user.Id.ToString(), user.UserName);
            _appUserRepository.Update(user, u => u.Id == user.Id);

            return await BuildResult(result, token, MSG_LOGIN_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<string>> RegisterAsync(RegisterRequest request)
        {

            var result = new AppActionResultMessage<string>();

            var trackDataResult = await ReadFile(request);
            if (!trackDataResult.IsSuccess)
            {
                return await BuildError(result, trackDataResult.Message);
            }

            if (!_validationService.ValidatePasswordFormat(request.Password))
            {
                return await BuildError(result, ERR_MSG_PASSWORD_ISVALID_FORMART);
            }

            if (request.Password != request.ComfirmPassWord)
            {
                return await BuildError(result, ERR_MSG_PASSWORD_ISVALID_FORMART);
            }

            if (!_validationService.ValidatePhoneNumberFormat(request.Phone))
            {
                return await BuildError(result, ERR_MSG_PHONE_ISVALID_FORMART);
            }

            if (!_validationService.ValidateVnPhoneNumberFormat(request.Phone))
            {
                return await BuildError(result, ERR_MSG_PHONE_ISVALID_FORMART_VN);
            }

            if (!_validationService.ValidateEmailFormat(request.Email))
            {
                return await BuildError(result, ERR_MSG_EMAIL_ISVALID_FORMART);
            }

            // add more message
            if (request.Gender != AppUserGenderConstain.Female && request.Gender != AppUserGenderConstain.Male)
            {
                return await BuildError(result, ERR_MSG_EMAIL_ISVALID_FORMART);
            }

            string hashPassword = HashPassWord(request.Password);
            var newUser = new AppUser
            {
                UserName = request.UserName,
                FullName = request.FullName,
                HashPassword = hashPassword,
                Email = request.Email,
                Location = new Location
                {
                    Province = request.Province,
                    District = request.District,
                    Ward = request.Ward,
                    Address = request.Address
                },
                IsEmailConfirmed = false,
                IsActive = AuthendicateConstain.ACTIVE,
                Birth = request.Birth,
                Phone = request.Phone,
                Gender = request.Gender,
            };

            if (request.Gender == AppUserGenderConstain.Female)
            {
                newUser.Avatar = _config[ConfigAppsettingConstaint.AvatarFemale];
            }

            if (request.Gender == AppUserGenderConstain.Male)
            {
                newUser.Avatar = _config[ConfigAppsettingConstaint.AvatarMale];
            }

            _appUserRepository.Add(newUser);
            newUser.SetFullInfo(newUser.Id.ToString(), newUser.UserName);
            _appUserRepository.Update(newUser, u => u.Id == newUser.Id);
            var template = await _templateSendMailRepository.GetAsync(t => t.Key == "TEMPLATE_EMAIL_REGISTER_ACCOUNT");
            var requestSendMail = new MailRequest
            {
                Body = template.TemplateHTML,
                Subject = "Đăng Ký Tài Khoản Cho Ứng Dụng HTC",
                ToEmail = request.Email
            };
            var sendMailResult = _sendMailService.SendMailRegisterAsync(requestSendMail);

            return await BuildResult(result,newUser.Id.ToString() ,MSG_SAVE_SUCCESSFULLY);
        }

        public Task<AppActionResultMessage<DtoRefreshToken>> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        #region
        private byte[] RSAEncrypt(byte[] plaintext, string destKey)
        {
            byte[] encryptedData;
            RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(destKey);
            encryptedData = rsa.Encrypt(plaintext, true);
            rsa.Dispose();

            return encryptedData;
        }

        private byte[] RSADecrypt(byte[] ciphertext, string srcKey)
        {
            byte[] decryptedData;
            RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(srcKey);
            decryptedData = rsa.Decrypt(ciphertext, true);
            rsa.Dispose();

            return decryptedData;
        }

        private string HashPassWord(string password)
        {
            RSACryptoServiceProvider rsa = new();
            string pubkey = rsa.ToXmlString(false);
            string prikey = rsa.ToXmlString(true);
            byte[] someThing = RSAEncrypt(Encoding.Unicode.GetBytes(password), pubkey);
            byte[] anotherThing = RSADecrypt(someThing, prikey);

            return Convert.ToBase64String(anotherThing);
        }

        private async Task<AppActionResultMessage<string>> ReadFile(RegisterRequest register)
        {
            var result = new AppActionResultMessage<string>();

            HttpWebRequest request = (HttpWebRequest)
            WebRequest.Create(_config[ConfigAppsettingConstaint.TrackData]);
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader v = new(resStream);
            string[] line = v.ReadToEnd().Split(new char[] { '\r' });
            for (int i = 0; i < line.Length; i++)
            {
                line[i] = line[i].ToLower();
                if (line[i] == register.UserName.ToLower())
                {
                    return await BuildError(result, ERR_MSG_NAME_INCORRECT, nameof(register.UserName));
                }

                if (line[i] == register.Password.ToLower())
                {
                    return await BuildError(result, ERR_MSG_NAME_INCORRECT, nameof(register.Password));
                }

                if (line[i] == register.FullName.ToLower())
                {
                    return await BuildError(result, ERR_MSG_NAME_INCORRECT, nameof(register.FullName));
                }
            }

            return await BuildResult(result, MSG_FIND_SUCCESSFULLY);
        }

        private DtoAuthent GenerateJwtToken(string UserId, string UserName)
        {
            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, UserId),
               new Claim(ClaimTypes.Name, UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[ConfigAppsettingConstaint.TokenKey]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config[ConfigAppsettingConstaint.TokenIssuer],
                _config[ConfigAppsettingConstaint.TokenIssuer],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            var dtoAppUser = new DtoAuthent();
            dtoAppUser.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            dtoAppUser.Expires = DateTime.Now.AddHours(1);

            return dtoAppUser;
        }

        private string GenerateRefresh(string UserId, string UserName)
        {
            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, UserId),
               new Claim(ClaimTypes.Name, UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[ConfigAppsettingConstaint.TokenKey]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config[ConfigAppsettingConstaint.TokenIssuer],
                _config[ConfigAppsettingConstaint.TokenIssuer],
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        #endregion
    }
}
