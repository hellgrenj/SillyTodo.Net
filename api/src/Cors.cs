using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;

public class Cors
{
    public static void Setup(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(o => o.AddPolicy("corsPolicy", builder =>
             {
                 builder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader();
             }));
    }
    public static void Use(WebApplication app)
    {
        app.UseCors("corsPolicy");
    }
}