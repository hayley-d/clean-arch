using GradTest.Shared.Errors;

namespace GradTest.Presentation.Common.Errors;

public class ContentTypeError: AbstractError
{
    public override required string Title { get; init; }
    public override required string Detail { get; init; }

    private ContentTypeError() { }

    public static ContentTypeError Create()
    {
        return new ContentTypeError
        {
            Title = "Content type not supported",
            Detail = "The file extension is not supported by the application"
        };
    }
}