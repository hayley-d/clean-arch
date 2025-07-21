using GradTest.Presentation.Common.Configuration.Extensions;
using GradTest.Application.Common.Configuration.Extensions;
using GradTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var startupLogger = builder.CreateStartupLogger();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.AddPresentationDependencies(startupLogger);
builder.AddApplicationDependencies(startupLogger);

var app = builder.Build();

await app.ConfigureAsync(args, startupLogger);

startupLogger.LogInformation("Startup Completed");

await app.RunAsync();
