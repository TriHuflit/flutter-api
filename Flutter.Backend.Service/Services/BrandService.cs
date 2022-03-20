using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.Service.IServices;

namespace Flutter.Backend.Service.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
    }
}
