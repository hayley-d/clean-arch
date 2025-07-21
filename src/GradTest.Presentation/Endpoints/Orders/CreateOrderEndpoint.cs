using GradTest.Application.BoundedContexts.Orders.Commands;
using GradTest.Contracts.Orders.Requests;
using GradTest.Presentation.Common.Extensions;
using GradTest.Shared.Errors;
using GradTest.Shared.Monads;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GradTest.Presentation.Endpoints.Orders;

public static class CreateOrderEndpoint
{
    public const string Name = "CreateOrder";

    public static IEndpointRouteBuilder MapCreateOrderEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Orders.CreateOrder, async (HttpContext httpContext,[FromBody] CreateOrderRequest request, ISender sender, CancellationToken ct) =>
            {
                var userId = httpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

                if (userId is null)
                {
                    return ErrorResults.Map(GenericError.Create("Missing user ID", "Missing user ID in request"));
                }
                
                var command = new CreateOrderCommand(Guid.Parse(userId), request.Items);
                var result = await sender.Send(command, ct);
            
                return result.Match(
                    onSuccess: response => TypedResults.Created($"/api/orders", response.OrderId), 
                    onError: error => ErrorResults.Map(error)
                );
            })
            .WithName(Name);
        
        return app;
    }
}