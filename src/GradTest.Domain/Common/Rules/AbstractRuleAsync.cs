using GradTest.Shared.Monads;

namespace GradTest.Domain.Common.Rules;

public abstract class AbstractAsyncRule
{
    protected const bool RulePassed = true;
    protected const bool RuleFailed = false;
    
    public string Title { get; protected set; } = "Business Rule Failed";
    public string ErrorDetails { get; protected set; } = "Business Rule Failed";
    protected abstract Task<bool> Passed();
    
    public async Task<Result> Match(
        Func<Result> onSuccess,
        Func<RuleError, Result> onFailure)
    {
        return await Passed()
            ? onSuccess()
            : onFailure(RuleError.Create(this));
    }
}