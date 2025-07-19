using MediatR;
using Microsoft.AspNetCore.Mvc;
using GradTest.Application.BoundedContexts.Users.Commands;
using GradTest.Contracts.Users.Requests;
using GradTest.Domain.BoundedContexts.Users.Policies;
using GradTest.Presentation.Common.Constants;
using GradTest.Presentation.Common.Extensions;
using GradTest.Shared.Monads;

namespace GradTest.Presentation.Endpoints.Users;

public static class CreateUserEndpoint
{
    public const string Name = "CreateUser";
    
    public static void MapCreateUserEndpoint(this IEndpointRouteBuilder app)
    {
        app
            .MapPost(ApiEndpoints.Users.CreateUser,
                async (
                    [FromBody] CreateUserRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var command = new CreateUserCommand(request.FirstName, request.LastName);
                    var result = await sender.Send(command, cancellationToken);

                    return result.Match(
                        onSuccess: response => TypedResults.CreatedAtRoute(response, GetUserEndpoint.Name, new
                        {
                            userId = response.Id
                        }),
                        onError: ErrorResults.Map);
                })
            .WithName(Name)
            .WithTags(RouteGroups.Users);
        //.RequireAuthorization(Policies.Admin);
    }
}