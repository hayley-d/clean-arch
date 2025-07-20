using GradTest.Shared.Errors;

namespace GradTest.Domain.Common.Rules;


public sealed class RuleError : AbstractError
{
    public override required string Title { get; init; }
    public override required string ErrorDetail { get; init; }

    private RuleError() { }
    
    public static RuleError Create(AbstractRule rule)
    {
        return new RuleError
        {
            Title = rule.Title,
            ErrorDetail = rule.ErrorDetails
        };
    }
    
    public static RuleError Create(AbstractAsyncRule rule)
    {
        return new RuleError
        {
            Title = rule.Title,
            ErrorDetail = rule.ErrorDetails
        };
    }
}