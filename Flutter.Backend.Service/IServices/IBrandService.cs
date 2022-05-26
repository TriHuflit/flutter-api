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
        /// Get all brand by category id
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<IEnumerable<DtoBrand>>> GetAllBrandByIdCategoryAsync(string CategoryId);


        /// <summary>
        /// Get detail brand by category id
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<DtoBrand>> GetDetailBrandByIdCategoryAsync(string CategoryId);
        /// <summary>
        /// Get all brand
        /// </summary>
        /// <returns></returns>
        Task<AppActionResultMessage<IEnumerable<DtoBrand>>> GetAllBrandAsync();

        /// <summary>
        /// Create new brand
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<string>> CreateBrandAsync(BaseBrandRequest request);

        /// <summary>
        /// update brand
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<string>> UpdateBrandsAsync(UpdateBrandRequest request);

        /// <summary>
        /// delete brand
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<string>> DeleteBrandAsync(string CategoryId);
    }
}
