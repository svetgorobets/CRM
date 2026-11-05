namespace NotificationManagement.Customers.Application;

public interface ICustomerService
{
    Task<CustomerDetails> CreateAsync(CustomerInput input, CancellationToken cancellationToken);
    Task<CustomerDetails?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<CustomerSummary>> ListAsync(string? search, CancellationToken cancellationToken);
    Task<CustomerDetails?> UpdateAsync(Guid id, CustomerUpdateInput input, CancellationToken cancellationToken);
    Task<CustomerDetails?> SetPreferenceAsync(Guid id, CustomerPreferenceInput input, CancellationToken cancellationToken);
}
