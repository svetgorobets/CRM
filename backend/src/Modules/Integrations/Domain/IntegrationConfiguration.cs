using NotificationManagement.BuildingBlocks.Domain;

namespace NotificationManagement.Integrations.Domain;

public sealed record IntegrationConfiguration(
    NotificationChannel Channel,
    string Provider,
    bool IsActive,
    DateTimeOffset UpdatedAt);
