using Api.Hosting;

var configuration = Configuration.Build();
var builder = WebApplication.CreateBuilder(args);
await new TypicalRestApi(builder, configuration).RunAsync();