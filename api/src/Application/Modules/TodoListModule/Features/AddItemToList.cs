using System.Threading;
using System.Threading.Tasks;
using api.Persistence;
using MediatR;
using System.Linq;
using api.Application.Modules.TodoListModule.Domain.Models;
using FluentValidation;
using api.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace api.Application.Modules.TodoListModule.Features
{
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

        public Task<int> Handle(AddItemToListCommand cmd, CancellationToken cancellationToken)
        {
            var list = _db.TodoLists.Where(l => l.Id == cmd.ListId).SingleOrDefault();
            if (list == null)
                throw new EntityNotFoundException(nameof(TodoList), cmd.ListId);

            var itemTobeAdded = new TodoListItem { Name = cmd.Name, Done = cmd.Done };
            list.AddItem(itemTobeAdded);
            _db.SaveChanges();
            _logger.LogInformation($"Added item {cmd.Name} to list {list.Name}");
            return Task.FromResult(itemTobeAdded.Id);
        }

    }
}