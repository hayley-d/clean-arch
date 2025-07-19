using MassTransit;
using Microsoft.EntityFrameworkCore;
using GradTest.Application.Common.Services;
using GradTest.Domain.BoundedContexts.Files.Entities;
using GradTest.Domain.BoundedContexts.Users.Entities;
using GradTest.Domain.Common.Entities;
using GradTest.Infrastructure.Common.Errors;
using GradTest.Infrastructure.Common.Configuration.Extensions;

namespace GradTest.Infrastructure.Persistence;

public class ApplicationDbContext: DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Blob> Blobs { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.AddInboxStateEntity(builder => builder.ToTable("inboxState", "massTransit"));
        modelBuilder.AddOutboxStateEntity(builder => builder.ToTable("outboxState", "massTransit"));
        modelBuilder.AddOutboxMessageEntity(builder => builder.ToTable("outboxMessage", "massTransit"));
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IInfrastructureAssemblyMarker).Assembly);
        modelBuilder.ConfigureISoftDeletable();
        modelBuilder.ConfigureIAuditable();
        modelBuilder.ApplyHashIndexToEntityIdField();
    }

    private void UpdateAuditableState()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreatedBy(_currentUserService.GetUserIdFromToken());
                    break;
                case EntityState.Modified:
                    entry.Entity.SetModifiedBy(_currentUserService.GetUserIdFromToken());
                    break;
            }
        }
    }

    private void UpdateSoftDeletableState()
    {
        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State is EntityState.Deleted)
            {
                entry.Entity.Delete();
                entry.State = EntityState.Modified;
                entry.Entity.SetDeletedBy(_currentUserService.GetUserIdFromToken());
            }
        }
    }

    public new async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            UpdateAuditableState();
            UpdateSoftDeletableState();
            
            var count = await base.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(count);
        }
        catch (Exception e)
        {
            return Result<int>.Error(DatabaseError.Create(e.Message));
        }
    }
}