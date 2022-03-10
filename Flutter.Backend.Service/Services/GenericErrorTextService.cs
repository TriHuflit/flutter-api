using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Services
{
    public class GenericErrorTextService
    {
        private readonly IMessageService _messageService;

        public GenericErrorTextService(IMessageService messageService)
        {
            _messageService = messageService;
        }


        public async Task<AppActionResultMessage<T>> BuildResult<T>(AppActionResultMessage<T> result, string data, string key)
        {
            var resultMessage = await _messageService.GetMessage(key);
            result.BuildResult(data, resultMessage);
            return result;
        }

        public async Task<AppActionResultMessage<T>> BuildResult<T>(AppActionResultMessage<T> result, T data, string key)
        {
            var resultMessage = await _messageService.GetMessage(key);
            result.BuildResult(data, resultMessage);
            return result;
        }

        public async Task<AppActionResultMessage<T>> BuildResult<T>(AppActionResultMessage<T> result,string key)
        {
            var resultMessage = await _messageService.GetMessage(key);
            result.BuildResult(resultMessage);
            return result;
        }

        public async Task<AppActionResultMessage<IList<T>>> BuildResult<T>(AppActionResultMessage<IList<T>> result, IEnumerable<T> data, string key)
        {
            var resultMessage = await _messageService.GetMessage(key);           
            result.BuildResult(data, resultMessage);          
            return result;
        }

     


        public async Task<AppActionResultMessage<IList<T>>> BuildError<T>(AppActionResultMessage<IList<T>> result, string key)
        {
            var errorMessage =await _messageService.GetMessage(key);          
            result.BuildError(errorMessage);

            return result;
        }

        public async Task<AppActionResultMessage<T>> BuildError<T>(AppActionResultMessage<T> result, string key)
        {
            var errorMessage = await _messageService.GetMessage(key);
            result.BuildError(errorMessage);

            return result;
        }

        public async Task<AppActionResultMessage<T>> BuildError<T>(AppActionResultMessage<T> result, string key,params object[] objectError)
        {
            var errorMessage = await _messageService.GetMessage(key);
            errorMessage = string.Format(errorMessage, objectError);
            result.BuildError(errorMessage);

            return result;
        }
    }
}
