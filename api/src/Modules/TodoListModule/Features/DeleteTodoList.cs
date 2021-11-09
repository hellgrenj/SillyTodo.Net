using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using api.Modules.TodoListModule.Domain.Models;
using Microsoft.Extensions.Logging;
using api.Modules.TodoListModule.Exceptions;

namespace api.Modules.TodoListModule.Features;
public record DeleteTodoListByIdCommand(int Id) : IRequest<int>;

public class DeleteTodoListByIdHandler : IRequestHandler<DeleteTodoListByIdCommand, int>
{
    private readonly TodoListContext _db;
    private readonly ILogger<DeleteTodoListByIdHandler> _logger;
    public DeleteTodoListByIdHandler(TodoListContext db, ILogger<DeleteTodoListByIdHandler> logger)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<int> Handle(DeleteTodoListByIdCommand cmd, CancellationToken cancellationToken)
    {
        var listToBeDeleted = await _db.TodoLists.Where(l => l.Id == cmd.Id).SingleOrDefaultAsync();
        if (listToBeDeleted == null)
            throw new EntityNotFoundException(nameof(TodoList), cmd.Id);

        _db.TodoLists.Remove(listToBeDeleted);
        await _db.SaveChangesAsync();
        _logger.LogInformation($"TodoList {listToBeDeleted.Name} was deleted");
        return listToBeDeleted.Id;
    }

}
