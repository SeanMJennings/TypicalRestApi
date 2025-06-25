using Api.Hosting;

var configuration = Configuration.Build();
var builder = WebApplication.CreateBuilder(args);
await new CrudApi(builder, configuration).RunAsync();