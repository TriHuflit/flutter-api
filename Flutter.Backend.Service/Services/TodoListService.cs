using AutoMapper;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class TodoListService : GenericErrorTextService, ITodoListService
    {
        private readonly ITodoListRepository _todoListRepository;

        private readonly IMapper _mapper;
        public TodoListService(ITodoListRepository todoListRepository,
            IMapper mapper, IMessageService messageService) : base(messageService)
        {
            _todoListRepository = todoListRepository;
            _mapper = mapper;
        }

        public async Task<AppActionResultMessage<string>> CreateTodoListAsync(BaseTodoListRequest request)
        {
            var result = new AppActionResultMessage<string>();

            var todoList = new Todolist
            {
                Title = request.Title,
                status = true
            };

            _todoListRepository.Add(todoList);

            return await BuildResult(result, todoList.Id.ToString() , MSG_SAVE_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<string>> DeleteTodoListAsync(string todoListId)
        {
            var result = new AppActionResultMessage<string>();

            if (!ObjectId.TryParse(todoListId, out ObjectId objTodoList))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(todoListId));
            }

            var todoList = await _todoListRepository.GetAsync(t => t.Id == objTodoList);
            if(todoList == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, todoListId);
            }
            _todoListRepository.DeleteAll(t => t.Id == objTodoList);

            return await BuildResult(result, todoList.Id.ToString(), MSG_SAVE_SUCCESSFULLY);
        }

        public async Task<AppActionResultMessage<IEnumerable<DtoTodoList>>> GetAllTodoListAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoTodoList>>();

            var todoList = await _todoListRepository.GetAll();

            var dtoTodoList = _mapper.Map< IEnumerable<Todolist>, IEnumerable<DtoTodoList>>(todoList);

            return await BuildResult(result, dtoTodoList, MSG_FIND_SUCCESSFULLY);
        }
    }
}
