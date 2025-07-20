namespace GradTest.Presentation.Endpoints.Products;

public static class ProductEndpointExtensions
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapCreateProductEndpoint()
            .MapGetProductByIdEndpoint()
            .MapListProductsEndpoint();
    }
}