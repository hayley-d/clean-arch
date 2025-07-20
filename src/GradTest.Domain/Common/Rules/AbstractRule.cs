using GradTest.Shared.Monads;

namespace GradTest.Domain.Common.Rules;

public abstract class AbstractRule
{
    protected const bool RulePassed = true;
    protected const bool RuleFailed = false;
    
    public string Title { get; protected set; } = "Business Rule Failed";
    public string ErrorDetails { get; protected set; } = "Business Rule Failed";
    protected abstract bool Passed();
    
    public Result Match(
        Func<Result> onSuccess,
        Func<RuleError, Result> onFailure)
    {
        return Passed()
            ? onSuccess()
            : onFailure(RuleError.Create(this));
    }
}