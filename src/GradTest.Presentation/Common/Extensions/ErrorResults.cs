using Microsoft.AspNetCore.Http.HttpResults;
using GradTest.Application.Common.Errors;
using GradTest.Domain.Common.Rules;
using GradTest.Infrastructure.Common.Errors;
using GradTest.Presentation.Common.Constants;
using GradTest.Shared.Errors;

namespace GradTest.Presentation.Common.Extensions;

public static class ErrorResults
{
    public static IResult Map(AbstractError error) => error switch
    {
        GenericError e => BadRequest(e),
        RuleError e => BadRequest(e),
        ValidationError e => Validation(e),
        NotFoundError e => NotFound(e),
        DatabaseError e => InternalServerError(e),
        not null => BadRequest(error),
        _ => UnknownError()
    };
    

    private static ProblemHttpResult NotFound(NotFoundError error)
    {
        return TypedResults.Problem(
            error.ErrorDetail,
            null,
            StatusCodes.Status404NotFound,
            error.Title,
            StatusCodeLinks.NotFound
        );
    }

    private static ValidationProblem Validation(ValidationError error)
    {
        return TypedResults.ValidationProblem(
            error.Errors,
            error.ErrorDetail,
            null,
            error.Title,
            StatusCodeLinks.BadRequest
        );
    }
    
    private static ProblemHttpResult BadRequest(AbstractError error)
    {
        return TypedResults.Problem(
            error.ErrorDetail, 
            null,
            StatusCodes.Status400BadRequest, 
            error.Title,
            StatusCodeLinks.BadRequest
        );
    }

    private static ProblemHttpResult UnknownError()
    {
        return TypedResults.Problem(
            "An unknown error occurred", 
            null,
            StatusCodes.Status400BadRequest, 
            "Unknown error",
            StatusCodeLinks.BadRequest
        );
    }
    
    private static ProblemHttpResult InternalServerError(AbstractError error)
    {
        return TypedResults.Problem(
            error.ErrorDetail, 
            null,
            StatusCodes.Status500InternalServerError, 
            error.Title,
            StatusCodeLinks.InternalServerError
        );
    }
}

