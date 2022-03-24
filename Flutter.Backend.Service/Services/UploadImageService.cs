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
using System.Text.RegularExpressions;
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
                File = new FileDescription(image)
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
            var imagebase64 = IsBase64(image);
            if (!imagebase64)
            {
                return await BuildError(result, ERR_MSG_INVALID_BASE64_STRING, nameof(image));
            }

            Match match = DataImage.DataUriPattern.Match(image);
            string base64Data = match.Groups["data"].Value;
            var imageData = Convert.FromBase64String(base64Data);
            if (imageData == null)
            {
                return await BuildError(result, ERR_MSG_INVALID_BASE64_STRING, nameof(image));
            }

            var imageOversize = ConvertSizeToMB(image.Length) > 5;
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

        public static bool IsBase64(string base64String)
        {
            DataImage image = DataImage.TryParse(base64String);
            if (image == null) return false;

            return true;
        }

        #region private Class
        public sealed class DataImage
        {
            public static readonly Regex DataUriPattern = new Regex(@"^data\:(?<type>image\/(png|tiff|jpg|jpeg|gif|webp));base64,(?<data>[A-Z0-9\+\/\=]+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

            private DataImage()
            {
            }

            public static DataImage TryParse(string dataUri)
            {
                if (string.IsNullOrWhiteSpace(dataUri)) return null;

                Match match = DataUriPattern.Match(dataUri);
                if (!match.Success) return null;

                string base64Data = match.Groups["data"].Value;

                try
                {
                    byte[] rawData = Convert.FromBase64String(base64Data);
                    return rawData.Length == 0 ? null : new DataImage();
                }
                catch (FormatException)
                {
                    return null;
                }
            }
        }

        #endregion private Class

    }
}
