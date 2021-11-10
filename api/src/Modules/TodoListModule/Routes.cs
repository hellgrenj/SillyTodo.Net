using api.Modules.TodoListModule.Features;
namespace api.Modules.TodoListModule;
public class TodoListModuleRoutes : IRoutes<TodoListModuleRoutes>
{
    private readonly IMediator _mediator;
    private readonly IValidationWrapper _validationWrapper;
    public TodoListModuleRoutes(IMediator mediator, IValidationWrapper validationWrapper)
    {
        _mediator = mediator;
        _validationWrapper = validationWrapper;
    }
    public void Register(WebApplication app)
    {
        app.MapGet("/todolist", async () => await _mediator.Send(new GetAllTodoListsQuery()));
        app.MapGet("/todolist/{id}", async (int id) => await _mediator.Send(new GetTodoListByIdQuery(id)));
        app.MapDelete("/todolist/{id}", async (int id) => await _mediator.Send(new DeleteTodoListByIdCommand(id)));

        app.MapPost("/todolist", async (IValidator<CreateNewTodoListCommand> validator, CreateNewTodoListCommand cmd) =>
        await _validationWrapper.ValidateAndSend<CreateNewTodoListCommand>(validator, cmd));

        app.MapPost("/todolist/item", async (IValidator<AddItemToListCommand> validator, AddItemToListCommand cmd) =>
        await _validationWrapper.ValidateAndSend<AddItemToListCommand>(validator, cmd));

        app.MapMethods("todolist/item", new string[] { "PATCH" }, async (CheckItemCommand command) => await _mediator.Send(command));
        app.MapDelete("todolist/item/{id}", async (int id) => await _mediator.Send(new DeleteItemByIdCommand(id)));
    }

}