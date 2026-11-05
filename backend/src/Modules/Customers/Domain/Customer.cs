using NotificationManagement.BuildingBlocks.Domain;

namespace NotificationManagement.Customers.Domain;

public sealed class Customer
{
    private Customer() { }

    private Customer(Guid id, string externalId, string? email, string? phone, string attributesJson, DateTimeOffset now)
    {
        Id = id;
        ExternalId = externalId;
        Email = email;
        Phone = phone;
        AttributesJson = attributesJson;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public Guid Id { get; private set; }
    public string ExternalId { get; private set; } = null!;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string AttributesJson { get; private set; } = "{}";
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public List<CustomerPreference> Preferences { get; } = [];

    public static Customer Create(string externalId, string? email, string? phone, string attributesJson, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            throw new ArgumentException("An external ID is required.", nameof(externalId));
        }

        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("An email address or phone number is required.");
        }

        return new Customer(Guid.NewGuid(), externalId.Trim(), email?.Trim(), phone?.Trim(), attributesJson, now);
    }

    public void Update(string? email, string? phone, string attributesJson, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("An email address or phone number is required.");
        }

        Email = email?.Trim();
        Phone = phone?.Trim();
        AttributesJson = attributesJson;
        UpdatedAt = now;
    }

    public void SetPreference(NotificationChannel channel, NotificationCategory category, bool enabled, DateTimeOffset now)
    {
        var preference = Preferences.SingleOrDefault(item => item.Channel == channel && item.Category == category);

        if (preference is null)
        {
            Preferences.Add(CustomerPreference.Create(Id, channel, category, enabled, now));
            return;
        }

        preference.SetEnabled(enabled, now);
    }
}
