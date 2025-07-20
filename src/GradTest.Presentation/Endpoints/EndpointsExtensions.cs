using GradTest.Presentation.Endpoints.Orders;
using GradTest.Presentation.Endpoints.Products;

namespace GradTest.Presentation.Endpoints;

public static class EndpointsExtensions
{
    public static void MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapProductsEndpoints();
        app.MapOrdersEndpoints();
    }
}