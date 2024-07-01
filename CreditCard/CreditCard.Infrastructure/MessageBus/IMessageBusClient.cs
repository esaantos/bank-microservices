namespace CreditCards.Infrastructure.MessageBus;

public interface IMessageBusClient
{
    Task Publish<T>(T @event, string routingKey, string exchange) where T : class;
}
