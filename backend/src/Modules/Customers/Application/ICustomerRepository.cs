using NotificationManagement.Customers.Domain;

namespace NotificationManagement.Customers.Application;

public interface ICustomerRepository
{
    Task<Customer?> FindAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExternalIdExistsAsync(string externalId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Customer>> ListAsync(string? search, CancellationToken cancellationToken);
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
