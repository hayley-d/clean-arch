using FluentValidation;
using GradTest.Application.BoundedContexts.Orders.Commands;
using GradTest.Application.BoundedContexts.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GradTest.Application.Common.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddApplicationDependencies(this WebApplicationBuilder builder, ILogger logger)
    {
        builder.AddMediatR();
        builder.AddFluentValidation();
    }
    
    private static void AddMediatR(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(IApplicationAssemblyMarker).Assembly;
        
        builder.Services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(applicationAssembly);
        });
        
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));
        
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

        builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
    }
    
    private static void AddFluentValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(
            assembly: typeof(IApplicationAssemblyMarker).Assembly, 
            lifetime: ServiceLifetime.Transient
        );
    }
}