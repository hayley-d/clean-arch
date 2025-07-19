using Microsoft.AspNetCore.Mvc;
using GradTest.Application.Common.Services;
using GradTest.Domain.BoundedContexts.Users.Entities;
using GradTest.Domain.BoundedContexts.Users.Policies;
using GradTest.Presentation.Common.Constants;
using GradTest.Presentation.Common.Extensions;
using GradTest.Shared.Errors;
using GradTest.Shared.Monads;

namespace GradTest.Presentation.Endpoints.Users;

public static class GetMyUserEndpoint
{
    public const string Name = "GetMyUser";
    
    public static void MapGetMyUserEndpoint(this IEndpointRouteBuilder app)
    {
        app
            .MapGet(ApiEndpoints.Users.GetMyUser, 
                (ICurrentUserService currentUserService) =>
                {
                    var userId = currentUserService.GetUserIdFromToken();
                    var result = Result.Error(NotFoundError.Create(nameof(User), userId.ToString()));

                    return result.Match(
                        onSuccess: TypedResults.Ok,
                        onError: ErrorResults.Map);
                })
            .WithName(Name)
            .WithTags(RouteGroups.Users)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}