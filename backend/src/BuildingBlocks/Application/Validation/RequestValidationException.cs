namespace NotificationManagement.BuildingBlocks.Application.Validation;

public sealed class RequestValidationException(ValidationResult result) : Exception("Request validation failed.")
{
    public ValidationResult Result { get; } = result;
}
