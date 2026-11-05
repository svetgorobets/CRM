using NotificationManagement.BuildingBlocks.Domain;
using NotificationManagement.Customers.Domain;

namespace NotificationManagement.Customers.Application;

public sealed record CustomerInput(string? Name, string? ExternalId, string? Email, string? Phone, string? AttributesJson = "{}");
public sealed record CustomerUpdateInput(string? Name, string? Email, string? Phone, string? AttributesJson = "{}");
public sealed record CustomerPreferenceInput(NotificationChannel Channel, NotificationCategory Category, bool Enabled);
public sealed record CustomerSummary(Guid Id, string Name, string? ExternalId, string? Email, string? Phone, string AttributesJson, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
public sealed record CustomerDetails(CustomerSummary Customer, IReadOnlyList<CustomerPreferenceInput> Preferences);

public sealed class CustomerService(ICustomerRepository repository, TimeProvider timeProvider) : ICustomerService
{
    public async Task<CustomerDetails> CreateAsync(CustomerInput input, CancellationToken cancellationToken)
    {
        CustomerInputValidator.ThrowIfInvalid(input);
        if (!string.IsNullOrWhiteSpace(input.ExternalId) && await repository.ExternalIdExistsAsync(input.ExternalId.Trim(), cancellationToken))
        {
            throw new CustomerValidationException(new Dictionary<string, string[]>
            {
                ["externalId"] = ["External ID is already in use."]
            });
        }

        var attributesJson = string.IsNullOrWhiteSpace(input.AttributesJson) ? "{}" : input.AttributesJson;
        var customer = Customer.Create(input.Name!, input.ExternalId, input.Email, input.Phone, attributesJson, timeProvider.GetUtcNow());
        await repository.AddAsync(customer, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return ToDetails(customer);
    }

    public async Task<CustomerDetails?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await repository.FindAsync(id, cancellationToken);
        return customer is null ? null : ToDetails(customer);
    }

    public async Task<IReadOnlyList<CustomerSummary>> ListAsync(string? search, CancellationToken cancellationToken) =>
        (await repository.ListAsync(search, cancellationToken)).Select(ToSummary).ToArray();

    public async Task<CustomerDetails?> UpdateAsync(Guid id, CustomerUpdateInput input, CancellationToken cancellationToken)
    {
        CustomerInputValidator.ThrowIfInvalid(input);
        var customer = await repository.FindAsync(id, cancellationToken);
        if (customer is null) return null;

        var attributesJson = string.IsNullOrWhiteSpace(input.AttributesJson) ? "{}" : input.AttributesJson;
        customer.Update(input.Name!, input.Email, input.Phone, attributesJson, timeProvider.GetUtcNow());
        await repository.SaveChangesAsync(cancellationToken);
        return ToDetails(customer);
    }

    public async Task<CustomerDetails?> SetPreferenceAsync(Guid id, CustomerPreferenceInput input, CancellationToken cancellationToken)
    {
        var customer = await repository.FindAsync(id, cancellationToken);
        if (customer is null) return null;

        customer.SetPreference(input.Channel, input.Category, input.Enabled, timeProvider.GetUtcNow());
        await repository.SaveChangesAsync(cancellationToken);
        return ToDetails(customer);
    }

    private static CustomerSummary ToSummary(Customer customer) =>
        new(customer.Id, customer.Name, customer.ExternalId, customer.Email, customer.Phone, customer.AttributesJson, customer.CreatedAt, customer.UpdatedAt);

    private static CustomerDetails ToDetails(Customer customer) =>
        new(ToSummary(customer), customer.Preferences.Select(item => new CustomerPreferenceInput(item.Channel, item.Category, item.Enabled)).ToArray());
}
