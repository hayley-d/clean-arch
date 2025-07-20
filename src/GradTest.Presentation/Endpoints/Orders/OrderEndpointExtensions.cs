namespace GradTest.Presentation.Endpoints.Orders;

public static class OrderEndpointExtensions
{
        public static void MapOrdersEndpoints(this IEndpointRouteBuilder app)
        {
            app
                .MapCreateOrderEndpoint()
                .MapGetOrderByIdEndpoint();
        }
}