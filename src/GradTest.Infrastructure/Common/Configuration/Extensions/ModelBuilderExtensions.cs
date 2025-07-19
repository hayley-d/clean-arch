using Microsoft.EntityFrameworkCore;
using GradTest.Domain.Common.Entities;
using GradTest.Infrastructure.BoundedContexts.Users.ValueConverters;

namespace GradTest.Infrastructure.Common.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigureISoftDeletable(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes();

        foreach (var entityType in entities)
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .HasIndex("IsDeleted")
                    .HasFilter("\"IsDeleted\" = FALSE");
                
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(nameof(ISoftDeletable.DeletedBy))
                    .HasMaxLength(50);

                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(nameof(ISoftDeletable.DeletedBy))
                    .HasConversion<UserIdValueConverter>();
            }
        }
    }
    
    public static void ConfigureIAuditable(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes();

        foreach (var entityType in entities)
        {
            if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(nameof(IAuditable.CreatedBy))
                    .HasMaxLength(50);
                
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(nameof(IAuditable.CreatedBy))
                    .HasConversion<UserIdValueConverter>();
                
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(nameof(IAuditable.ModifiedBy))
                    .HasMaxLength(50);
                
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(nameof(IAuditable.ModifiedBy))
                    .HasConversion<UserIdValueConverter>();
            }
        }
    }

    public static void ApplyHashIndexToEntityIdField(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes();

        foreach (var entityType in entities)
        {
            if (typeof(EntityBase).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .HasIndex("Id")
                    .HasAnnotation("Npgsql:IndexMethod", "hash");
            }
        }
    }
}