using GradTest.Domain.Common.DomainEvents;

namespace GradTest.Application.Common.Services;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<DomainEventBase> events);
}