using GradTest.Domain.Common.DomainEvents;

namespace GradTest.Domain.Common.Entities;

public interface IEntityBase
{
    public IEnumerable<DomainEventBase> DomainEvents { get; }
    public void AddDomainEvent(DomainEventBase domainEvent);
    void ClearDomainEvents();
}