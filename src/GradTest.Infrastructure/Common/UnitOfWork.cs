using Microsoft.EntityFrameworkCore;
using GradTest.Application.Common.Contracts;
using GradTest.Domain.Common.DomainEvents;
using GradTest.Domain.Common.Entities;
using GradTest.Infrastructure.Persistence;

namespace GradTest.Infrastructure.Common;

public class UnitOfWork: IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UnitOfWork(ApplicationDbContext applicationDbContext)
    {
        applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        _applicationDbContext = applicationDbContext;
    }

    public Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _applicationDbContext.Database.CurrentTransaction?.CommitAsync(cancellationToken)!;
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _applicationDbContext.Database.CurrentTransaction?.RollbackAsync(cancellationToken)!;
    }
    
    public IEnumerable<DomainEventBase> GetAndClearDomainEvents()
    {
        var domainEvents = _applicationDbContext.ChangeTracker
            .Entries<EntityBase>()
            .SelectMany(entry => entry.Entity.DomainEvents);
    
        var entries = _applicationDbContext.ChangeTracker.Entries<EntityBase>();
        
        foreach (var entry in entries)
        {
            entry.Entity.ClearDomainEvents();
        }
    
        return domainEvents;
    }
}