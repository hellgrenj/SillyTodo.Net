using System.Threading;
using System.Threading.Tasks;
using api.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using api.Application.Modules.TodoListModule.Domain.Models;

namespace api.Application.Modules.TodoListModule.Features
{

    public record GetAllTodoListsQuery() : IRequest<List<TodoList>>;
    public class GetAllTodoListsHandler : IRequestHandler<GetAllTodoListsQuery, List<TodoList>>
    {
        private readonly TodoListContext _db;
        public GetAllTodoListsHandler(TodoListContext db) => _db = db;

        public Task<List<TodoList>> Handle(GetAllTodoListsQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(_db.TodoLists.ToList());
        }
    }
}