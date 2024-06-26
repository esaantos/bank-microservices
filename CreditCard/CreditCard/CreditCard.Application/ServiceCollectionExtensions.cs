using CreditCards.Application.BackgroundServices;
using CreditCards.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CreditCards.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddServices()
            .AddSubscribers();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICreditCardService, CreditCardService>();

        return services;
    }

    private static IServiceCollection AddSubscribers(this  IServiceCollection services)
    {
        services.AddHostedService<CustomerCreatedConsumer>();

        return services;
    }
}
