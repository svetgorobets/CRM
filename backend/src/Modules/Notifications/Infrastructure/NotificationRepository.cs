using NotificationManagement.Notifications.Application;
using NotificationManagement.Notifications.Domain;

namespace NotificationManagement.Notifications.Infrastructure;

public sealed class NotificationRepository(NotificationsDbContext dbContext) : INotificationRepository
{
    public Task AddAsync(Notification notification, CancellationToken cancellationToken) =>
        dbContext.Notifications.AddAsync(notification, cancellationToken).AsTask();

    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
