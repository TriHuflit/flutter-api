using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Flutter.Backend.Service.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IProductServices
    {
        Task<AppActionResultMessage<string>> CreateProductAsync(CreateProductRequest request);

        Task<AppActionResultMessage<string>> UpdateProductAsync(UpdateProductRequest request);

        Task<AppActionResultMessage<string>> DeleteProductAsync(string productId);

        Task<AppActionResultMessage<IEnumerable<DtoProduct>>> SearchProductAsync(SearchRequestProduct request);

        Task<AppActionResultMessage<SearchResultData>> GetAllProductAsync(PaginationRequest request);

        Task<AppActionResultMessage<DtoProductDetail>> GetProductAsync(string productId);

        Task<AppActionResultMessage<SearchResultData>> GetAllProductMobileAsync(PaginationRequest request);

        Task<AppActionResultMessage<DtoProductDetail>> GetProductMobileAsync(string productId);

        Task<AppActionResultMessage<SearchResultData>> GetProductPromotionAsync();

        Task<AppActionResultMessage<SearchResultData>> GetProductBestSellerAsync();
    }
}
