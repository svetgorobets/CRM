using Microsoft.Extensions.Logging;
using NotificationManagement.Integrations.Application;

namespace NotificationManagement.Integrations.Infrastructure;

public sealed class DevelopmentEmailProvider(ILogger<DevelopmentEmailProvider> logger) : IEmailProvider
{
    public string Name => "Development";

    public Task<DeliveryResult> SendAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Development email accepted for {Recipient}", message.To);
        return Task.FromResult(Accepted());
    }

    private static DeliveryResult Accepted() => new(DeliveryStatus.Accepted, $"dev_email_{Guid.NewGuid():N}", null, null);
}

public sealed class DevelopmentSmsProvider(ILogger<DevelopmentSmsProvider> logger) : ISmsProvider
{
    public string Name => "Development";

    public Task<DeliveryResult> SendAsync(SmsMessage message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Development SMS accepted for {Recipient}", message.To);
        return Task.FromResult(Accepted());
    }

    private static DeliveryResult Accepted() => new(DeliveryStatus.Accepted, $"dev_sms_{Guid.NewGuid():N}", null, null);
}

public sealed class DevelopmentPushProvider(ILogger<DevelopmentPushProvider> logger) : IPushProvider
{
    public string Name => "Development";

    public Task<DeliveryResult> SendAsync(PushMessage message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Development push accepted for token ending {TokenSuffix}", message.DeviceToken[^Math.Min(4, message.DeviceToken.Length)..]);
        return Task.FromResult(Accepted());
    }

    private static DeliveryResult Accepted() => new(DeliveryStatus.Accepted, $"dev_push_{Guid.NewGuid():N}", null, null);
}
