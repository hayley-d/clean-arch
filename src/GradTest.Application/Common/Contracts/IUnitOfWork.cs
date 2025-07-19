using GradTest.Domain.Common.DomainEvents;

namespace GradTest.Application.Common.Contracts;

public interface IUnitOfWork
{
    Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    IEnumerable<DomainEventBase> GetAndClearDomainEvents();
}