using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using GradTest.Domain.BoundedContexts.Files.Enums;

namespace GradTest.Infrastructure.BoundedContexts.Files.ValueConverters;

public class ContentTypeValueConverter() : ValueConverter<ContentType, string>(
    contentType => contentType.Name,
    name => ContentType.FromName(name, true)
);