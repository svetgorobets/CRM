using Microsoft.EntityFrameworkCore;
using NotificationManagement.Templates.Application;
using NotificationManagement.Templates.Domain;

namespace NotificationManagement.Templates.Infrastructure;

public sealed class TemplateRepository(TemplatesDbContext dbContext) : ITemplateRepository
{
    public Task<NotificationTemplate?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.Templates.SingleOrDefaultAsync(template => template.Id == id, cancellationToken);

    public async Task<IReadOnlyList<NotificationTemplate>> ListAsync(CancellationToken cancellationToken) =>
        await dbContext.Templates.AsNoTracking().OrderBy(template => template.Name).ToListAsync(cancellationToken);

    public Task AddAsync(NotificationTemplate template, CancellationToken cancellationToken) => dbContext.Templates.AddAsync(template, cancellationToken).AsTask();
    public void Remove(NotificationTemplate template) => dbContext.Templates.Remove(template);
    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
