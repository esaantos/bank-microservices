using Customers.Application.EventHandlers;
using Customers.Application.Events;
using Customers.Application.Subscribers;
using Customers.Core.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Customers.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddSubscribers()
            .AddHandlers();

        return services;
    }

    private static IServiceCollection AddSubscribers(this IServiceCollection services)
    {
        services.AddHostedService<CreatedCreditProposal>();
        services.AddHostedService<CreatedCreditCard>();

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<INotificationHandler<CreditProposalCreatedEvent>, CreditProposalEventHandler>();
        services.AddScoped<INotificationHandler<CreditCardCreatedEvent>, CreditCardEventHandler>();

        return services;
    }
}
