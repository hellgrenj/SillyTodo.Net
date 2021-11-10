using api.Modules.TodoListModule.Domain.Models;

namespace api.Modules.TodoListModule.Features;
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
