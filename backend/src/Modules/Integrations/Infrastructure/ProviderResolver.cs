using Microsoft.Extensions.Configuration;
using NotificationManagement.BuildingBlocks.Domain;
using NotificationManagement.Integrations.Application;
using NotificationManagement.Integrations.Domain;

namespace NotificationManagement.Integrations.Infrastructure;

public sealed class ProviderResolver(
    IConfiguration configuration,
    IEmailProvider emailProvider,
    ISmsProvider smsProvider,
    IPushProvider pushProvider) : IProviderResolver, IIntegrationConfigurationService
{
    public IEmailProvider GetEmailProvider() => Resolve(NotificationChannel.Email, emailProvider);
    public ISmsProvider GetSmsProvider() => Resolve(NotificationChannel.Sms, smsProvider);
    public IPushProvider GetPushProvider() => Resolve(NotificationChannel.Push, pushProvider);

    public IReadOnlyList<IntegrationConfiguration> GetActiveConfigurations() =>
    [
        ToConfiguration(NotificationChannel.Email, GetEmailProvider().Name),
        ToConfiguration(NotificationChannel.Sms, GetSmsProvider().Name),
        ToConfiguration(NotificationChannel.Push, GetPushProvider().Name)
    ];

    private TProvider Resolve<TProvider>(NotificationChannel channel, TProvider provider) where TProvider : class
    {
        var configuredProvider = configuration[$"Integrations:{channel}:Provider"] ?? "Development";
        var providerName = provider switch
        {
            IEmailProvider email => email.Name,
            ISmsProvider sms => sms.Name,
            IPushProvider push => push.Name,
            _ => throw new InvalidOperationException("Unsupported provider contract.")
        };

        if (!string.Equals(configuredProvider, providerName, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"No active {channel} provider named '{configuredProvider}' is registered.");
        }

        return provider;
    }

    private IntegrationConfiguration ToConfiguration(NotificationChannel channel, string provider) =>
        new(channel, provider, true, DateTimeOffset.UtcNow);
}
