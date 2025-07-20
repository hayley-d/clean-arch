using GradTest.Shared.Errors;
using GradTest.Shared.Monads;

namespace GradTest.Domain.Common.Rules;

public static class RuleValidator
{
    public static Task<Result> ResultFrom(AbstractAsyncRule rule)
    {
        return rule.Match(
            onSuccess: Result.Success,
            onFailure: error => Result.Error(GenericError.Create(rule.Title, rule.ErrorDetails)));
    }
    
    public static async Task<Result> ResultFromAll(AbstractAsyncRule[] rules)
    {
        foreach (var rule in rules)
        {
            var result = await rule.Match(
                onSuccess: Result.Success,
                onFailure: _ => Result.Error(GenericError.Create(rule.Title, rule.ErrorDetails)));

            if (result.IsError)
            {
                return result;
            }
        }
        
        return Result.Success();
    }
    
    public static Result ResultFrom(AbstractRule rule)
    {
        return rule.Match(
            onSuccess: Result.Success,
            onFailure: _ => Result.Error(GenericError.Create(rule.Title, rule.ErrorDetails)));
    }
    
    public static Result ResultFromAll(AbstractRule[] rules)
    {
        foreach (var rule in rules)
        {
            var result = rule.Match(
                onSuccess: Result.Success,
                onFailure: _ => Result.Error(GenericError.Create(rule.Title, rule.ErrorDetails)));

            if (result.IsError)
            {
                return result;
            }
        }
        
        return Result.Success();
    }
}