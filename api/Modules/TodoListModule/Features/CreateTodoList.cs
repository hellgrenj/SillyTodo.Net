using System.Threading;
using System.Threading.Tasks;
using api.Infrastructure.Persistence;
using MediatR;
using api.Modules.TodoListModule.Domain.Models;
using System;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace api.Modules.TodoListModule.Feature
{

    public record CreateNewTodoListCommand(string Name) : IRequest<int>;

    public class CreateNewTodoListCommandValidator : AbstractValidator<CreateNewTodoListCommand>
    {
        public CreateNewTodoListCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
    public class CreateTodoListHandler : IRequestHandler<CreateNewTodoListCommand, int>
    {
        private readonly TodoListContext _db;
        private readonly ILogger<CreateTodoListHandler> _logger;

        public CreateTodoListHandler(TodoListContext db, ILogger<CreateTodoListHandler> logger)
        {
            _logger = logger;
            _db = db;
        }

        public Task<int> Handle(CreateNewTodoListCommand cmd, CancellationToken cancellationToken)
        {
            var list = new TodoList() { Name = cmd.Name };
            _db.TodoLists.Add(list);
            _db.SaveChanges();
            _logger.LogInformation($"A new todo list with name {cmd.Name} was created");
            return Task.FromResult(list.Id);
        }

    }
}