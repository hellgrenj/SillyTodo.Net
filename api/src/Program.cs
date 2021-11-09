using System;
using System.Net;
using api.Modules.TodoListModule;
using api.Modules.TodoListModule.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Logging.AddConsole();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSingleton<IValidationWrapper, ValidationWrapper>();
builder.Services.AddCors(o => o.AddPolicy("corsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            }));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
});
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING") ?? "Host=localhost;Database=silly;Username=silly;Password=silly";
builder.Services.AddDbContext<TodoListContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Transient, ServiceLifetime.Transient);
builder.Services.AddTransient<ITodoListModuleRoutes, TodoListModuleRoutes>();



var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
app.UseCors("corsPolicy");

app.UseExceptionHandler(errorApp =>
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
            });

// update modules db context and register modules routes
var todoListContext = app.Services.GetService<TodoListContext>();
todoListContext.Database.Migrate();

var todoListModuleRoutes = app.Services.GetService<ITodoListModuleRoutes>();
todoListModuleRoutes.Register(app);

app.Logger.LogInformation("The application started");
app.Run("http://*:8080");


