using NotificationManagement.BuildingBlocks.Domain;

namespace NotificationManagement.Customers.Domain;

public sealed class CustomerPreference
{
    private CustomerPreference() { }

    private CustomerPreference(Guid customerId, NotificationChannel channel, NotificationCategory category, bool enabled, DateTimeOffset updatedAt)
    {
        CustomerId = customerId;
        Channel = channel;
        Category = category;
        Enabled = enabled;
        UpdatedAt = updatedAt;
    }

    public Guid CustomerId { get; private set; }
    public NotificationChannel Channel { get; private set; }
    public NotificationCategory Category { get; private set; }
    public bool Enabled { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    internal static CustomerPreference Create(Guid customerId, NotificationChannel channel, NotificationCategory category, bool enabled, DateTimeOffset updatedAt) =>
        new(customerId, channel, category, enabled, updatedAt);

    internal void SetEnabled(bool enabled, DateTimeOffset updatedAt)
    {
        Enabled = enabled;
        UpdatedAt = updatedAt;
    }
}
