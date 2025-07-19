using System.ComponentModel.DataAnnotations.Schema;
using GradTest.Domain.BoundedContexts.Files.Enums;
using GradTest.Domain.BoundedContexts.Files.ValueObjects;
using GradTest.Domain.Common.Entities;

namespace GradTest.Domain.BoundedContexts.Files.Entities;

public class Blob: EntityBase
{
    public BlobId Id { get; private set; }
    public string FileName { get; private set; }
    public ContentType ContentType { get; private set; }
    [NotMapped] public byte[] Content { get; private set; } = [];
    [NotMapped] public string StorageName => $"{Id}{ContentType.Extension()}";
    
    private Blob() { }

    public static Blob Create(string fileName, ContentType contentType)
    {
        var extension = contentType.Extension();
        var cleanedFileName = FormatFileName(fileName, extension);

        return new Blob
        {
            FileName = cleanedFileName,
            ContentType = contentType
        };
    }

    private static string FormatFileName(string fileName, string extension)
    {
        var cleanedString = fileName.Trim();
        var extensionLength = extension.Length + 1;
        var extensionFromFileName = cleanedString[^1..extensionLength];
        var hasCorrectExtension = extensionFromFileName.Equals(extension);
        
        return hasCorrectExtension
            ? cleanedString
            : cleanedString + extension;
    }

    public void InitializeContent(MemoryStream memoryStream)
    {
        memoryStream.Position = 0;
        Content = memoryStream.ToArray();
    }
}