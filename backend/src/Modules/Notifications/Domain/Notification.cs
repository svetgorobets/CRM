using NotificationManagement.BuildingBlocks.Domain;

namespace NotificationManagement.Notifications.Domain;

public enum NotificationStatus
{
    Requested,
    Processing,
    Accepted,
    Delivered,
    Failed,
    Rejected
}

public sealed class Notification
{
    private Notification() { }

    private Notification(
        Guid id,
        Guid customerId,
        Guid templateId,
        NotificationChannel channel,
        NotificationCategory category,
        string dataJson,
        DateTimeOffset requestedAt)
    {
        Id = id;
        CustomerId = customerId;
        TemplateId = templateId;
        Channel = channel;
        Category = category;
        DataJson = dataJson;
        Status = NotificationStatus.Requested;
        RequestedAt = requestedAt;
    }

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid TemplateId { get; private set; }
    public NotificationChannel Channel { get; private set; }
    public NotificationCategory Category { get; private set; }
    public string DataJson { get; private set; } = "{}";
    public NotificationStatus Status { get; private set; }
    public DateTimeOffset RequestedAt { get; private set; }

    public static Notification Request(
        Guid customerId,
        Guid templateId,
        NotificationChannel channel,
        NotificationCategory category,
        string dataJson,
        DateTimeOffset requestedAt)
    {
        if (customerId == Guid.Empty) throw new ArgumentException("A customer is required.", nameof(customerId));
        if (templateId == Guid.Empty) throw new ArgumentException("A template is required.", nameof(templateId));

        return new Notification(Guid.NewGuid(), customerId, templateId, channel, category, dataJson, requestedAt);
    }
}
