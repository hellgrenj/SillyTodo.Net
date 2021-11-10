using api.Modules.TodoListModule.Exceptions;
public class ExceptionHandler
{
    public static void Handle(IApplicationBuilder errorApp)
    {
        errorApp.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature?.Error is EntityNotFoundException)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                    await context.Response.WriteAsJsonAsync(new { contextFeature?.Error.Message });
                    await context.Response.CompleteAsync();
                });
    }
}
