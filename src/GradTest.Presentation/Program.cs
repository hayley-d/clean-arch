using GradTest.Presentation.Common.Configuration.Extensions;
using GradTest.Application.Common.Configuration.Extensions;
using GradTest.Application.Common.Services;
using GradTest.Domain.BoundedContexts.ExchangeRates.Repositories;
using GradTest.Domain.BoundedContexts.OrderItems.Repositories;
using GradTest.Domain.BoundedContexts.Orders.Repositories;
using GradTest.Domain.BoundedContexts.Products.Repositories;
using GradTest.Infrastructure.BoundedContexts.ExchangeRates;
using GradTest.Infrastructure.BoundedContexts.OrderItems.Repositories;
using GradTest.Infrastructure.BoundedContexts.Ordrs.Repositories;
using GradTest.Infrastructure.BoundedContexts.Products.Repositories;
using GradTest.Infrastructure.Persistence;
using GradTest.Presentation.Common.Configuration;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ExchangeRateService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IExchangeRateRepository, ExchangeRatesRepository>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

var startupLogger = builder.CreateStartupLogger();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.AddPresentationDependencies(startupLogger);
builder.AddApplicationDependencies(startupLogger);
builder.SetupRucurringJobs();

var app = builder.Build();

app.UseHangfireDashboard();
app.UseHangfireServer();  

app.SetupJobs();

await app.ConfigureAsync(args, startupLogger);

await app.RunAsync();
