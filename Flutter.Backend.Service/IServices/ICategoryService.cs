using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface ICategoryService
    {

        Task<AppActionResultMessage<IEnumerable<DtoCategory>>> GetAllCategoriesAsync();
    }
}
