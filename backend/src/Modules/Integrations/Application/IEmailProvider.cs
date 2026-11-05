namespace NotificationManagement.Integrations.Application;

public interface IEmailProvider
{
    string Name { get; }
    Task<DeliveryResult> SendAsync(EmailMessage message, CancellationToken cancellationToken);
}
