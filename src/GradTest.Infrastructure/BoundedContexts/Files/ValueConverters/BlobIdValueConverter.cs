using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using GradTest.Domain.BoundedContexts.Files.ValueObjects;

namespace GradTest.Infrastructure.BoundedContexts.Files.ValueConverters;

public class BlobIdValueConverter() : ValueConverter<BlobId, Guid>(
    blobId => blobId.Value,
    guid => BlobId.FromGuid(guid)
);