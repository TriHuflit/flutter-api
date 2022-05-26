using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface ICategoryService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<AppActionResultMessage<IEnumerable<DtoCategory>>> GetAllCategoriesAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<DtoCategory>> GetDetailCategoryAsync(string CategoryId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<string>> CreateCategoriesAsync(BaseCategoryRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<string>> UpdateCategoriesAsync(UpdateCategoryRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        Task<AppActionResultMessage<string>> DeleteCategoriesAsync(string CategoryId);

    }
}
