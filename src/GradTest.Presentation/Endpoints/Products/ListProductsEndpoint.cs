using GradTest.Application.BoundedContexts.Products.Queries;
using GradTest.Presentation.Common.Extensions;
using GradTest.Shared.Monads;
using MediatR;

namespace GradTest.Presentation.Endpoints.Products;

public static class ListProductsEndpoint
{
    public const string Name = "ListProducts";

    public static IEndpointRouteBuilder MapListProductsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Products.ListProducts, async (ISender sender, CancellationToken ct) =>
            {
                var query = new ListProdutsQuery();
                var result = await sender.Send(query, ct);

                return result.Match(
                    onSuccess: response => TypedResults.Ok(response),
                    onError: error => ErrorResults.Map(error)
                );
            })
            .WithName(Name);

        return app;
    }}