using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IProductServices
    {
        Task<AppActionResultMessage<DtoProduct>> CreateProductAsync(CreateRequestProduct request);

        Task<AppActionResultMessage<string>> UpdateProductAsync(UpdateRequestProduct request);

        Task<AppActionResultMessage<string>> DeleteProductAsync(string productId);

        Task<AppActionResultMessage<IList<DtoProduct>>> SearchProductAsync(SearchRequestProduct request);

        Task<AppActionResultMessage<IList<DtoProduct>>> GetAllProductAsync();

        Task<AppActionResultMessage<DtoProduct>> GetProductAsync(string productId);
    }
}
