using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IFeedBackService
    {
        Task<AppActionResultMessage<string>> CreateFeedBack(CreateFeedBackRequest request);

        Task<AppActionResultMessage<IEnumerable<DtoFeedBack>>> GetAllFeedBackByIdProductAsync(string productId);

        Task<AppActionResultMessage<IEnumerable<DtoFeedBack>>> GetAllFeedBackAsync();

        Task<AppActionResultMessage<IEnumerable<DtoFeedBack>>> GetDetailFeedBackAsync(string feedBackId);
    }
}
