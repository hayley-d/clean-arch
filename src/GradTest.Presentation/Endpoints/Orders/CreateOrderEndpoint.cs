using GradTest.Application.BoundedContexts.Orders.Commands;
using GradTest.Contracts.Orders.Requests;
using GradTest.Presentation.Common.Extensions;
using GradTest.Shared.Monads;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GradTest.Presentation.Endpoints.Orders;

public static class CreateOrderEndpoint
{
    public const string Name = "CreateOrder";

    public static IEndpointRouteBuilder MapCreateOrderEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Products.CreateProduct, async ([FromBody] CreateOrderRequest request, ISender sender, CancellationToken ct) =>
            {
                var userId = new Guid();
                var command = new CreateOrderCommand(userId, request.Items);
                var result = await sender.Send(command, ct);
            
                return result.Match(
                    onSuccess: response => TypedResults.CreatedAtRoute(response, routeName: "GetOrder"), 
                    onError: error => ErrorResults.Map(error)
                );
            })
            .WithName(Name);
        
        return app;
    }
}