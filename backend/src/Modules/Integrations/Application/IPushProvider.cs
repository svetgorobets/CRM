namespace NotificationManagement.Integrations.Application;

public interface IPushProvider
{
    string Name { get; }
    Task<DeliveryResult> SendAsync(PushMessage message, CancellationToken cancellationToken);
}
