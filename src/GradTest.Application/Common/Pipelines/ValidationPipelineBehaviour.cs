using FluentValidation;
using GradTest.Application.Common.Errors;
using MediatR;
using GradTest.Shared.Tracing;

namespace GradTest.Application.Common.Pipelines;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TResponse : IResult
    where TRequest : IRequest<TResponse> 
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        using var activity = ActivityProvider.Validation();
        
        foreach (var validator in _validators)
        {
            activity?.SetTag("validator", validator.GetType().ToString());
        }
        
        var validationTasks = _validators.Select(v => v.ValidateAsync(context, cancellationToken));
        var validationResults = await Task.WhenAll(validationTasks);

        activity?.Stop();
        
        if (validationResults.ToList().TrueForAll(x => x.IsValid))
        {
            return await next();
        }

        var responseType = typeof(TResponse);
        var genericTypeArguments = responseType.GenericTypeArguments;
        
        if (genericTypeArguments.Length > 0)
        {
            var resultType = genericTypeArguments[0];
            var resultGenericType = typeof(Result<>).MakeGenericType(resultType);
            var errorMethod = resultGenericType.GetMethod(nameof(Result.Error), [typeof(ValidationError)]);
            var errorResult = errorMethod!.Invoke(null, [ValidationError.Create(validationResults)]);
            
            return (TResponse) errorResult!;
        }
        
        return (TResponse)(object) Result.Error(ValidationError.Create(validationResults));
    }
}