using NotificationManagement.Rules.Domain;

namespace NotificationManagement.Rules.Application;

public interface IRuleRepository
{
    Task AddAsync(NotificationRule rule, CancellationToken cancellationToken);
    Task<IReadOnlyList<NotificationRule>> ListActiveForEventAsync(string eventType, CancellationToken cancellationToken);
    Task<IReadOnlyList<NotificationRule>> ListAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
