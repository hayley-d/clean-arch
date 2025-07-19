using GradTest.Presentation.Common.Configuration.Extensions;
using GradTest.Application.Common.Configuration.Extensions;
using GradTest.Infrastructure.Common.Configuration.Extensions;

var builder = WebApplication.CreateBuilder(args);

var startupLogger = builder.CreateStartupLogger();

builder.AddPresentationDependencies(startupLogger);
builder.AddApplicationDependencies(startupLogger);
builder.AddInfrastructureDependencies(startupLogger);

var app = builder.Build();

await app.ConfigureAsync(args, startupLogger);

startupLogger.LogInformation("Startup Completed");

await app.RunAsync();
