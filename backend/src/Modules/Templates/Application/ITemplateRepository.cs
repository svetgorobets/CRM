using NotificationManagement.Templates.Domain;

namespace NotificationManagement.Templates.Application;

public interface ITemplateRepository
{
    Task<NotificationTemplate?> FindAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<NotificationTemplate>> ListAsync(CancellationToken cancellationToken);
    Task AddAsync(NotificationTemplate template, CancellationToken cancellationToken);
    void Remove(NotificationTemplate template);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
