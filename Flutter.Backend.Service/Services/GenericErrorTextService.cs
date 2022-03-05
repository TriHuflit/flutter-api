using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Services
{
    public class GenericErrorTextService<T>
    {
        private readonly IMessageService _messageService;

        public GenericErrorTextService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<AppActionResultMessage<IList<T>>> BuildResult(AppActionResultMessage<IList<T>> result, IEnumerable<T> data, string key)
        {
            var resultMessage = await _messageService.GetMessage(key);           
            result.BuildResult(data, resultMessage);          
            return result;
        }   


        public async Task<AppActionResultMessage<IList<T>>> BuildError(AppActionResultMessage<IList<T>> result, string key)
        {
            var errorMessage =await _messageService.GetMessage(key);          
            result.BuildError(errorMessage);

            return result;
        }

    }
}
