using api.Modules.TodoListModule.Domain.Models;
namespace api.Modules.TodoListModule.Features;
public record CheckItemCommand(int Id, bool Done) : IRequest<int>;
public class CheckItemHandler : IRequestHandler<CheckItemCommand, int>
{
    private readonly TodoListContext _db;
    private readonly ILogger<CheckItemHandler> _logger;
    public CheckItemHandler(TodoListContext db, ILogger<CheckItemHandler> logger)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<int> Handle(CheckItemCommand cmd, CancellationToken cancellationToken)
    {
        var item = await _db.TodoListItems.Where(i => i.Id == cmd.Id).SingleOrDefaultAsync();
        if (item == null)
            throw new EntityNotFoundException(nameof(TodoListItem), cmd.Id);

        item.Done = cmd.Done;
        _db.TodoListItems.Update(item);
        await _db.SaveChangesAsync();
        _logger.LogInformation($"item(id={cmd.Id}).done changed to {cmd.Done}");
        return item.Id;
    }

}
