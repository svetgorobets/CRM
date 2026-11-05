using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationManagement.Integrations.Application;

namespace NotificationManagement.Integrations.Infrastructure;

public static class IntegrationsServiceCollectionExtensions
{
    public static IServiceCollection AddIntegrationsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEmailProvider, DevelopmentEmailProvider>();
        services.AddSingleton<ISmsProvider, DevelopmentSmsProvider>();
        services.AddSingleton<IPushProvider, DevelopmentPushProvider>();
        services.AddSingleton<ProviderResolver>();
        services.AddSingleton<IProviderResolver>(provider => provider.GetRequiredService<ProviderResolver>());
        services.AddSingleton<IIntegrationConfigurationService>(provider => provider.GetRequiredService<ProviderResolver>());
        return services;
    }
}
