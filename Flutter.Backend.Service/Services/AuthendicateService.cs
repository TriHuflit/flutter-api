using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
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
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _config;

        private readonly IValidationService _validationService;
        private readonly ISendMailService _sendMailService;

        public AuthendicateService(
            IAppUserRepository appUserRepository,
            IConfiguration config,
            IValidationService validationService,
            ISendMailService sendMailService,
            IRoleRepository roleRepository,
            ITemplateSendMailRepository templateSendMailRepository,
            IMessageService messageService) : base(messageService)
        {
            _appUserRepository = appUserRepository;
            _templateSendMailRepository = templateSendMailRepository;
            _roleRepository = roleRepository;
            _validationService = validationService;
            _sendMailService = sendMailService;
            _config = config;
        }

        /// <summary>
        /// Authendicates the user asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<DtoAuthent>> AuthendicateUserAsync(AuthendicateRequest request)
        {
            var result = new AppActionResultMessage<DtoAuthent>();

            string hashPassword = HashPassWord(request.Password);

            var user = await _appUserRepository.GetAsync(u => u.UserName == request.UserName
                && u.HashPassword == hashPassword && u.IsActive != AuthendicateConstain.DELETE);

            if (user == null)
            {
                return await BuildError(result, ERR_MSG_USERNAME_PASSWORD_INCORRECT);
            }

            if (!user.IsEmailConfirmed)
            {
                return await BuildError(result, ERR_MSG_EMAIL_IS_NOT_CONFIRM);
            }

            if (user.IsActive == AuthendicateConstain.INACTIVE)
            {
                return await BuildError(result, ERR_MSG_ACCOUNT_INACTIVE);
            }

            var role = await _roleRepository.GetAsync(r => r.Id == user.RoleId);

            if (role.Name != RoleConstain.USER)
            {
                return await BuildError(result, ERR_MSG_401_UNAUTHORIZED, user.UserName);
            }


            var token = GenerateJwtToken(user.Id.ToString(), user.UserName, role.Name);

            var refreshToken = GenerateRefresh(user.Id.ToString(), user.UserName, role.Name);

            token.ExpiresRefresh = refreshToken.ExpiresRefresh;
            token.RefreshToken = refreshToken.RefreshToken;
            user.RefreshToken = refreshToken.RefreshToken;
            user.SetUpdatedInFo(user.Id.ToString(), user.UserName);
            _appUserRepository.Update(user, u => u.Id == user.Id);

            return await BuildResult(result, token, MSG_LOGIN_SUCCESSFULLY);
        }

        /// <summary>
        /// Authendicates the admin asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<DtoAuthent>> AuthendicateAdminAsync(AuthendicateRequest request)
        {
            var result = new AppActionResultMessage<DtoAuthent>();

            string hashPassword = HashPassWord(request.Password);

            var user = await _appUserRepository.GetAsync(u => u.UserName == request.UserName
                && u.HashPassword == hashPassword);

            if (user == null && user.IsActive == AuthendicateConstain.DELETE)
            {
                return await BuildError(result, ERR_MSG_USERNAME_PASSWORD_INCORRECT);
            }

            if (!user.IsEmailConfirmed)
            {
                return await BuildError(result, ERR_MSG_EMAIL_IS_NOT_CONFIRM);
            }

            if (user.IsActive == AuthendicateConstain.INACTIVE)
            {
                return await BuildError(result, ERR_MSG_ACCOUNT_INACTIVE);
            }

            var role = await _roleRepository.GetAsync(r => r.Id == user.RoleId);

            if (role.Name == RoleConstain.USER)
            {
                return await BuildError(result, ERR_MSG_401_UNAUTHORIZED, user.UserName);
            }


            var token = GenerateJwtToken(user.Id.ToString(), user.UserName, role.Name);

            var refreshToken = GenerateRefresh(user.Id.ToString(), user.UserName, role.Name);

            token.ExpiresRefresh = refreshToken.ExpiresRefresh;
            token.RefreshToken = refreshToken.RefreshToken;
            user.RefreshToken = refreshToken.RefreshToken;
            user.SetUpdatedInFo(user.Id.ToString(), user.UserName);
            _appUserRepository.Update(user, u => u.Id == user.Id);

            return await BuildResult(result, token, MSG_LOGIN_SUCCESSFULLY);
        }

        /// <summary>
        /// Registers the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<string>> RegisterAsync(RegisterRequest request)
        {

            var result = new AppActionResultMessage<string>();

            //validation username
            if (!_validationService.ValidateUserName(request.UserName))
            {
                return await BuildError(result, ERR_MSG_USERNAME_IS_VALID, nameof(request.UserName));
            }

            var trackDataResult = await ReadFile(request);
            if (!trackDataResult.IsSuccess)
            {
                return await BuildError(result, trackDataResult.Message);
            }

            //validation password
            if (!_validationService.ValidatePasswordFormat(request.Password))
            {
                return await BuildError(result, ERR_MSG_PASSWORD_ISVALID_FORMART);
            }

            if (request.Password != request.ComfirmPassWord)
            {
                return await BuildError(result, ERR_MSG_PASSWORD_ISVALID_FORMART);
            }

            //validation phone
            if (!_validationService.ValidatePhoneNumberFormat(request.Phone))
            {
                return await BuildError(result, ERR_MSG_PHONE_ISVALID_FORMART);
            }

            if (!_validationService.ValidateVnPhoneNumberFormat(request.Phone))
            {
                return await BuildError(result, ERR_MSG_PHONE_ISVALID_FORMART_VN);
            }

            //validation email
            if (!_validationService.ValidateEmailFormat(request.Email))
            {
                return await BuildError(result, ERR_MSG_EMAIL_ISVALID_FORMART);
            }

            var user = await _appUserRepository.GetAsync(u => u.UserName == request.UserName
            && u.IsActive != AuthendicateConstain.DELETE);

            if (user != null)
            {
                return await BuildError(result, ERR_MSG_USERNAME_IS_EXIST, request.UserName);
            }

            user = await _appUserRepository.GetAsync(u => u.Email == request.Email
            && u.IsActive != AuthendicateConstain.DELETE);
            if (user != null)
            {
                return await BuildError(result, ERR_MSG_EMAIL_IS_EXIST, request.Email);
            }

            user = await _appUserRepository.GetAsync(u => u.Phone == request.Phone &&
            u.IsActive != AuthendicateConstain.DELETE);
            if (user != null)
            {
                return await BuildError(result, ERR_MSG_PHONE_IS_EXIST, request.Phone);
            }

            if (request.Gender != AppUserGenderConstain.Female && request.Gender != AppUserGenderConstain.Male)
            {
                return await BuildError(result, ERR_MSG_GENDER_INVALID, request.Gender);
            }

            // create new user
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

            //send email comfirm account
            _appUserRepository.Add(newUser);
            newUser.SetFullInfo(newUser.Id.ToString(), newUser.UserName);
            _appUserRepository.Update(newUser, u => u.Id == newUser.Id);
            var template = await _templateSendMailRepository.GetAsync(t => t.Key == SendMailConstain.TemplateEmailRegister);
            var urlComfirmEmail = String.Format(SendMailConstain.EmailComfirmUrl, newUser.Id.ToString());
            var requestSendMail = new MailRequest
            {
                Body = String.Format(template.TemplateHTML, urlComfirmEmail),
                Subject = SendMailConstain.SubjectRegister,
                ToEmail = request.Email
            };

            var sendMailResult = await _sendMailService.SendMailRegisterAsync(requestSendMail);
            if (!sendMailResult.IsSuccess)
            {
                return await BuildError(result,ERR_MSG_EMAIL_IS_NOT_CONFIRM);
            }

            return await BuildResult(result, newUser.Id.ToString(), MSG_SAVE_SUCCESSFULLY);
        }

        /// <summary>
        /// Refreshes the token asynchronous.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<DtoRefreshToken>> RefreshTokenAsync(string refreshToken)
        {
            var result = new AppActionResultMessage<DtoRefreshToken>();

            if (!IsValidateToken(refreshToken))
            {
                return await BuildError(result, ERR_MSG_REFRESH_TOKEN_IS_VALID, nameof(refreshToken));
            }

            var user = await _appUserRepository.GetAsync(u => u.RefreshToken == refreshToken && u.IsActive == AuthendicateConstain.ACTIVE);

            if (user == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(user));
            }

            var role = await _roleRepository.GetAsync(r => r.Id == user.RoleId);
            var token = GenerateJwtToken(user.Id.ToString(), user.UserName, role.Name);

            var dtoRefreshToken = new DtoRefreshToken
            {
                AccessToken = token.AccessToken,
                ExpiresAccess = token.ExpiresAccess,
            };

            return await BuildResult(result, dtoRefreshToken, MSG_SAVE_SUCCESSFULLY);
        }


        /// <summary>
        /// Comfirms the email asynchronous.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<string>> ComfirmEmailAsync(string UserId)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(UserId, out ObjectId objUserId))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(UserId));
            }

            var userResult = await _appUserRepository.GetAsync(u => u.Id == objUserId);
            if (userResult == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(UserId));
            }

            userResult.IsEmailConfirmed = true;
            userResult.SetUpdatedInFo(UserId, userResult.UserName);

            _appUserRepository.Update(userResult, u => u.Id == userResult.Id);

            return await BuildResult(result, MSG_UPDATE_SUCCESSFULLY);
        }

        #region Private method
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
                if (line[i].Contains(register.UserName.ToLower()))
                {
                    return await BuildError(result, ERR_MSG_USERNAME_IS_VALID, nameof(register.UserName));
                }

                if (line[i].Contains(register.Password.ToLower()))
                {
                    return await BuildError(result, ERR_MSG_USERNAME_IS_VALID, nameof(register.Password));
                }

                if (line[i].Contains(register.FullName.ToLower()))
                {
                    return await BuildError(result, ERR_MSG_USERNAME_IS_VALID, nameof(register.FullName));
                }
            }

            return await BuildResult(result, MSG_FIND_SUCCESSFULLY);
        }

        private DtoAuthent GenerateJwtToken(string UserId, string UserName, string Role)
        {
            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, UserId),
               new Claim(ClaimTypes.Name, UserName),
               new Claim(ClaimTypes.Role,Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[ConfigAppsettingConstaint.TokenKey]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config[ConfigAppsettingConstaint.TokenIssuer],
                _config[ConfigAppsettingConstaint.TokenIssuer],
                claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds);

            var dtoAppUser = new DtoAuthent();
            dtoAppUser.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            dtoAppUser.ExpiresAccess = DateTime.UtcNow.AddHours(1);

            return dtoAppUser;
        }

        private DtoAuthent GenerateRefresh(string UserId, string UserName, string Role)
        {
            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, UserId),
               new Claim(ClaimTypes.Name, UserName),
               new Claim(ClaimTypes.Role,Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[ConfigAppsettingConstaint.TokenKey]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config[ConfigAppsettingConstaint.TokenIssuer],
                _config[ConfigAppsettingConstaint.TokenIssuer],
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            var dtoAppUser = new DtoAuthent();
            dtoAppUser.RefreshToken = new JwtSecurityTokenHandler().WriteToken(token);
            dtoAppUser.ExpiresRefresh = token.ValidTo;

            return dtoAppUser;
        }


        private bool IsValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            var jwtToken = new JwtSecurityToken(token);

            if (jwtToken == null) return false;


            if (jwtToken.ValidTo.ToLocalTime() < DateTime.UtcNow) return false;

            return true;
        }

        #endregion Private method
    }
}
