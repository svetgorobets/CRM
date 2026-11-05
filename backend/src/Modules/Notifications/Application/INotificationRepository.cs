using NotificationManagement.Notifications.Domain;

namespace NotificationManagement.Notifications.Application;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
