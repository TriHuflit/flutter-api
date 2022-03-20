using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.Service.IServices;

namespace Flutter.Backend.Service.Services
{
    public class WaterProofService : IWaterProofService
    {
        private readonly IWaterProofRepository _waterProofRepository;

        public WaterProofService(IWaterProofRepository waterProofRepository)
        {
            _waterProofRepository = waterProofRepository;
        }
    }
}
