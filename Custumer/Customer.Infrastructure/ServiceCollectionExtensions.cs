using Customers.Core.Repositories;
using Customers.Infrastructure.MessageBus;
using Customers.Infrastructure.Persistence;
using Customers.Infrastructure.Persistence.Respositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Customers.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRepositories()
            .AddMessageBus()
            .AddSqlServer(configuration);
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICustomersRepository, CustomersRepository>();
        return services;
    }
    private static IServiceCollection AddMessageBus(this IServiceCollection services)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection("customer-service-producer");

        services.AddSingleton(new ProducerConnection(connection));
        services.AddSingleton<IMessageBusClient, RabbitMQClient>();
        services.AddTransient<IEventProcessor, EventProcessor>();

        return services;
    }
    private static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("CustomersAPI");
        services.AddDbContext<CustomerContext>(p => p.UseSqlServer(connection));

        return services;
    }
}
