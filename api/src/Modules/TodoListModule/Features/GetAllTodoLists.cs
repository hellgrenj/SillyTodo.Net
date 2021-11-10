using api.Modules.TodoListModule.Domain.Models;
namespace api.Modules.TodoListModule.Features;

public record GetAllTodoListsQuery() : IRequest<List<TodoList>>;
public class GetAllTodoListsHandler : IRequestHandler<GetAllTodoListsQuery, List<TodoList>>
{
    private readonly TodoListContext _db;
    public GetAllTodoListsHandler(TodoListContext db) => _db = db;

    public async Task<List<TodoList>> Handle(GetAllTodoListsQuery query, CancellationToken cancellationToken)
    {
        return await _db.TodoLists.ToListAsync();
    }
}
