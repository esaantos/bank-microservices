using CreditsProposal.Application.BackgroundServices;
using CreditsProposal.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CreditsProposal.Application;

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
        services.AddScoped<ICreditProposalService, CreditProposalService>();

        return services;
    }

    private static IServiceCollection AddSubscribers(this IServiceCollection services)
    {
        services.AddHostedService<CustomerCreatedConsumer>();

        return services;
    }

}
