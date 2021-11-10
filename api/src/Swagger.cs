using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

public class Swagger
{
    public static void Setup(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
        });
    }
    public static void Use(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
    }
}