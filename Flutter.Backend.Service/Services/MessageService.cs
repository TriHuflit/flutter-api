using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.Service.IServices;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Services
{
    public class MessageService : IMessageService
    {
        IMessageRepository _messageRespository;

        public MessageService(IMessageRepository messageRespository)
        {
            _messageRespository = messageRespository;
        }

        public async Task<string> GetMessage(string key)
        {
            var result = await _messageRespository.GetAsync(x=>x.Key==key);
            if(result == null)
            {
                return key;
            }
            return  result.MessageResponse;
        }
    }
}
