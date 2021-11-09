using System;
using api.Modules.TodoListModule;
using api.Modules.TodoListModule.Features;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddJsonConsole();
builder.Services.AddCors(o => o.AddPolicy("corsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            }));

var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
if (connectionString is null)
{
    connectionString = "Host=localhost;Database=silly;Username=silly;Password=silly";
}
// add every modules db context
builder.Services.AddDbContext<TodoListContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddControllers().AddFluentValidation(o => { o.RegisterValidatorsFromAssemblyContaining<Program>(); });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));

var _mediator = app.Services.GetService<IMediator>();
// Lyft ut till en Routes.cs sen?
app.MapGet("/todolist", async () => await _mediator.Send(new GetAllTodoListsQuery()));
app.MapGet("/todolist/{id}", async (int id) => await _mediator.Send(new GetTodoListByIdQuery(id)));
app.MapDelete("/todolist/{id}", async (int id) => await _mediator.Send(new DeleteTodoListByIdCommand(id)));
app.MapPost("/todolist", async (CreateNewTodoListCommand command) => await _mediator.Send(command));
app.MapPost("/todolist/item", async (AddItemToListCommand command) => await _mediator.Send(command));
app.MapMethods("todolist/item", new string[] { "PATCH" }, async (CheckItemCommand command) => await _mediator.Send(command));
app.MapDelete("todolist/item/{id}", async (int id) => await _mediator.Send(new DeleteItemByIdCommand(id)));

app.UseHttpLogging();
// app.UseExceptionHandler();
app.UseCors("corsPolicy");
app.UseAuthorization();

// update modules db context
var todoListContext = app.Services.GetService<TodoListContext>();
todoListContext.Database.Migrate();


app.Run("http://*:8080");

