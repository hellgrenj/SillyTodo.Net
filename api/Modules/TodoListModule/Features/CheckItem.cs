using System.Threading;
using System.Threading.Tasks;
using api.Infrastructure.Persistence;
using MediatR;

using System.Linq;
using api.Exceptions;
using api.Modules.TodoListModule.Domain.Models;
using Microsoft.Extensions.Logging;

namespace api.Modules.TodoListModule.Feature
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

        public Task<int> Handle(CheckItemCommand cmd, CancellationToken cancellationToken)
        {
            var item = _db.TodoListItems.Where(i => i.Id == cmd.Id).SingleOrDefault();
            if (item == null)
                throw new EntityNotFoundException(nameof(TodoListItem), cmd.Id);

            item.Done = cmd.Done;
            _db.TodoListItems.Update(item);
            _db.SaveChanges();
            _logger.LogInformation($"item(id={cmd.Id}).done changed to {cmd.Done}");
            return Task.FromResult(item.Id);
        }

    }
}