using CreditCards.Core.Repositories;
using CreditCards.Infrastructure.MessageBus;
using CreditCards.Infrastructure.Persistence;
using CreditCards.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CreditCards.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRepositories()
            .AddMessageBus()
            .AddSqlServer(configuration);

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICreditCardRepository, CreditCardRepository>();

        return services;
    }

    private static IServiceCollection AddMessageBus(this IServiceCollection services)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection("creditcard-service-producer");

        services.AddSingleton(connection);
        services.AddSingleton<IMessageBusClient, RabbitMQEventPublisher>();
        services.AddTransient<IEventProcessor, EventProcessor>();
        return services;
    }

    private static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("CreditCardDb");
        services.AddDbContext<CreditCardContext>(p => p.UseSqlServer(connection));

        return services;
    }
}
