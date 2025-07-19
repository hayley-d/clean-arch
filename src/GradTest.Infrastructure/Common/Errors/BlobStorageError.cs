namespace GradTest.Infrastructure.Common.Errors;

public class BlobStorageError: AbstractError
{
    public override required string Title { get; init; }
    public override required string Detail { get; init; }

    private BlobStorageError() { }

    public static BlobStorageError Create()
    {
        return new BlobStorageError
        {
            Title = "File storage error",
            Detail = "An error occured with file storage"
        };
    }
}