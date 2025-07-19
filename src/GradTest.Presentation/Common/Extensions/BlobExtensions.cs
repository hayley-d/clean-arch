using Microsoft.AspNetCore.Http.HttpResults;
using GradTest.Domain.BoundedContexts.Files.Entities;

namespace GradTest.Presentation.Common.Extensions;

public static class BlobExtensions
{
    public static FileContentHttpResult ToFileResult(this Blob blob)
    {
        return TypedResults.File(
            fileContents: blob.Content, 
            contentType: blob.ContentType.Name, 
            fileDownloadName: blob.FileName);
    }
}