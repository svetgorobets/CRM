using System.Net.Mail;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NotificationManagement.Customers.Application;

public static partial class CustomerInputValidator
{
    public static IReadOnlyDictionary<string, string[]> Validate(CustomerInput input) => Validate(
        input.Name, input.ExternalId, input.Email, input.Phone, input.AttributesJson);

    public static IReadOnlyDictionary<string, string[]> Validate(CustomerUpdateInput input) => Validate(
        input.Name, null, input.Email, input.Phone, input.AttributesJson);

    public static void ThrowIfInvalid(CustomerInput input) => ThrowIfInvalid(Validate(input));
    public static void ThrowIfInvalid(CustomerUpdateInput input) => ThrowIfInvalid(Validate(input));

    private static IReadOnlyDictionary<string, string[]> Validate(string? name, string? externalId, string? email, string? phone, string? attributesJson)
    {
        var errors = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        AddIf(errors, "name", string.IsNullOrWhiteSpace(name), "Name is required.");
        AddIf(errors, "name", name?.Trim().Length > 200, "Name must be 200 characters or fewer.");
        AddIf(errors, "externalId", externalId?.Trim().Length > 200, "External ID must be 200 characters or fewer.");
        AddIf(errors, "externalId", !string.IsNullOrWhiteSpace(externalId) && !ExternalIdPattern().IsMatch(externalId), "External ID may contain letters, numbers, periods, hyphens, colons, and underscores.");
        AddIf(errors, "email", email?.Trim().Length > 320, "Email address must be 320 characters or fewer.");
        AddIf(errors, "email", !string.IsNullOrWhiteSpace(email) && !IsEmailAddress(email), "Enter a valid email address.");
        AddIf(errors, "phone", phone?.Trim().Length > 50, "Phone number must be 50 characters or fewer.");
        AddIf(errors, "phone", !string.IsNullOrWhiteSpace(phone) && !PhonePattern().IsMatch(phone), "Phone number must use E.164 format, for example +14155550100.");
        AddIf(errors, "contact", string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone), "Provide an email address or phone number.");
        AddIf(errors, "attributesJson", !IsJsonObject(attributesJson), "Attributes must be a valid JSON object.");

        return errors.ToDictionary(item => item.Key, item => item.Value.ToArray(), StringComparer.OrdinalIgnoreCase);
    }

    private static void ThrowIfInvalid(IReadOnlyDictionary<string, string[]> errors)
    {
        if (errors.Count > 0) throw new CustomerValidationException(errors);
    }

    private static bool IsEmailAddress(string value)
    {
        try { return new MailAddress(value.Trim()).Address == value.Trim(); }
        catch (FormatException) { return false; }
    }

    private static bool IsJsonObject(string? value)
    {
        try { using var document = JsonDocument.Parse(string.IsNullOrWhiteSpace(value) ? "{}" : value); return document.RootElement.ValueKind == JsonValueKind.Object; }
        catch (JsonException) { return false; }
    }

    private static void AddIf(IDictionary<string, List<string>> errors, string field, bool condition, string message)
    {
        if (!condition) return;
        if (!errors.TryGetValue(field, out var messages)) errors[field] = messages = [];
        messages.Add(message);
    }

    [GeneratedRegex("^[A-Za-z0-9._:-]+$")]
    private static partial Regex ExternalIdPattern();

    [GeneratedRegex("^\\+[1-9]\\d{1,14}$")]
    private static partial Regex PhonePattern();
}

public sealed class CustomerValidationException(IReadOnlyDictionary<string, string[]> errors) : Exception("Customer validation failed.")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}
