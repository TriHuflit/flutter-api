using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.Service.IServices;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
    }
}
