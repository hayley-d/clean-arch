using Ardalis.SmartEnum;
using GradTest.Application.BoundedContexts.Products.Commands;
using GradTest.Contracts.Products.Requests;
using GradTest.Domain.BoundedContexts.Products.Enums;
using GradTest.Infrastructure.BoundedContexts.Products.ValueConvertrs;
using GradTest.Presentation.Common.Extensions;
using GradTest.Shared.Monads;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GradTest.Presentation.Endpoints.Products;

public static class CreateProductEndpoint
{
    public const string Name = "CreateProduct";

    public static IEndpointRouteBuilder MapCreateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Products.CreateProduct, async ([FromBody] CreateProductRequest request, ISender sender, CancellationToken ct) =>
        {
            Category category;

            try
            {
                category = Category.FromName(request.CategoryName, ignoreCase: true);
            }
            catch (SmartEnumNotFoundException)
            {
                return TypedResults.BadRequest($"Invalid category: {request.CategoryName}");
            }
            
            var command = new CreateProductCommand(request.Name, request.Description, request.Price, request.Quantity, category);
            var result = await sender.Send(command, ct);
            
            return result.Match(
                onSuccess: response => TypedResults.CreatedAtRoute(response, routeName: "GetProduct"), 
                onError: error => ErrorResults.Map(error)
            );
        })
            .WithName(Name);
        
        return app;
    }
}