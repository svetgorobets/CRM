using NotificationManagement.BuildingBlocks.Domain;

namespace NotificationManagement.Rules.Domain;

public sealed class NotificationRule
{
    private NotificationRule() { }

    private NotificationRule(Guid id, string name, string eventType, Guid templateId, NotificationChannel channel, NotificationCategory category, DateTimeOffset now)
    {
        Id = id;
        Name = name;
        EventType = eventType;
        TemplateId = templateId;
        Channel = channel;
        Category = category;
        IsActive = true;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string EventType { get; private set; } = null!;
    public Guid TemplateId { get; private set; }
    public NotificationChannel Channel { get; private set; }
    public NotificationCategory Category { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static NotificationRule Create(string name, string eventType, Guid templateId, NotificationChannel channel, NotificationCategory category, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A rule name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(eventType)) throw new ArgumentException("An event type is required.", nameof(eventType));
        if (templateId == Guid.Empty) throw new ArgumentException("A template is required.", nameof(templateId));

        return new NotificationRule(Guid.NewGuid(), name.Trim(), eventType.Trim().ToUpperInvariant(), templateId, channel, category, now);
    }

    public bool Matches(string eventType) => IsActive && string.Equals(EventType, eventType, StringComparison.OrdinalIgnoreCase);
}
