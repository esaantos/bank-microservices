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

public class CreatedCreditCard : BackgroundService
{
    private readonly ILogger<CreatedCreditCard> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string QueueName = "customer-service/creditcard-created-subscriber";
    private const string Exchange = "credit-card-exchange";
    private const string RoutingKey = "credit-card-created-event";

    public CreatedCreditCard(IServiceProvider serviceProvider, ILogger<CreatedCreditCard> logger)
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
            var @event = JsonConvert.DeserializeObject<CreditCardCreatedEvent>(contentString);

            _logger.LogInformation($"Message CreatedCredit received with Id {@event.CustomerId}");

            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<INotificationHandler<CreditCardCreatedEvent>>();
                bool processed = false;
                int retryCount = 0;
                const int maxRetryAttempts = 3;

                while (!processed && retryCount < maxRetryAttempts)
                {
                    try
                    {
                        await handler.Handle(@event, stoppingToken);
                        _channel.BasicAck(eventArgs.DeliveryTag, false);
                        processed = true;
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        _logger.LogError(ex, $"Error processing message with Id {@event.CustomerId}. Attempt {retryCount} of {maxRetryAttempts}");

                        if (retryCount >= maxRetryAttempts)
                        {
                            _channel.BasicNack(eventArgs.DeliveryTag, false, false); // Reject and do not requeue
                            _logger.LogError($"Message with Id {@event.CustomerId} could not be processed after {maxRetryAttempts} attempts and will be nacked.");
                            processed = true;
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                        }
                    }
                }
            }
        };

        _channel.BasicConsume(QueueName, false, consumer);

        return Task.CompletedTask;
    }
}
