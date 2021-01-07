using System.Threading;
using System.Threading.Tasks;
using api.Infrastructure.Persistence;
using MediatR;

using System;
using System.Linq;
using FluentValidation;
using api.Exceptions;
using api.Modules.TodoListModule.Domain.Models;
using Microsoft.Extensions.Logging;

namespace api.Modules.TodoListModule.Feature
{
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

        public Task<int> Handle(DeleteItemByIdCommand cmd, CancellationToken cancellationToken)
        {
            var itemToBeDeleted = _db.TodoListItems.Where(i => i.Id == cmd.Id).SingleOrDefault();
            if (itemToBeDeleted == null)
                throw new EntityNotFoundException(nameof(TodoListItem), cmd.Id);

            _db.TodoListItems.Remove(itemToBeDeleted);
            _db.SaveChanges();
            _logger.LogInformation($"Item with id {cmd.Id} was deleted");
            return Task.FromResult(itemToBeDeleted.Id);
        }
    }
}