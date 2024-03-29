
using api.Modules.TodoListModule.Domain.Models;
namespace api.Modules.TodoListModule.Features;
public record AddItemToListCommand(int ListId, string Name, bool Done) : IRequest<int>;
public class AddItemToListCommandValidator : AbstractValidator<AddItemToListCommand>
{
    public AddItemToListCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
public class AddItemToListHandler : IRequestHandler<AddItemToListCommand, int>
{
    private readonly TodoListContext _db;
    private readonly ILogger<AddItemToListHandler> _logger;

    public AddItemToListHandler(TodoListContext db, ILogger<AddItemToListHandler> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task<int> Handle(AddItemToListCommand cmd, CancellationToken cancellationToken)
    {
        var list = await _db.TodoLists.Where(l => l.Id == cmd.ListId).SingleOrDefaultAsync();
        if (list == null)
            throw new EntityNotFoundException(nameof(TodoList), cmd.ListId);

        var itemTobeAdded = new TodoListItem { Name = cmd.Name, Done = cmd.Done };
        list.AddItem(itemTobeAdded);
        await _db.SaveChangesAsync();
        _logger.LogInformation($"Added item {cmd.Name} to list {list.Name}");
        return itemTobeAdded.Id;
    }

}
