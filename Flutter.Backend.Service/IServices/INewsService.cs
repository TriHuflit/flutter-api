using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface INewsService
    {

        Task<AppActionResultMessage<string>> CreateNewsAsync(BaseNewsRequest request);

        Task<AppActionResultMessage<string>> UpdateNewsAsync(UpdateNewsRequest request);

        Task<AppActionResultMessage<string>> DeleteNewsAsync(string newsId);

        Task<AppActionResultMessage<IEnumerable<DtoNews>>> GetAllNewsAsync();

        Task<AppActionResultMessage<DtoNews>> GetNewsAsync(string newsId);

        Task<AppActionResultMessage<IEnumerable<DtoNews>>> GetAllNewsMobileAsync();

        Task<AppActionResultMessage<DtoNews>> GetNewsMobileAsync(string newsId);
    }
}
