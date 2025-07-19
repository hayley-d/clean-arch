using MassTransit;
using Microsoft.Extensions.Logging;
using GradTest.Application.Common.Services;
using GradTest.Domain.Common.DomainEvents;

namespace GradTest.Infrastructure.Common.Services;

public class EventPublisher: IEventPublisher
{
    private readonly IPublishEndpoint _endpoint;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(
        IPublishEndpoint endpoint,
        ILogger<EventPublisher> logger)
    {
        _endpoint = endpoint;
        _logger = logger;
    }
    
    public async Task PublishEventsAsync(IEnumerable<DomainEventBase> events)
    {
        foreach (var domainEvent in events)
        {
            await _endpoint.Publish(domainEvent.GetType(), domainEvent);
            _logger.LogInformation(
                "Publishing Event: {@DomeEventType}, Event: {@DomainEvent}", 
                domainEvent.GetType(),
                domainEvent);
        }
    }
}