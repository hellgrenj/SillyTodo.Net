using api.Modules.TodoListModule.Features;
using Microsoft.AspNetCore.Builder;
public interface IRoutes<T>
{
    void Register(WebApplication app);
}
public class TodoListModuleRoutes : IRoutes<TodoListModuleRoutes>
{
    private readonly IValidationWrapper _validationWrapper;
    public TodoListModuleRoutes(IMediator mediator, IValidationWrapper validationWrapper)
    {
        _validationWrapper = validationWrapper;
    }
    public void Register(WebApplication app)
    {
        app.MapGet("/todolist", async (IMediator _mediator) => await _mediator.Send(new GetAllTodoListsQuery()));
        app.MapGet("/todolist/{id}", async (IMediator _mediator, int id) => await _mediator.Send(new GetTodoListByIdQuery(id)));
        app.MapDelete("/todolist/{id}", async (IMediator _mediator,int id) => await _mediator.Send(new DeleteTodoListByIdCommand(id)));

        app.MapPost("/todolist", async (IValidator<CreateNewTodoListCommand> validator, CreateNewTodoListCommand cmd) =>
        await _validationWrapper.ValidateAndSend<CreateNewTodoListCommand>(validator, cmd));

        app.MapPost("/todolist/item", async (IValidator<AddItemToListCommand> validator, AddItemToListCommand cmd) =>
        await _validationWrapper.ValidateAndSend<AddItemToListCommand>(validator, cmd));

        app.MapMethods("todolist/item", new string[] { "PATCH" }, async (IMediator _mediator, CheckItemCommand command) => await _mediator.Send(command));
        app.MapDelete("todolist/item/{id}", async (IMediator _mediator, int id) => await _mediator.Send(new DeleteItemByIdCommand(id)));
    }
}