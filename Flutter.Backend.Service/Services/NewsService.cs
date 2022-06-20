using AutoMapper;
using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class NewsService : GenericErrorTextService, INewsService
    {
        private readonly INewsRepository _newsRepository;

        private readonly IMapper _mapper;

        private readonly IUploadImageService _uploadImageService;
        private readonly ICurrentUserService _currentUserService;

        public NewsService(
            IMapper mapper,
            IConfiguration config,
            IUploadImageService uploadImageService,
            ICurrentUserService currentUserService,
            INewsRepository newsRepository,
            IMessageService messageService) : base(messageService)
        {

            _newsRepository = newsRepository;
            _uploadImageService = uploadImageService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<AppActionResultMessage<string>> CreateNewsAsync(BaseNewsRequest request)
        {
            var result = new AppActionResultMessage<string>();

            if (!ValidateIsShow(request.IsShow))
            {
                return await BuildError(result, "Trạng thái bài viết không đúng");
            }

            var news = new News
            {
                Author = request.Author,
                Content = request.Content,
                Title = request.Title,
                IsShow = request.IsShow
            };

            var imageResult = await _uploadImageService.UploadImage(request.Thumbnail);
            if (!imageResult.IsSuccess)
            {
                return await BuildError(result, imageResult.Message);
            }
            news.Thumbnail = imageResult.Data;
            news.SetFullInfor(_currentUserService.UserId, _currentUserService.UserName);
            _newsRepository.Add(news);

            return await BuildResult(result, news.Id.ToString(), MSG_SAVE_SUCCESSFULLY);
        }



        public async Task<AppActionResultMessage<string>> DeleteNewsAsync(string newsId)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(newsId, out ObjectId objNews))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, newsId);
            }

            var news = await _newsRepository.GetAsync(n => n.Id == objNews && n.IsShow != IsShowConstain.DELETE);
            if (news == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, newsId);
            }

            news.IsShow = IsShowConstain.DELETE;
            news.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _newsRepository.Update(news, n => n.Id == objNews);

            return await BuildResult(result, news.Id.ToString(), MSG_DELETE_SUCCESSFULLY);
        }


        public async Task<AppActionResultMessage<IEnumerable<DtoNews>>> GetAllNewsAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoNews>>();

            var news = await _newsRepository.FindByAsync(n => n.IsShow != IsShowConstain.DELETE);

            var dtoNews = _mapper.Map<IEnumerable<News>, IEnumerable<DtoNews>>(news);

            return await BuildResult(result, dtoNews, MSG_FIND_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<IEnumerable<DtoNews>>> GetAllNewsMobileAsync()
        {

            var result = new AppActionResultMessage<IEnumerable<DtoNews>>();

            var news = await _newsRepository.FindByAsync(n => n.IsShow == IsShowConstain.ACTIVE);

            var dtoNews = _mapper.Map<IEnumerable<News>, IEnumerable<DtoNews>>(news);

            return await BuildResult(result, dtoNews, MSG_FIND_SUCCESSFULLY);
        }


        public async Task<AppActionResultMessage<DtoNews>> GetNewsAsync(string newsId)
        {
            var result = new AppActionResultMessage<DtoNews>();

            if (!ObjectId.TryParse(newsId, out ObjectId objNews))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, newsId);
            }

            var news = await _newsRepository.GetAsync(n => n.Id == objNews && n.IsShow != IsShowConstain.DELETE);
            if (news == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, newsId);
            }

            var dtoNews = _mapper.Map<News, DtoNews>(news);

            return await BuildResult(result, dtoNews, MSG_FIND_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<DtoNews>> GetNewsMobileAsync(string newsId)
        {
            var result = new AppActionResultMessage<DtoNews>();

            if (!ObjectId.TryParse(newsId, out ObjectId objNews))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, newsId);
            }

            var news = await _newsRepository.GetAsync(n => n.Id == objNews && n.IsShow == IsShowConstain.ACTIVE);
            if (news == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, newsId);
            }

            var dtoNews = _mapper.Map<News, DtoNews>(news);

            return await BuildResult(result, dtoNews, MSG_FIND_SUCCESSFULLY);
        }



        public async Task<AppActionResultMessage<string>> UpdateNewsAsync(UpdateNewsRequest request)
        {
            var result = new AppActionResultMessage<string>();


            if (!ObjectId.TryParse(request.Id, out ObjectId objNews))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, request.Id);
            }

            var news = await _newsRepository.GetAsync(n => n.Id == objNews && n.IsShow != IsShowConstain.DELETE);
            if (news == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, request.Id);
            }

            if (ValidateIsShow(request.IsShow))
            {
                news.IsShow = request.IsShow;
            }

            if (!string.IsNullOrEmpty(request.Content))
            {
                news.Content = request.Content;
            }

            if (!string.IsNullOrEmpty(request.Title))
            {
                news.Title = request.Title;
            }

            if (!string.IsNullOrEmpty(request.Author))
            {
                news.Author = request.Author;
            }

            if (!string.IsNullOrEmpty(request.Thumbnail) && request.Thumbnail != news.Thumbnail)
            {
                var imageResult = await _uploadImageService.UploadImage(request.Thumbnail);
                if (!imageResult.IsSuccess)
                {
                    return await BuildError(result, imageResult.Message);
                }
                news.Thumbnail = imageResult.Data;
            }

            news.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _newsRepository.Update(news, n => n.Id == news.Id);

            return await BuildResult(result, news.Id.ToString(), MSG_UPDATE_SUCCESSFULLY);
        }

        #region Private Method
        public bool ValidateIsShow(int IsShow)
        {
            return IsShow == IsShowConstain.ACTIVE || IsShow == IsShowConstain.INACTIVE;
        }
        #endregion Private Method

    }



}
