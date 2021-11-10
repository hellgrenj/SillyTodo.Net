
using api.Modules.TodoListModule;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
    
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Logging.AddConsole();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddTransient<IValidationWrapper, ValidationWrapper>();
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
builder.Services.AddDbContext<TodoListContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddTransient<IRoutes<TodoListModuleRoutes>, TodoListModuleRoutes>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
app.UseCors("corsPolicy");
app.UseExceptionHandler(Exceptions.Handler);
// update modules db and register modules routes
var todoListContext = app.Services.GetService<TodoListContext>();
todoListContext.Database.Migrate();
var todoListModuleRoutes = app.Services.GetService<IRoutes<TodoListModuleRoutes>>();
todoListModuleRoutes.Register(app);
// start app
app.Logger.LogInformation("The application started");
app.Run("http://*:8080");


