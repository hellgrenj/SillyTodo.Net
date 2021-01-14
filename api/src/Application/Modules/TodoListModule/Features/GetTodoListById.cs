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

        public async Task<TodoList> Handle(GetTodoListByIdQuery query, CancellationToken cancellationToken)
        {
            var list = await _db.TodoLists.Include(l => l.Items).Where(l => l.Id == query.Id).SingleOrDefaultAsync();
            if (list == null)
                throw new EntityNotFoundException(nameof(TodoList), query.Id);

            return list;
        }


    }
}