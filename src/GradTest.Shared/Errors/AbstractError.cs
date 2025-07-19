namespace GradTest.Shared.Errors;

public abstract class AbstractError
{
    public abstract required string Title { get; init; }
    public abstract required string Detail { get; init; }
}