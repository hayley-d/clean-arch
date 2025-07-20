using GradTest.Domain.Common.Rules;

namespace GradTest.Domain.Common.Entities;

public class Result<T>
{
    public bool IsSuccess { get; }
    public RuleError Error { get; }
    public T? Value { get; }
    
    public bool IsError() {
       return !IsSuccess;
    }      

    private Result(bool isSuccess, T? value, RuleError error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    public static Result<T> Success(T value) => new(true, value, null);
    
    public static Result<T> Failure(RuleError error) => new(false, default, error);
}
