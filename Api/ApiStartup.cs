using Api.Middleware;
using Application;
using Domain;
using Domain.Entities;
using Domain.Primitives;
using Persistence;

namespace Api;

public class ApiStartup(WebApplicationBuilder Builder)
{
    public WebApplication Setup()
    {
        Builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            Converters.AddConverters(options.SerializerSettings.Converters);
        });
        Builder.Services.AddEndpointsApiExplorer();
        Builder.Services.AddSwaggerGen();
        
        Builder.Services.AddDbContext<DatabaseContext<User>>();
        Builder.Services.AddScoped<IAmARepository<User>, InMemoryRepository<User>>();
        Builder.Services.AddScoped<IAmAUserService, UserService>();

        var app = Builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}