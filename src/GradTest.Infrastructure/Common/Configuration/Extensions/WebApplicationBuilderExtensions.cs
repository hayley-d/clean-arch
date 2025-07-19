using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GradTest.Application;
using GradTest.Application.Common.Contracts;
using GradTest.Application.Common.Services;
using GradTest.Domain.Common.Contracts;
using GradTest.Infrastructure.Common.Configuration.BlobStorage;
using GradTest.Infrastructure.Common.Services;
using GradTest.Infrastructure.Persistence;

namespace GradTest.Infrastructure.Common.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddInfrastructureDependencies(this WebApplicationBuilder builder, ILogger logger)
    {
        builder.AddDatabase();
        logger.LogInformation("Configured: {@ServiceName}", "Database");
        
        builder.AddRepositories();
        logger.LogInformation("Configured: {@ServiceName}", "Repositories");

        builder.AddUnitOfWork();
        logger.LogInformation("Configured: {@ServiceName}", "Unit of Work");

        builder.AddEventPublisher();
        logger.LogInformation("Configured: {@ServiceName}", "Event Publisher");

        builder.AddAzureBlobStorage();
        logger.LogInformation("Configured: {@ServiceName}", "Azure Blob Storage");
    }

    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Database");
        ArgumentNullException.ThrowIfNull(connectionString);
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseNpgsql(connectionString,
                optionsBuilder =>
                {
                    optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    optionsBuilder.MigrationsAssembly(typeof(IInfrastructureAssemblyMarker).Assembly.FullName);
                });
        });
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    private static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.Scan(sources => sources
            .FromAssemblyOf<IInfrastructureAssemblyMarker>()
            .AddClasses(repositories => repositories.AssignableTo<IRepository>())
            .AsMatchingInterface()
            .WithScopedLifetime());
    }
    
    private static void AddUnitOfWork(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddEventPublisher(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventPublisher, EventPublisher>();
        
        builder.Services.AddMassTransit(options =>
        {
            options.AddConsumers(typeof(IApplicationAssemblyMarker).Assembly);
            options.SetKebabCaseEndpointNameFormatter();

            options.AddEntityFrameworkOutbox<ApplicationDbContext>(config =>
            {
                config.QueryDelay = TimeSpan.FromSeconds(30);
                config.UsePostgres().UseBusOutbox();
            });
            
            options.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
                config.UseMessageRetry(x =>
                {
                    x.Exponential(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(5));
                });
            });
        });
    }

    private static void AddAzureBlobStorage(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureOptions<AzureBlobStorageOptionsSetup>();
        
        builder.Services.AddTransient<IBlobContainerInitializer, BlobContainerInitializer>();
        builder.Services.AddScoped<IBlobServiceClientWrapper, BlobServiceClientWrapper>();
        builder.Services.AddScoped<IBlobStorage, AzureBlobStorage>();
    }
}