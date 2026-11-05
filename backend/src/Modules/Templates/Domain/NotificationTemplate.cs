using NotificationManagement.BuildingBlocks.Domain;

namespace NotificationManagement.Templates.Domain;

public sealed class NotificationTemplate
{
    private NotificationTemplate() { }

    private NotificationTemplate(Guid id, string name, NotificationChannel channel, string? subject, string content, DateTimeOffset now)
    {
        Id = id;
        Name = name;
        Channel = channel;
        Subject = subject;
        Content = content;
        IsActive = true;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public NotificationChannel Channel { get; private set; }
    public string? Subject { get; private set; }
    public string Content { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static NotificationTemplate Create(string name, NotificationChannel channel, string? subject, string content, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A template name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Template content is required.", nameof(content));

        return new NotificationTemplate(Guid.NewGuid(), name.Trim(), channel, subject?.Trim(), content, now);
    }

    public void Update(string name, string? subject, string content, bool isActive, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A template name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Template content is required.", nameof(content));

        Name = name.Trim();
        Subject = subject?.Trim();
        Content = content;
        IsActive = isActive;
        UpdatedAt = now;
    }
}
