namespace GradTest.Shared.Errors;

public class NotFoundError: AbstractError
{
    public override required string Title { get; init; }
    public override required string Detail { get; init; }

    private NotFoundError() { }

    public static NotFoundError Create(string entity, string key)
    {
        return new NotFoundError
        {
            Title = "Entity Not Found",
            Detail = $"Could not find {entity} with Id: {key}" 
        };
    }
}