using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Flutter.Backend.Api.Controllers
{
    [Route("api/todo-list")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _todoListService;

        public TodoListController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }



        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllTodoListAsync()
        {
            var result = await _todoListService.GetAllTodoListAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTodoListAsync(BaseTodoListRequest request)
        {
            var result = await _todoListService.CreateTodoListAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete]
        [Route("delete/{todoId}")]
        public async Task<IActionResult> DeleteTodoListAsync(string todoId)
        {
            var result = await _todoListService.DeleteTodoListAsync(todoId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
