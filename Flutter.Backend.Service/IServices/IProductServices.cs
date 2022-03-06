using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IProductServices
    {
        Task<AppActionResultMessage<DtoProduct>> Add(CreateRequestProduct request);

        Task<AppActionResultMessage<DtoProduct>> Update(UpdateRequestProduct request);

        Task<AppActionResultMessage<DtoProduct>> Delete(string request);

        Task<AppActionResultMessage<IList<DtoProduct>>> Search(SearchRequestProduct request);

        Task<AppActionResultMessage<IList<DtoProduct>>> GetAll();

        Task<AppActionResultMessage<DtoProduct>> Get(string request);
    }
}
