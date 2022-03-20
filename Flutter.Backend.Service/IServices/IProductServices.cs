﻿using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IProductServices
    {
        Task<AppActionResultMessage<string>> CreateProductAsync(CreateRequestProduct request);

        Task<AppActionResultMessage<string>> UpdateProductAsync(UpdateRequestProduct request);

        Task<AppActionResultMessage<string>> DeleteProductAsync(string productId);

        Task<AppActionResultMessage<IEnumerable<DtoProduct>>> SearchProductAsync(SearchRequestProduct request);

        Task<AppActionResultMessage<IEnumerable<DtoProduct>>> GetAllProductAsync(PaginationRequest request);

        Task<AppActionResultMessage<DtoProductDetail>> GetProductAsync(string productId);
    }
}
