namespace NotificationManagement.Rules.Application;

public interface IRuleService
{
    Task<NotificationRuleSummary> CreateAsync(CreateNotificationRule input, CancellationToken cancellationToken);
    Task<IReadOnlyList<NotificationRuleSummary>> ListAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<NotificationAction>> EvaluateAsync(string eventType, CancellationToken cancellationToken);
}
