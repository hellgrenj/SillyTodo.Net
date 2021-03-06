using System.Collections.Generic;
using System.Threading.Tasks;
using api.Modules.TodoListModule.Features;
using api.Modules.TodoListModule.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Modules.TodoListModule
{
    [ExceptionHandling]
    [ApiController]
    [Route("todolist")]
    public class Controller : ControllerBase
    {
        private readonly IMediator _mediator;
        public Controller(IMediator mediator) => _mediator = mediator;
        [HttpGet]
        public async Task<List<TodoList>> Get() => await _mediator.Send(new GetAllTodoListsQuery());

        [HttpGet("{id}")]
        public async Task<TodoList> Get(int id) => await _mediator.Send(new GetTodoListByIdQuery(id));

        [HttpDelete("{id}")]
        public async Task<int> Del(int id) => await _mediator.Send(new DeleteTodoListByIdCommand(id));

        [HttpPost]
        public async Task<int> CreateNewList(CreateNewTodoListCommand command) => await _mediator.Send(command);

        [HttpPost("item")]
        public async Task<int> AddItemToList(AddItemToListCommand command) => await _mediator.Send(command);

        [HttpPatch("item")]
        public async Task<int> CheckItem(CheckItemCommand command) => await _mediator.Send(command);

        [HttpDelete("item/{id}")]
        public async Task<int> DelItem(int id) => await _mediator.Send(new DeleteItemByIdCommand(id));
    }
}
