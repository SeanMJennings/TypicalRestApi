using Api.Middleware;
using Application;
using Domain;
using Persistence;
using Repositories;

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
        Builder.Services.AddScoped<Db>(_ => new Db(Settings.Database.Connection));
        Builder.Services.AddScoped<UserRepository>();
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