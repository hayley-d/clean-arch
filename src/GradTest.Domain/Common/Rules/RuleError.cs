namespace GradTest.Domain.Common.Rules;


public sealed class RuleError
{
    public required string Title { get; init; }
    public required string ErrorDetail { get; init; }

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