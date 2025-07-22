using GradTest.Application.BoundedContexts.Products.Queries;
using GradTest.Presentation.Common.Extensions;
using GradTest.Shared.Monads;
using MediatR;

namespace GradTest.Presentation.Endpoints.Products;

public static class GetProductByIdEndpoint
{
    private const string Name = "GetProduct";

    public static IEndpointRouteBuilder MapGetProductByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Products.GetProductById, async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send(query, ct);

                return result.Match(
                    onSuccess: response => TypedResults.Ok(response),
                    onError: error => ErrorResults.Map(error)
                );
            })
            .WithName(Name);

        return app;
    }
}

