using FluentValidation.Results;

namespace GradTest.Application.Common.Errors;

public class ValidationError: AbstractError
{
    public override required string Title { get; init; }
    public override required string ErrorDetail { get; init; }
    public required IDictionary<string, string[]> Errors { get; init; }

    private ValidationError() { }

    public static ValidationError Create(ValidationResult[] validationResults)
    {
        return new ValidationError
        {
            Title = "Validation failed",
            ErrorDetail = "One or more validation errors occurred",
            Errors = AggregateValidationErrors(validationResults)
        };
    }

    private static Dictionary<string, string[]> AggregateValidationErrors(ValidationResult[] validationResults)
    {
        var errors = new Dictionary<string, string[]>();

        foreach (var validationResult in validationResults)
        {
            var resultDictionary = validationResult.ToDictionary();
            
            foreach (var kvp in resultDictionary)
            {
                if (errors.TryGetValue(kvp.Key, out var array))
                {
                    array.ToList().AddRange(kvp.Value);
                }
                else
                {
                    errors[kvp.Key] = kvp.Value;
                }
            }
        }

        return errors;
    }
}