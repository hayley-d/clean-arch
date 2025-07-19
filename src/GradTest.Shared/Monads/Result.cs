using GradTest.Shared.Errors;

namespace GradTest.Shared.Monads;

public interface IResult
{
    bool IsError { get; }
}

public sealed class Result<T> : IResult
{
    public T Value { get; private init; }
    public bool IsError { get; private init; }
    public AbstractError ErrorValue { get; private init; } = GenericError.None;

    private Result() { }
    
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(AbstractError error) => Error(error);
    public static implicit operator T(Result<T> result) => result.Value;
    public static implicit operator Result<T>(Result result) => Error(result.ErrorValue);

    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            IsError = false,
            Value = value
        };
    }

    public static Result<T> Error(AbstractError error)
    {
        return new Result<T>
        {
            IsError = true,
            ErrorValue = error
        };
    }
}

public sealed class Result: IResult
{
    public bool IsError { get; private init; }
    public AbstractError ErrorValue { get; private init; } = GenericError.None;

    private Result() { }

    public static implicit operator Result(AbstractError error) => Error(error);

    public static Result Success()
    {
        return new Result
        {
            IsError = false
        };
    }

    public static Result Error(AbstractError error)
    {
        return new Result
        {
            ErrorValue = error,
            IsError = true
        };
    }
}