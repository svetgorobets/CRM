using NotificationManagement.Integrations.Domain;

namespace NotificationManagement.Integrations.Application;

public enum DeliveryStatus
{
    Accepted,
    Rejected,
    Failed
}

public sealed record DeliveryResult(
    DeliveryStatus Status,
    string? ProviderMessageId,
    string? ErrorCode,
    string? ErrorMessage);

public sealed record EmailMessage(string To, string? Subject, string Content);
public sealed record SmsMessage(string To, string Content);
public sealed record PushMessage(string DeviceToken, string Title, string Content);

public interface IEmailProvider
{
    string Name { get; }
    Task<DeliveryResult> SendAsync(EmailMessage message, CancellationToken cancellationToken);
}

public interface ISmsProvider
{
    string Name { get; }
    Task<DeliveryResult> SendAsync(SmsMessage message, CancellationToken cancellationToken);
}

public interface IPushProvider
{
    string Name { get; }
    Task<DeliveryResult> SendAsync(PushMessage message, CancellationToken cancellationToken);
}

public interface IProviderResolver
{
    IEmailProvider GetEmailProvider();
    ISmsProvider GetSmsProvider();
    IPushProvider GetPushProvider();
}

public interface IIntegrationConfigurationService
{
    IReadOnlyList<IntegrationConfiguration> GetActiveConfigurations();
}
