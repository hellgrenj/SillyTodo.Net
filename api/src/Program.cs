using api.Modules.TodoListModule;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Logging.AddConsole();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddTransient<IValidationWrapper, ValidationWrapper>();
Cors.Setup(builder);
Swagger.Setup(builder);
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING") ?? "Host=localhost;Database=silly;Username=silly;Password=silly";
builder.Services.AddDbContext<TodoListContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddTransient<IRoutes<TodoListModuleRoutes>, TodoListModuleRoutes>();

var app = builder.Build();
Swagger.Use(app);
Cors.Use(app);
app.UseExceptionHandler(Exceptions.Handler);
// update modules db and register modules routes
var todoListContext = app.Services.GetService<TodoListContext>();
todoListContext.Database.Migrate();
var todoListModuleRoutes = app.Services.GetService<IRoutes<TodoListModuleRoutes>>();
todoListModuleRoutes.Register(app);
// start app
app.Logger.LogInformation("The application started");
app.Run("http://*:8080");


