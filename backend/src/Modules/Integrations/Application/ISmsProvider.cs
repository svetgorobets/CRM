namespace NotificationManagement.Integrations.Application;

public interface ISmsProvider
{
    string Name { get; }
    Task<DeliveryResult> SendAsync(SmsMessage message, CancellationToken cancellationToken);
}
