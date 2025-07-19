using MediatR;
using Microsoft.AspNetCore.Mvc;
using GradTest.Application.BoundedContexts.Users.Queries;
using GradTest.Domain.BoundedContexts.Users.Policies;
using GradTest.Domain.BoundedContexts.Users.ValueObjects;
using GradTest.Presentation.Common.Constants;
using GradTest.Presentation.Common.Extensions;
using GradTest.Shared.Monads;

namespace GradTest.Presentation.Endpoints.Users;

public static class GetUserEndpoint
{
    public const string Name = "GetUser";
    
    public static void MapGetUserEndpoint(this IEndpointRouteBuilder app)
    {
        app
            .MapGet(ApiEndpoints.Users.GetUserById,
                async (
                    UserId userId, 
                    ISender sender, 
                    CancellationToken cancellationToken) =>
                {
                    var query = new GetUserQuery(userId);
                    var result = await sender.Send(query, cancellationToken);

                    return result.Match(
                        onSuccess: TypedResults.Ok,
                        onError: ErrorResults.Map);
                })
            .WithName(Name)
            .WithTags(RouteGroups.Users)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.Admin);
    }
}