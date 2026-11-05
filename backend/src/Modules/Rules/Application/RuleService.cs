using NotificationManagement.BuildingBlocks.Domain;
using NotificationManagement.BuildingBlocks.Application.Validation;
using NotificationManagement.Rules.Domain;

namespace NotificationManagement.Rules.Application;

public sealed record CreateNotificationRule(
    string Name,
    string EventType,
    Guid TemplateId,
    NotificationChannel Channel,
    NotificationCategory Category);

public sealed record NotificationRuleSummary(
    Guid Id,
    string Name,
    string EventType,
    Guid TemplateId,
    NotificationChannel Channel,
    NotificationCategory Category,
    bool IsActive);

public sealed record NotificationAction(Guid RuleId, Guid TemplateId, NotificationChannel Channel, NotificationCategory Category);

public sealed class RuleService(IRuleRepository repository, IValidator<CreateNotificationRule> validator, TimeProvider timeProvider) : IRuleService
{
    public async Task<NotificationRuleSummary> CreateAsync(CreateNotificationRule input, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(input);
        if (!validationResult.IsValid) throw new RequestValidationException(validationResult);

        var rule = NotificationRule.Create(input.Name, input.EventType, input.TemplateId, input.Channel, input.Category, timeProvider.GetUtcNow());
        await repository.AddAsync(rule, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return ToSummary(rule);
    }

    public async Task<IReadOnlyList<NotificationRuleSummary>> ListAsync(CancellationToken cancellationToken) =>
        (await repository.ListAsync(cancellationToken)).Select(ToSummary).ToArray();

    public async Task<IReadOnlyList<NotificationAction>> EvaluateAsync(string eventType, CancellationToken cancellationToken) =>
        (await repository.ListActiveForEventAsync(eventType.Trim().ToUpperInvariant(), cancellationToken))
            .Where(rule => rule.Matches(eventType))
            .Select(rule => new NotificationAction(rule.Id, rule.TemplateId, rule.Channel, rule.Category))
            .ToArray();

    private static NotificationRuleSummary ToSummary(NotificationRule rule) =>
        new(rule.Id, rule.Name, rule.EventType, rule.TemplateId, rule.Channel, rule.Category, rule.IsActive);
}
