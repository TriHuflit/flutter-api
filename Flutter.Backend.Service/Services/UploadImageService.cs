using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Settings;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class UploadImageService : GenericErrorTextService, IUploadImageService
    {
        private Cloudinary _account;

        public UploadImageService(ICloundinarySetting cloundinarySetting,
            IMessageService messageService) : base(messageService)
        {
            var myAccount = new Account
            {
                ApiKey = cloundinarySetting.ApiKey,
                ApiSecret = cloundinarySetting.ApiSecret,
                Cloud = cloundinarySetting.CloudName
            };

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

            if (uploadResult == null)
            {
                return await BuildError(result, ERR_MSG_EMPTY_DATA);
            }


            return await BuildResult(result, uploadResult.Url.ToString(), MSG_SAVE_SUCCESSFULLY);
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

            var imgIcon = BytesToImage(imageData, out IImageFormat format);
            if (imgIcon == null)
            {
                return await BuildError(result, ERR_MSG_IMAGE_INVALID_WITH_BYTE_TYPE);
            }

            var imgFormat = format.Name.ToLower();
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

        private Image BytesToImage(byte[] imgBytes, out IImageFormat format)
        {
            Stream outputStream = new MemoryStream();

            using (var image = Image.Load(imgBytes, out format))
            {
                image.Mutate(c => c.Grayscale());
                image.Save(outputStream, format);


                return image;
            }
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
