using NotificationManagement.BuildingBlocks.Domain;
using NotificationManagement.Customers.Domain;

namespace NotificationManagement.Customers.Application;

public sealed record CustomerInput(string ExternalId, string? Email, string? Phone, string AttributesJson = "{}");
public sealed record CustomerUpdateInput(string? Email, string? Phone, string AttributesJson = "{}");
public sealed record CustomerPreferenceInput(NotificationChannel Channel, NotificationCategory Category, bool Enabled);
public sealed record CustomerSummary(Guid Id, string ExternalId, string? Email, string? Phone, string AttributesJson, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
public sealed record CustomerDetails(CustomerSummary Customer, IReadOnlyList<CustomerPreferenceInput> Preferences);

public interface ICustomerRepository
{
    Task<Customer?> FindAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Customer>> ListAsync(string? search, CancellationToken cancellationToken);
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public interface ICustomerService
{
    Task<CustomerDetails> CreateAsync(CustomerInput input, CancellationToken cancellationToken);
    Task<CustomerDetails?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<CustomerSummary>> ListAsync(string? search, CancellationToken cancellationToken);
    Task<CustomerDetails?> UpdateAsync(Guid id, CustomerUpdateInput input, CancellationToken cancellationToken);
    Task<CustomerDetails?> SetPreferenceAsync(Guid id, CustomerPreferenceInput input, CancellationToken cancellationToken);
}

public sealed class CustomerService(ICustomerRepository repository, TimeProvider timeProvider) : ICustomerService
{
    public async Task<CustomerDetails> CreateAsync(CustomerInput input, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(input.ExternalId, input.Email, input.Phone, input.AttributesJson, timeProvider.GetUtcNow());
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
        var customer = await repository.FindAsync(id, cancellationToken);
        if (customer is null) return null;

        customer.Update(input.Email, input.Phone, input.AttributesJson, timeProvider.GetUtcNow());
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
        new(customer.Id, customer.ExternalId, customer.Email, customer.Phone, customer.AttributesJson, customer.CreatedAt, customer.UpdatedAt);

    private static CustomerDetails ToDetails(Customer customer) =>
        new(ToSummary(customer), customer.Preferences.Select(item => new CustomerPreferenceInput(item.Channel, item.Category, item.Enabled)).ToArray());
}
