using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CreditCards.Infrastructure.MessageBus;

public class RabbitMQEventPublisher : IMessageBusClient
{
    private readonly IConnection _connection;
    //private const string Exchange = "credit-card-exchange";
    //private const string RoutingKey = "creditcard-created";

    public RabbitMQEventPublisher(IConnection connection)
    {
        _connection = connection;
    }

    public Task Publish<T>(T @event, string routingKey, string exchange) where T : class
    {
        using (var channel = _connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange, "topic", true);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
            channel.BasicPublish(exchange, routingKey, null,  body);
        }
        return Task.CompletedTask;
    }
}
