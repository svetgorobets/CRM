using NotificationManagement.BuildingBlocks.Application.Validation;

namespace NotificationManagement.Rules.Application;

public sealed class CreateNotificationRuleValidator : IValidator<CreateNotificationRule>
{
    public ValidationResult Validate(CreateNotificationRule input)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(input.Name)) errors["name"] = ["Rule name is required."];
        else if (input.Name.Trim().Length > 200) errors["name"] = ["Rule name must be 200 characters or fewer."];

        if (string.IsNullOrWhiteSpace(input.EventType)) errors["eventType"] = ["Event type is required."];
        else if (input.EventType.Trim().Length > 100) errors["eventType"] = ["Event type must be 100 characters or fewer."];

        if (input.TemplateId == Guid.Empty) errors["templateId"] = ["Template ID is required."];

        return errors.Count == 0 ? ValidationResult.Success : new ValidationResult(errors);
    }
}
