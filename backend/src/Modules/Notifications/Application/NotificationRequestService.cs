using NotificationManagement.BuildingBlocks.Domain;
using NotificationManagement.Notifications.Domain;

namespace NotificationManagement.Notifications.Application;

public sealed record CreateNotificationRequest(
    Guid CustomerId,
    Guid TemplateId,
    NotificationChannel Channel,
    NotificationCategory Category,
    string DataJson = "{}");

public sealed record NotificationRequestSummary(
    Guid Id,
    Guid CustomerId,
    Guid TemplateId,
    NotificationChannel Channel,
    NotificationCategory Category,
    NotificationStatus Status,
    DateTimeOffset RequestedAt);

public sealed class NotificationRequestService(INotificationRepository repository, TimeProvider timeProvider) : INotificationRequestService
{
    public async Task<NotificationRequestSummary> CreateAsync(CreateNotificationRequest request, CancellationToken cancellationToken)
    {
        var notification = Notification.Request(
            request.CustomerId,
            request.TemplateId,
            request.Channel,
            request.Category,
            request.DataJson,
            timeProvider.GetUtcNow());

        await repository.AddAsync(notification, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new NotificationRequestSummary(
            notification.Id,
            notification.CustomerId,
            notification.TemplateId,
            notification.Channel,
            notification.Category,
            notification.Status,
            notification.RequestedAt);
    }
}
