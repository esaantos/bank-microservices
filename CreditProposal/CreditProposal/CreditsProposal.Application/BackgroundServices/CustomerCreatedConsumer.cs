using CreditsProposal.Application.Services;
using CreditsProposal.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CreditsProposal.Application.BackgroundServices;

public class CustomerCreatedConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<CustomerCreatedConsumer> _logger;
    private const string Queue = "credit-proposal-service-queue";
    private const string Exchange = "customer-service";
    private const int MaxRetry = 3;

    public CustomerCreatedConsumer(IServiceProvider serviceProvider, ILogger<CustomerCreatedConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        var connectionFactory = new ConnectionFactory { HostName = "localhost" };

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(Exchange, "topic", true);
        _channel.QueueDeclare(Queue, true, false, false, null);
        _channel.QueueBind(Queue, Exchange, "customer-created");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var customerCreatedEvent = JsonConvert.DeserializeObject<CustomerCreated>(message);

            _logger.LogInformation($"Message CustomerCreated received with Id {customerCreatedEvent.Id}");

            var success = false;
            using (var scope = _serviceProvider.CreateScope())
            {
                var creditCardService = scope.ServiceProvider.GetRequiredService<ICreditProposalService>();
                success = await creditCardService.HandlerProposalCreateAsync(customerCreatedEvent, stoppingToken);
            }

            if (success)
                _channel.BasicAck(ea.DeliveryTag, false);
            else
            {
                int retryCount = 0;
                if (ea.BasicProperties.Headers != null && ea.BasicProperties.Headers.ContainsKey("x-retry-count"))
                {
                    retryCount = (int)ea.BasicProperties.Headers["x-retry-count"];
                }

                if (retryCount < MaxRetry)
                {
                    var properties = _channel.CreateBasicProperties();
                    properties.Headers = ea.BasicProperties.Headers ?? new Dictionary<string, object>();
                    properties.Headers["x-retry-count"] = ++retryCount;

                    _channel.BasicPublish(
                        exchange: Exchange,
                        routingKey: "customer-created",
                        basicProperties: properties,
                        body: body
                    );
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    _logger.LogError($"Message CustomerCreated with Id {customerCreatedEvent.Id} failed after {MaxRetry} attempts.");
                    _channel.BasicNack(ea.DeliveryTag, false, false); // Optionally move to a dead-letter queue
                }
            }
        };
        _channel.BasicConsume(Queue, false, consumer);

        return Task.CompletedTask;
    }
}
