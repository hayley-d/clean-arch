using GradTest.Shared.Errors;

namespace GradTest.Infrastructure.Common.Errors;

public class DatabaseError: AbstractError
{
    public override required string Title { get; init; }
    public override required string ErrorDetail { get; init; }

    private DatabaseError() { }

    public static DatabaseError Create(string detail)
    {
        return new DatabaseError
        {
            Title = "Database Error",
            ErrorDetail = detail
        };
    }
}