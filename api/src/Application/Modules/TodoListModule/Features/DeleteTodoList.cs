using System.Threading;
using System.Threading.Tasks;
using api.Persistence;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using api.Application.Modules.TodoListModule.Domain.Models;
using api.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace api.Application.Modules.TodoListModule.Features
{
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

        public Task<int> Handle(DeleteTodoListByIdCommand cmd, CancellationToken cancellationToken)
        {
            var listToBeDeleted = _db.TodoLists.Where(l => l.Id == cmd.Id).SingleOrDefault();
            if (listToBeDeleted == null)
                throw new EntityNotFoundException(nameof(TodoList), cmd.Id);

            _db.TodoLists.Remove(listToBeDeleted);
            _db.SaveChanges();
            _logger.LogInformation($"TodoList {listToBeDeleted.Name} was deleted");
            return Task.FromResult(listToBeDeleted.Id);
        }

    }
}