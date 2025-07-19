using GradTest.Presentation.Endpoints.Users;

namespace GradTest.Presentation.Endpoints;

public static class EndpointsExtensions
{
    public static void MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapUsersEndpoints();
    }
}