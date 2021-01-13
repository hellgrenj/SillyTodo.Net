using System.Threading;
using System.Threading.Tasks;
using api.Persistence;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using api.Application.Modules.TodoListModule.Domain.Models;
using api.Application.Exceptions;

namespace api.Application.Modules.TodoListModule.Features
{
    public record GetTodoListByIdQuery(int Id) : IRequest<TodoList>;
    public class GetTodoListByIdHandler : IRequestHandler<GetTodoListByIdQuery, TodoList>
    {
        private readonly TodoListContext _db;

        public GetTodoListByIdHandler(TodoListContext db) => _db = db;

        public Task<TodoList> Handle(GetTodoListByIdQuery cmd, CancellationToken cancellationToken)
        {
            var list = _db.TodoLists.Include(l => l.Items).Where(l => l.Id == cmd.Id).SingleOrDefault();
            if (list == null)
                throw new EntityNotFoundException(nameof(TodoList), cmd.Id);

            return Task.FromResult(list);
        }


    }
}