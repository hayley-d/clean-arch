namespace GradTest.Domain.Common.DomainEvents;

public class DomainEventBase
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}