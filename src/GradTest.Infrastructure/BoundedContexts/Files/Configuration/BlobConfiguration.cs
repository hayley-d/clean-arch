using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GradTest.Domain.BoundedContexts.Files.Entities;
using GradTest.Domain.BoundedContexts.Files.Enums;
using GradTest.Domain.BoundedContexts.Files.ValueObjects;
using GradTest.Infrastructure.BoundedContexts.Files.ValueConverters;

namespace GradTest.Infrastructure.BoundedContexts.Files.Configuration;

public class BlobConfiguration: IEntityTypeConfiguration<Blob>
{
    public void Configure(EntityTypeBuilder<Blob> builder)
    {
        builder.ToTable(nameof(Blob).ToLower(), nameof(Files).ToLower());

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion<BlobIdValueConverter>();

        builder
            .Property(x => x.ContentType)
            .HasConversion<ContentTypeValueConverter>();
    }
}