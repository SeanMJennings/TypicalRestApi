using System.Text.Json.Serialization;
using Application;
using Domain;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using WebHost;

namespace Api.Hosting;

internal sealed class TypicalRestApi(WebApplicationBuilder webApplicationBuilder, IConfiguration configuration) : WebApi(webApplicationBuilder, configuration)
{
    private readonly Settings _settings = new(configuration);
    protected override string ApplicationName => nameof(TypicalRestApi);
    protected override string TelemetryConnectionString => _settings.ApplicationInsights.ConnectionString;
    protected override List<JsonConverter> JsonConverters => Converters.GetConverters;

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddScoped<Db>(_ => new Db(_settings.Database.Connection));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAmAUserService, UserService>();
    }
}