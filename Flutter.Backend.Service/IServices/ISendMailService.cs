using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Requests;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface ISendMailService
    {
        Task<AppActionResultMessage<string>> SendMailRegisterAsync(MailRequest request);
    }
}
