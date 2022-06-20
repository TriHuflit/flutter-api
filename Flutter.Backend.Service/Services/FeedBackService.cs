using Flutter.Backend.Service.IServices;
using Flutter.Backend.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Requests;
using Flutter.Backend.Service.Models.Dtos;
using static Flutter.Backend.Common.Constains.MessageResConstain;
using MongoDB.Bson;
using Flutter.Backend.Common.Constains;
using AutoMapper;

namespace Flutter.Backend.Service.Services
{
    public class FeedBackService : GenericErrorTextService, IFeedBackService
    {
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly IProductRepository _productRepository;

        private readonly ICurrentUserService _currentUserService;
        private readonly IUploadImageService _uploadImageService;
        private readonly IMapper _mapper;
        public FeedBackService(
            IFeedBackRepository feedBackRepository,
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IUploadImageService uploadImageService,
             IMessageService messageService) : base(messageService)
        {
            _feedBackRepository = feedBackRepository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _uploadImageService = uploadImageService;
            _mapper = mapper;
        }

        public async Task<AppActionResultMessage<string>> CreateFeedBack(CreateFeedBackRequest request)
        {
            var result = new AppActionResultMessage<string>();

            if (!CheckStart(request.Star))
            {
                return await BuildError(result, "Số sao không hợp lệ {0}", request.Star);
            }

            if (!ObjectId.TryParse(request.ProductId, out ObjectId objProduct))
            {
                return await BuildError(result, ERR_MSG_EMAIL_ISVALID_FORMART, request.ProductId);
            }

            var product = await _productRepository.GetAsync(p => p.Id == objProduct && p.IsShow == IsShowConstain.ACTIVE);
            if (product == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, request.ProductId);
            }

            var feedBack = new FeedBack
            {
                Content = request.Content,
                Star = request.Star,
                ProductId = objProduct,
                UserId =ObjectId.Parse(_currentUserService.UserId),
            };

            foreach(var image in request.ImageFeedBack)
            {
              
                if (!string.IsNullOrEmpty(image))
                {
                    var imageFeedBackResult = await _uploadImageService.UploadImage(image);
                    if (!imageFeedBackResult.IsSuccess)
                    {
                        return await BuildError(result, imageFeedBackResult.Message);
                    }
                    feedBack.ImageFeedBack.Add(imageFeedBackResult.Data);
                }
            }

            feedBack.SetCreatedInFor(_currentUserService.UserId, _currentUserService.UserName);

            return await BuildResult(result, MSG_SAVE_SUCCESSFULLY);
        }

        public Task<AppActionResultMessage<IEnumerable<DtoFeedBack>>> GetAllFeedBackAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResultMessage<IEnumerable<DtoFeedBack>>> GetAllFeedBackByIdProductAsync(string productId)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResultMessage<IEnumerable<DtoFeedBack>>> GetDetailFeedBackAsync(string feedBackId)
        {
            throw new NotImplementedException();
        }

        #region Private Method
        private bool CheckStart(int star)
        {
            return star > 0 && star <= 5;
        }
        #endregion Private Method
    }
}
