namespace Customers.Infrastructure.MessageBus;

public interface IMessageBusClient
{
    void Publish<T>(T @event, string exchange) where T: class;
}
