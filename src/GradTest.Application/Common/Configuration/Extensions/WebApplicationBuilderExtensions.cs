using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GradTest.Application.Common.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddApplicationDependencies(this WebApplicationBuilder builder, ILogger logger)
    {
        builder.AddMediatR();
        logger.LogInformation("Configured: {@ServiceName}", "MediatR");
        
        builder.AddFluentValidation();
        logger.LogInformation("Configured: {@ServiceName}", "FluentValidation");
    }
    
    private static void AddMediatR(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(IApplicationAssemblyMarker).Assembly;
        
        builder.Services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(applicationAssembly);
        });

    }
    
    private static void AddFluentValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(
            assembly: typeof(IApplicationAssemblyMarker).Assembly, 
            lifetime: ServiceLifetime.Transient
        );
    }
}