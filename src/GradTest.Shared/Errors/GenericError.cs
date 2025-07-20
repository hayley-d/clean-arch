namespace GradTest.Shared.Errors;

public sealed class GenericError: AbstractError
{
    public override required string Title { get; init; }
    public override required string ErrorDetail { get; init; }
    
    public static readonly GenericError None = new GenericError
    {
        Title = string.Empty,
        ErrorDetail = string.Empty
    };

    private GenericError() { }
    
    public static GenericError Create(string title, string details)
    {
        return new GenericError
        {
            Title = title,
            ErrorDetail = details
        };
    }
}