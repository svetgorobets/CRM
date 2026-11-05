using Microsoft.EntityFrameworkCore;
using NotificationManagement.Rules.Application;
using NotificationManagement.Rules.Domain;

namespace NotificationManagement.Rules.Infrastructure;

public sealed class RuleRepository(RulesDbContext dbContext) : IRuleRepository
{
    public Task AddAsync(NotificationRule rule, CancellationToken cancellationToken) => dbContext.Rules.AddAsync(rule, cancellationToken).AsTask();

    public async Task<IReadOnlyList<NotificationRule>> ListActiveForEventAsync(string eventType, CancellationToken cancellationToken) =>
        await dbContext.Rules.Where(rule => rule.IsActive && rule.EventType == eventType).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<NotificationRule>> ListAsync(CancellationToken cancellationToken) =>
        await dbContext.Rules.AsNoTracking().OrderBy(rule => rule.Name).ToListAsync(cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
