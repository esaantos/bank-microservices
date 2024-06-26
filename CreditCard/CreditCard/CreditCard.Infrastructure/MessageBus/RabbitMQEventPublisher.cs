using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CreditCards.Infrastructure.MessageBus;

public class RabbitMQEventPublisher : IEventPublisher
{
    private readonly IConnection _connection;
    private const string Exchange = "credit-card-exchange";
    private const string RoutingKey = "creditcard-created";

    public RabbitMQEventPublisher(IConnection connection)
    {
        _connection = connection;
    }

    public Task Publish<T>(T @event) where T : class
    {
        using (var channel = _connection.CreateModel())
        {

            channel.ExchangeDeclare("customer-service", "topic", true);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
            channel.BasicPublish(Exchange, RoutingKey, null,  body);
        }
        return Task.CompletedTask;
    }
}
