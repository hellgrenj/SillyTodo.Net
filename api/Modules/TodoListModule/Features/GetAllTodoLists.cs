using System.Threading;
using System.Threading.Tasks;
using api.Infrastructure.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using api.Modules.TodoListModule.Domain.Models;

namespace api.Modules.TodoListModule.Feature
{

    public record GetAllTodoListsQuery () : IRequest<List<TodoList>>;
    public class GetAllTodoListsHandler : IRequestHandler<GetAllTodoListsQuery, List<TodoList>>
    {
        private readonly TodoListContext _db;

        public GetAllTodoListsHandler(TodoListContext db) => _db = db;

        public Task<List<TodoList>> Handle(GetAllTodoListsQuery cmd, CancellationToken cancellationToken)
        {
           return Task.FromResult(_db.TodoLists.ToList());
        }

      
    }
}