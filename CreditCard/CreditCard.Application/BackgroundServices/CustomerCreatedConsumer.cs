﻿using CreditCards.Application.Services;
using CreditCards.Core.Events;
using CreditCards.Infrastructure.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace CreditCards.Application.BackgroundServices
{
    public class CustomerCreatedConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<CustomerCreatedConsumer> _logger;
        private const string Exchange = "customer-service";
        private const string QueueName = "credit-card-service-queue";        
        private const int maxRetry = 3;

        public CustomerCreatedConsumer(IServiceProvider serviceProvider, ILogger<CustomerCreatedConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            var connectionFactory = new ConnectionFactory { HostName = "localhost" };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(Exchange, "fanout", true);
            _channel.QueueDeclare(QueueName, true, false, false, null);
            _channel.QueueBind(QueueName, Exchange, "");
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

                using (var scope = _serviceProvider.CreateScope())
                {

                    var creditCardService = scope.ServiceProvider.GetRequiredService<ICreditCardService>();
                    bool processed = false;
                    int retryCount = 0;

                    while (!processed && retryCount < maxRetry)
                    {
                        try
                        {
                            await creditCardService.HandleCustomerCreatedAsync(customerCreatedEvent, stoppingToken);
                            _channel.BasicAck(ea.DeliveryTag, false);
                            processed = true;
                        }
                        catch (Exception ex)
                        {
                            retryCount++;

                            _logger.LogError(ex, $"Error processing message with Id {customerCreatedEvent.Id}. Attempt {retryCount} of {maxRetry}");

                            if (retryCount >= maxRetry)
                            {
                                _channel.BasicNack(ea.DeliveryTag, false, false); // Reject and do not requeue
                                _logger.LogError($"Message with Id {customerCreatedEvent.Id} could not be processed after {maxRetry} attempts and will be nacked.");
                                processed = true;
                            }
                            else
                            {
                                // Optionally implement a backoff strategy before retrying
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
}
