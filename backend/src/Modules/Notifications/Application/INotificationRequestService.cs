namespace NotificationManagement.Notifications.Application;

public interface INotificationRequestService
{
    Task<NotificationRequestSummary> CreateAsync(CreateNotificationRequest request, CancellationToken cancellationToken);
}
