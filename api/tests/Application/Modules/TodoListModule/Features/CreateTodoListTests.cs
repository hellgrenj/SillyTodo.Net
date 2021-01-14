



using System;
using System.Linq;
using System.Threading;
using api.Persistence;
using api.Application.Modules.TodoListModule.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.Modules.TodoListModule.Features.Tests
{
    public class TodoListTests
    {
        DbContextOptions<TodoListContext> InMemoryOptions()
        {
            return new DbContextOptionsBuilder<TodoListContext>().UseInMemoryDatabase(databaseName: "Test_Database").Options;
        }
        ILogger<CreateTodoListHandler> GetMockedLogger()
        {
            return Mock.Of<ILogger<CreateTodoListHandler>>();
        }
        [Fact]
        public async System.Threading.Tasks.Task CreateTodoList_creates_a_valid_todolistAsync()
        {
            using (var context = new TodoListContext(InMemoryOptions()))
            {
                var logger = GetMockedLogger();
                var handler = new CreateTodoListHandler(context, logger);
                var id = await handler.Handle(new CreateNewTodoListCommand("A new todo list"), default(CancellationToken));
                

                Assert.Equal(1, id);
                Assert.Equal("A new todo list", context.TodoLists.Where(t => t.Id == 1).SingleOrDefault().Name);
            }
        }
    }
}
