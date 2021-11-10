using api.Modules.TodoListModule.Domain.Models;
namespace api.Modules.TodoListModule.Features;
public record DeleteItemByIdCommand(int Id) : IRequest<int>;
public class DeleteItemByIdHandler : IRequestHandler<DeleteItemByIdCommand, int>
{
    private readonly TodoListContext _db;
    private readonly ILogger<DeleteItemByIdHandler> _logger;
    public DeleteItemByIdHandler(TodoListContext db, ILogger<DeleteItemByIdHandler> logger)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<int> Handle(DeleteItemByIdCommand cmd, CancellationToken cancellationToken)
    {
        var itemToBeDeleted = await _db.TodoListItems.Where(i => i.Id == cmd.Id).SingleOrDefaultAsync();
        if (itemToBeDeleted == null)
            throw new EntityNotFoundException(nameof(TodoListItem), cmd.Id);

        _db.TodoListItems.Remove(itemToBeDeleted);
        await _db.SaveChangesAsync();
        _logger.LogInformation($"Item with id {cmd.Id} was deleted");
        return itemToBeDeleted.Id;
    }
}
