using GradTest.Shared.Errors;

namespace GradTest.Shared.Monads;

public static class ResultExtensions
{
    public static TResponse Match<TResponse, TResult>(
        this Result<TResult> result, 
        Func<TResult, TResponse> onSuccess, 
        Func<AbstractError, TResponse> onError)
    {
        return !result.IsError
            ? onSuccess(result.Value)
            : onError(result.ErrorValue);
    }
    
    public static TResponse Match<TResponse>(
        this Result result, 
        Func<TResponse> onSuccess, 
        Func<AbstractError, TResponse> onError)
    {
        return !result.IsError
            ? onSuccess()
            : onError(result.ErrorValue);
    }
}