using System.Threading;
using System.Threading.Tasks;
using api.Persistence;
using MediatR;
using System.Linq;
using api.Application.Exceptions;
using api.Application.Modules.TodoListModule.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace api.Application.Modules.TodoListModule.Features
{
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
}