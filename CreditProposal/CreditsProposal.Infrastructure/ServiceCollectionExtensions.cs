using CreditsProposal.Core.Repositories;
using CreditsProposal.Infrastructure.MessageBus;
using CreditsProposal.Infrastructure.Persistence;
using CreditsProposal.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CreditsProposal.Infrastructure;

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
        services.AddScoped<ICreditProposalRepository, CreditProposalRepository>();

        return services;
    }

    private static IServiceCollection AddMessageBus(this IServiceCollection services)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection("creditproposal-service-producer");

        services.AddSingleton(connection);
        services.AddSingleton<IEventPublisher, RabbitMQEventPublisher>();

        return services;
    }

    private static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("CreditProposalDb");
        services.AddDbContext<CreditProposalContext>(p => p.UseSqlServer(connection));

        return services;
    }
}
