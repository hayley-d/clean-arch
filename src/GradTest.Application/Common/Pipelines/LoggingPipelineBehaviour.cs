using GradTest.Application.Common.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using GradTest.Shared.Tracing;

namespace GradTest.Application.Common.Pipelines;

public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : IResult
    where TRequest : IRequest<TResponse> 
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserIdFromToken();
        var roles = _currentUserService.GetRolesFromToken();
        
        using var activity = ActivityProvider.RequestHandler();
        activity?.SetTag("Request", typeof(TRequest).FullName);
            
        using (_logger.BeginScope("UserId: {UserId}, Roles: {@Roles}" , userId, roles))
        {
            _logger.LogInformation(
                "Starting handler for: {@RequestName}",
                typeof(TRequest)
            );
            
            var handlerResult = await next();

            if (handlerResult.IsError)
            {
                _logger.LogError("Error: {@RequestName}, Request: {@RequestParameters}", typeof(TRequest), request);
                return handlerResult;
            }

            _logger.LogInformation("Handler completed for: {@RequestName}", typeof(TRequest));
            return handlerResult;
        }
    }
}