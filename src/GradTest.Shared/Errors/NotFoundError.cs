namespace GradTest.Shared.Errors;

public class NotFoundError: AbstractError
{
    public override required string Title { get; init; }
    public override required string ErrorDetail { get; init; }

    private NotFoundError() { }

    public static NotFoundError Create(string entity, string key)
    {
        return new NotFoundError
        {
            Title = "Entity Not Found",
            ErrorDetail = $"Could not find {entity} with Id: {key}" 
        };
    }
}