using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using System.Web;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using static Flutter.Backend.Common.Contains.MessageResContains;
using Flutter.Backend.Service.Settings;
using System.Drawing;
using System.IO;
using System;
using Flutter.Backend.Common.Contains;

namespace Flutter.Backend.Service.Services
{
    public class UploadImageService : GenericErrorTextService , IUploadImageService
    {
        private Cloudinary _account;

        public UploadImageService(ICloundinarySetting  cloundinarySetting, 
            IMessageService messageService) : base(messageService)
        {
            var myAccount = new Account { ApiKey = cloundinarySetting.ApiKey ,
                ApiSecret = cloundinarySetting.ApiSecret , Cloud  = cloundinarySetting.CloudName};

            _account = new Cloudinary(myAccount);


        }



        public async Task<AppActionResultMessage<string>> UploadImage(string image)
        {
            var result = new AppActionResultMessage<string>();

            var validateImage = await isValidUploadImage(image);

            if (!validateImage.IsSuccess)
            {
                return await BuildError(result, validateImage.Message);
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Base64ToImage(image))
            };

            var uploadResult = await _account.UploadLargeAsync(uploadParams);
            
            if(uploadResult == null)
            {
                return await BuildError(result,ERR_MSG_EMPTY_DATA);
            }


            return await BuildResult(result, uploadResult.Url.ToString() ,MSG_SAVE_SUCCESSFULLY);
        }

        private async Task<AppActionResultMessage<string>> isValidUploadImage(string image)
        {
            var result = new AppActionResultMessage<string>();
            var imagebase64 = IsBase64String(image);
            if (!imagebase64)
            {
                return await BuildError(result, ERR_MSG_INVALID_BASE64_STRING, nameof(image));
            }

            var imageData = Convert.FromBase64String(image);
            if (imageData == null)
            {
                return await BuildError(result, ERR_MSG_INVALID_BASE64_STRING, nameof(image));
            }

            var imageOversize = ConvertSizeToMB(imageData.Length) > 5;
            if (imageOversize)
            {
                return await BuildError(result, ERR_MSG_UPLOAD_FILE_SIZE_OVER_MAXIMUM);
            }

            var imgIcon = BytesToImage(imageData);
            if (imgIcon == null)
            {
                return await BuildError(result, ERR_MSG_IMAGE_INVALID_WITH_BYTE_TYPE);
            }

            var imgFormat = imgIcon.RawFormat.ToString().ToLower();
            if (imgFormat != ImageFormatConstant.JPEG && imgFormat != ImageFormatConstant.PNG)
            {
                return await BuildError(result, ERR_MSG_INVALID_IMAGE_DATA);
            }

            return await BuildResult(result, MSG_SAVE_SUCCESSFULLY); 
        }

        private double ConvertSizeToMB(int dataSizeInBytes)
        {
            return (double)dataSizeInBytes / 1048576.0;
        }

        private Bitmap BytesToImage(byte[] imgBytes)
        {
            Bitmap result = null;
            if (imgBytes != null)
            {
                MemoryStream stream = new MemoryStream(imgBytes);
                result = (Image.FromStream(stream) as Bitmap);
            }
            return result;
        }

        private string Base64ToImage(string image)
        {
            return "data:image/jpeg;base64," + image;
        }

        private static bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }
    }
}
