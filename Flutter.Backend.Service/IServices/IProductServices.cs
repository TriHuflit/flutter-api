using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IProductServices
    {
        public Task<AppActionResultMessage<DtoProduct>> Add(CreateProduct request);

        public Task<AppActionResultMessage<IList<DtoProduct>>> GetAll();
    }
}
