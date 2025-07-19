namespace GradTest.Presentation.Endpoints.Users;

public static class UsersEndpointsExtensions
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetUserEndpoint();
        app.MapGetMyUserEndpoint();
        app.MapCreateUserEndpoint();
    }
}