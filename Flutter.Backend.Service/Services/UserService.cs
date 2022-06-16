using AutoMapper;
using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using MongoDB.Bson;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class UserService : GenericErrorTextService, IUserService
    {
        private readonly IAppUserRepository _appUserRepository;

        private readonly ICurrentUserService _currentUserService;
        private readonly IUploadImageService _uploadImageService;
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;

        public UserService(
            IAppUserRepository appUserRepository,
             IMessageService messageService,
              IMapper mapper,
             ICurrentUserService currentUserService,
             IUploadImageService uploadImageService,
             IValidationService validationService) : base(messageService)
        {
            _appUserRepository = appUserRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _uploadImageService = uploadImageService;
            _validationService = validationService;
        }

        public async Task<AppActionResultMessage<DtoUser>> GetInfoUserAsync()
        {
            var result = new AppActionResultMessage<DtoUser>();

            var user = await _appUserRepository.GetAsync(u => u.Id == ObjectId.Parse(_currentUserService.UserId) && u.IsActive == IsShowConstain.ACTIVE);

            if (user == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, user);
            }

            var dtoUser = _mapper.Map<AppUser, DtoUser>(user);

            return await BuildResult(result, dtoUser, MSG_FIND_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<string>> UpdateAvatarAsync(UpdateAvatarRequest request)
        {
            var result = new AppActionResultMessage<string>();

            var user = await _appUserRepository.GetAsync(u => u.Id == ObjectId.Parse(_currentUserService.UserId) && u.IsActive == IsShowConstain.ACTIVE);
            if (user == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, user);
            }

            if (!string.IsNullOrEmpty(request.Avatar) && request.Avatar != user.Avatar)
            {
                var imageResult = await _uploadImageService.UploadImage(request.Avatar);
                if (!imageResult.IsSuccess)
                {
                    return await BuildError(result, imageResult.Message);
                }
                user.Avatar = imageResult.Data;
                user.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
                _appUserRepository.Update(user, u => u.Id == user.Id);
            }

            return await BuildResult(result, MSG_SAVE_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<string>> UpdateInfoUserAsync(UpdateUserRequest request)
        {
            var result = new AppActionResultMessage<string>();

            var user = await _appUserRepository.GetAsync(u => u.Id == ObjectId.Parse(_currentUserService.UserId) && u.IsActive == IsShowConstain.ACTIVE);
            if (user == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, user);
            }


            if (!string.IsNullOrEmpty(request.FullName))
            {
                user.FullName = request.FullName;
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                if (!_validationService.ValidateEmailFormat(request.Email))
                {
                    return await BuildError(result, ERR_MSG_EMAIL_ISVALID_FORMART);
                }

                if (request.Email != user.Email)
                {
                    var isExistEmail = await _appUserRepository.FindByAsync(u => u.Email == request.Email
                                                                            && u.IsActive == IsShowConstain.ACTIVE);
                    if (isExistEmail != null)
                    {
                        return await BuildError(result, ERR_MSG_EMAIL_IS_EXIST, request.Email);
                    }
                }
                user.Email = request.Email;
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                if (!_validationService.ValidatePhoneNumberFormat(request.Phone))
                {
                    return await BuildError(result, ERR_MSG_PHONE_ISVALID_FORMART);
                }

                if (!_validationService.ValidateVnPhoneNumberFormat(request.Phone))
                {
                    return await BuildError(result, ERR_MSG_PHONE_ISVALID_FORMART_VN);
                }

                if (request.Phone != user.Phone)
                {
                    var isExistPhone = await _appUserRepository.FindByAsync(u => u.Phone == request.Phone
                                                                           && u.IsActive == IsShowConstain.ACTIVE);
                    if (isExistPhone != null)
                    {
                        return await BuildError(result, ERR_MSG_PHONE_IS_EXIST, request.Phone);
                    }
                }
                user.Phone = request.Phone;
            }

            if (!string.IsNullOrEmpty(request.Gender))
            {
                if (!ValidateGender(request.Gender))
                {
                    return await BuildError(result, "Giới tính {0} không hợp lệ", request.Gender);
                }
                user.Gender = request.Gender;
            }

            if (request.Birth > DateTime.UtcNow)
            {
                return await BuildError(result, "Ngày sinh {0} không được quá ngày hiện tại", request.Birth);
            }
            var Age = DateTime.UtcNow.Year - request.Birth.Year;
            if (Age <= 16)
            {
                return await BuildError(result, "Ngày sinh {0} không hợp lệ (Tuổi phải từ 16 trở lên)", request.Birth);
            }
            user.Birth = request.Birth;

            //Location
            if (request.Location == null || string.IsNullOrEmpty(request.Location.Province)
                || string.IsNullOrEmpty(request.Location.District) || string.IsNullOrEmpty(request.Location.Ward)
                || string.IsNullOrEmpty(request.Location.Address))
            {
                return await BuildError(result, "Địa chỉ không được để trống");
            }

            if (!string.IsNullOrEmpty(request.Location.Province))
            {
                user.Location.Province = request.Location.Province;
            }

            if (!string.IsNullOrEmpty(request.Location.District))
            {
                user.Location.District = request.Location.District;
            }

            if (!string.IsNullOrEmpty(request.Location.Ward))
            {
                user.Location.Ward = request.Location.Ward;
            }

            if (!string.IsNullOrEmpty(request.Location.Address))
            {
                user.Location.Address = request.Location.Address;
            }

            user.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _appUserRepository.Update(user, u => u.Id == user.Id);

            return await BuildResult(result, MSG_SAVE_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<string>> UpdatePassWordAsync(UpdatePasswordRequest request)
        {
            var result = new AppActionResultMessage<string>();

            var user = await _appUserRepository.GetAsync(u => u.Id == ObjectId.Parse(_currentUserService.UserId) && u.HashPassword == HashPassWord(request.OldPassword)
            && u.IsActive == IsShowConstain.ACTIVE);
            if (user == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, user);
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return await BuildError(result, ERR_MSG_PASSWORD_IS_NOT_SAME);
            }

            if (!_validationService.ValidatePasswordFormat(request.NewPassword))
            {
                return await BuildError(result, ERR_MSG_PASSWORD_ISVALID_FORMART);
            }

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                var hashPassword = HashPassWord(request.NewPassword);
                user.HashPassword = hashPassword;
                user.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
                _appUserRepository.Update(user, u => u.Id == user.Id);
            }

            return await BuildResult(result, MSG_SAVE_SUCCESSFULLY);
        }


        #region Private Method
        private string HashPassWord(string password)
        {
            RSACryptoServiceProvider rsa = new();
            string pubkey = rsa.ToXmlString(false);
            string prikey = rsa.ToXmlString(true);
            byte[] someThing = RSAEncrypt(Encoding.Unicode.GetBytes(password), pubkey);
            byte[] anotherThing = RSADecrypt(someThing, prikey);

            return Convert.ToBase64String(anotherThing);
        }

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


        public bool ValidateGender(string Gender)
        {
            return Gender == "Nam" || Gender == "Nữ";
        }
        #endregion Private Method
    }
}
