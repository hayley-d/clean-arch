using GradTest.Presentation.Common.Configuration.Extensions;
using GradTest.Application.Common.Configuration.Extensions;
using GradTest.Domain.BoundedContexts.OrderItems.Repositories;
using GradTest.Domain.BoundedContexts.Orders.Repositories;
using GradTest.Domain.BoundedContexts.Products.Repositories;
using GradTest.Infrastructure.BoundedContexts.OrderItems.Repositories;
using GradTest.Infrastructure.BoundedContexts.Ordrs.Repositories;
using GradTest.Infrastructure.BoundedContexts.Products.Repositories;
using GradTest.Infrastructure.Common.Repository;
using GradTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

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
