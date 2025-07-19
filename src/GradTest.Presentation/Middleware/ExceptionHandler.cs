using System.ComponentModel.DataAnnotations;
using System.Data;
using GradTest.Presentation.Common.Constants;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GradTest.Presentation.Middleware;

public class ExceptionHandler: IExceptionHandler
{
    private readonly Dictionary<Type, ExceptionHandlerDelegate> _exceptionHandlers;

    public ExceptionHandler()
    {
        _exceptionHandlers = new Dictionary<Type, ExceptionHandlerDelegate>
        {
            { typeof(InvalidOperationException), HandleInvalidOperationException },
            { typeof(ValidationException), HandleValidationException },
            { typeof(BadHttpRequestException), HandleBadRequestException },
            { typeof(ArgumentException), HandleArgumentException },
            { typeof(ArgumentNullException), HandleArgumentNullException },
            { typeof(DBConcurrencyException), HandleConcurrencyException }
        };
    }

    private delegate ProblemDetails ExceptionHandlerDelegate(Exception exception, HttpContext context);
    
    private ProblemDetails HandleException(Exception exception, HttpContext context)
    {
        var type = exception.GetType();
        
        return _exceptionHandlers.TryGetValue(type, out var handler) 
            ? handler(exception, context) 
            : HandleUnhandledException(exception, context);
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var details = HandleException(exception, httpContext);
        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);
        return true;
    }

    private static ProblemDetails HandleInvalidOperationException(Exception exception, HttpContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid operation attempted",
            Type = StatusCodeLinks.BadRequest,
            Detail = exception.Message
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return details;
    }

    private static ProblemDetails HandleValidationException(Exception exception, HttpContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Data validation failed",
            Type = StatusCodeLinks.BadRequest,
            Detail = exception.Message
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return details;

    }

    private static ProblemDetails HandleBadRequestException(Exception exception, HttpContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Bad Request",
            Type = StatusCodeLinks.BadRequest,
            Detail = exception.Message
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return details;
    }
    
    private static ProblemDetails HandleArgumentNullException(Exception exception, HttpContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Null values not allowed",
            Type = StatusCodeLinks.BadRequest,
            Detail = exception.Message
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return details;
    }

    private static ProblemDetails HandleArgumentException(Exception exception, HttpContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid value provided",
            Type = StatusCodeLinks.BadRequest,
            Detail = exception.Message
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return details;
    }

    private static ProblemDetails HandleUnhandledException(Exception exception, HttpContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unknown error occured",
            Type = StatusCodeLinks.InternalServerError,
            Detail = exception.Message
        };

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return details;
    }

    private static ProblemDetails HandleConcurrencyException(Exception exception, HttpContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Version mismatch",
            Type = StatusCodeLinks.Conflict,
            Detail = "The entity you tried to update has been updated already, please refresh and try again"
        };

        context.Response.StatusCode = StatusCodes.Status409Conflict;
        
        return details;
    }
}