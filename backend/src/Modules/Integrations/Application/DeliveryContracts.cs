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

