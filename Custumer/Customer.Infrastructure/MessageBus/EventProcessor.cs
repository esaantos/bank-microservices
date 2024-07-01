using Customers.Core.Events;

namespace Customers.Infrastructure.MessageBus;

public class EventProcessor : IEventProcessor
{
    private readonly IMessageBusClient _messageBusClient;
    private const string Exchange = "customer-service";

    public EventProcessor(IMessageBusClient messageBusClient)
    {
        _messageBusClient = messageBusClient;
    }

    public void Process(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            _messageBusClient.Publish(@event, Exchange);
        }
    }
}

public interface IEventProcessor
{
    void Process (IEnumerable<IDomainEvent> events);
}
