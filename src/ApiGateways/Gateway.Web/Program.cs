using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((hostingContext, configuration) =>
{
    configuration.AddJsonFile($"configuration.{hostingContext.HostingEnvironment.EnvironmentName}.json").AddEnvironmentVariables();
});

builder.Services.AddOcelot();

var app = builder.Build();

await app.UseOcelot();

app.Run();