using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface ITodoListService
    {
        Task<AppActionResultMessage<IEnumerable<DtoTodoList>>> GetAllTodoListAsync();

        Task<AppActionResultMessage<string>> CreateTodoListAsync(BaseTodoListRequest request);

        Task<AppActionResultMessage<string>> DeleteTodoListAsync(string todoListId);
    }
}
