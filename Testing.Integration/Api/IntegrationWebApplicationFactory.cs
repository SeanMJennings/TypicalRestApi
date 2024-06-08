using Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Api;

public class IntegrationWebApplicationFactory<TProgram>(IAmAUserService UserService)
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            builder.ConfigureTestServices(c => c.AddScoped<IAmAUserService>(_ => UserService));
        });

        builder.UseEnvironment("Development");
    }
}