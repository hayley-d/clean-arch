using GradTest.Domain.Common.Rules;

namespace GradTest.Domain.Common.Errors;

public sealed class RuleError: AbstractError
{
    public override required string Title { get; init; }
    public override required string Detail { get; init; }

    private RuleError() { }
    
    public static RuleError Create(AbstractRule rule)
    {
        return new RuleError
        {
            Title = rule.Title,
            Detail = rule.Details
        };
    }
    
    public static RuleError Create(AbstractAsyncRule rule)
    {
        return new RuleError
        {
            Title = rule.Title,
            Detail = rule.Details
        };
    }
}