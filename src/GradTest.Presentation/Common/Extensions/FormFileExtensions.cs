using GradTest.Domain.BoundedContexts.Files.Entities;
using GradTest.Domain.BoundedContexts.Files.Enums;
using GradTest.Presentation.Common.Errors;
using GradTest.Shared.Monads;

namespace GradTest.Presentation.Common.Extensions;

public static class FormFileExtensions
{
    public static Result<Blob> TryCreateBlob(this IFormFile file)
    {
        if (ContentType.TryFromName(file.ContentType, true, out var parsedContentType))
        {
            var blob = Blob.Create(file.FileName, parsedContentType);
            
            var ms = new MemoryStream();
            file.CopyTo(ms);
            blob.InitializeContent(ms);
            
            return Result<Blob>.Success(blob);
        }

        return Result<Blob>.Error(ContentTypeError.Create());
    }
}