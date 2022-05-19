using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IBrandService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<IEnumerable<DtoBrand>>> GetAllBrandByIdCategoryAsync(string CategoryId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<string>> CreateBrandAsync(BaseBrandRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<string>> UpdateBrandsAsync(UpdateBrandRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<string>> DeleteBrandAsync(string CategoryId);
    }
}
