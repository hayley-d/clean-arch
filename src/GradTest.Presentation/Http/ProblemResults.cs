using Microsoft.AspNetCore.Http.HttpResults;
using GradTest.Domain.Common.Errors;
using GradTest.Presentation.Common.Constants;

namespace GradTest.Presentation.Http;

public static class ProblemResults
{
    public static ProblemHttpResult Forbid()
    {
        return TypedResults.Problem(
            "You do not have the correct role to perform this action", 
            null,
            StatusCodes.Status403Forbidden,
            "Action not allowed",
            StatusCodeLinks.Forbidden
        );
    }

    public static ProblemHttpResult FileTooLarge()
    {
        return TypedResults.Problem(
            "File size must be less than 5MB", 
            null,
            StatusCodes.Status413RequestEntityTooLarge,
            "File too large",
            StatusCodeLinks.RequestEntityTooLarge
        );
    }

    public static ProblemHttpResult Error(RuleError ruleError)
    {
        return TypedResults.Problem(
            ruleError.Detail, 
            null,
            StatusCodes.Status400BadRequest,
            ruleError.Title,
            StatusCodeLinks.BadRequest
        );
    }
    
    public static ProblemHttpResult Conflict(RuleError ruleError)
    {
        return TypedResults.Problem(
            ruleError.Detail, 
            null,
            StatusCodes.Status409Conflict, 
            ruleError.Title,
            StatusCodeLinks.Conflict
        );
    }
    
    public static ProblemHttpResult NotFound(RuleError ruleError)
    {
        return TypedResults.Problem(
            ruleError.Detail, 
            null,
            StatusCodes.Status404NotFound,
            ruleError.Title,
            StatusCodeLinks.NotFound
        );
    }

    public static ProblemHttpResult TooManyRecords(RuleError ruleError)
    {
        return TypedResults.Problem(
            ruleError.Detail,
            null,
            StatusCodes.Status413RequestEntityTooLarge,
            "Too many records",
            StatusCodeLinks.RequestEntityTooLarge);
    }

    public static ProblemHttpResult UnsupportedMediaType(string supportedMediaType)
    {
        return TypedResults.Problem(
            $"Only {supportedMediaType} file can be uploaded",
            null,
            StatusCodes.Status415UnsupportedMediaType,
            "Unsupported media type",
            StatusCodeLinks.UnsupportedMediaType);
    }
}