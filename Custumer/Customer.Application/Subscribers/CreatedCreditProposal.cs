using Customers.Core.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Customers.Application.Subscribers;

public class CreatedCreditProposal : BackgroundService
{
    private readonly ILogger<CreatedCreditProposal> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string QueueName = "customer-service/proposal-created-subscriber";
    private const string Exchange = "credit-proposal-exchange";
    private const string RoutingKey = "proposal-created";

    public CreatedCreditProposal(IServiceProvider serviceProvider, ILogger<CreatedCreditProposal> logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        var connectionFactory = new ConnectionFactory { HostName = "localhost" };

        _connection = connectionFactory.CreateConnection();

        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(Exchange, "topic", true);
        _channel.QueueDeclare(QueueName, true, false, false, null);
        _channel.QueueBind(QueueName, Exchange, RoutingKey);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, eventArgs) => {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);
            var @event = JsonConvert.DeserializeObject<CreditProposalCreatedEvent>(contentString);

            _logger.LogInformation($"Message CreatedCreditProposal received with Id {@event.ProposalId}");
            _channel.BasicAck(eventArgs.DeliveryTag, false);

            using(var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<INotificationHandler<CreditProposalCreatedEvent>>();
                await handler.Handle(@event, stoppingToken);
            }
        };

        _channel.BasicConsume(QueueName, false, consumer);

        return Task.CompletedTask;
    }
}
