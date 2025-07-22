using GradTest.Application.BoundedContexts.Orders.Queries;
using GradTest.Presentation.Common.Extensions;
using GradTest.Shared.Monads;
using MediatR;

namespace GradTest.Presentation.Endpoints.Orders;

public static class GetOrderByIdEndpoint
{
    private const string Name = "GetOrderById";

    public static IEndpointRouteBuilder MapGetOrderByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Orders.GetOrderById, async (Guid orderId, ISender sender, CancellationToken ct) =>
            {
                var query = new GetOrderByIdQuery(orderId);
                var result = await sender.Send(query, ct);

                return result.Match(
                    onSuccess: response =>
                    {
                        return TypedResults.Ok(response);
                    },
                    onError: error => ErrorResults.Map(error)
                );
            })
            .WithName(Name);

        return app;
    }
}
