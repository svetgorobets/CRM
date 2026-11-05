namespace NotificationManagement.BuildingBlocks.Application.Validation;

public sealed class ValidationResult(IReadOnlyDictionary<string, string[]> errors)
{
    public static ValidationResult Success { get; } = new(new Dictionary<string, string[]>());

    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
    public bool IsValid => Errors.Count == 0;
}
