using Microsoft.EntityFrameworkCore;
using NotificationManagement.Customers.Application;
using NotificationManagement.Customers.Domain;

namespace NotificationManagement.Customers.Infrastructure;

public sealed class CustomerRepository(CustomersDbContext dbContext) : ICustomerRepository
{
    public Task<Customer?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.Customers.Include(customer => customer.Preferences).SingleOrDefaultAsync(customer => customer.Id == id, cancellationToken);

    public Task<bool> ExternalIdExistsAsync(string externalId, CancellationToken cancellationToken) =>
        dbContext.Customers.AnyAsync(customer => customer.ExternalId == externalId, cancellationToken);

    public async Task<IReadOnlyList<Customer>> ListAsync(string? search, CancellationToken cancellationToken)
    {
        var query = dbContext.Customers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(customer => EF.Functions.ILike(customer.Name, pattern)
                || (customer.ExternalId != null && EF.Functions.ILike(customer.ExternalId, pattern))
                || (customer.Email != null && EF.Functions.ILike(customer.Email, pattern))
                || (customer.Phone != null && EF.Functions.ILike(customer.Phone, pattern)));
        }

        return await query.OrderByDescending(customer => customer.CreatedAt).ToListAsync(cancellationToken);
    }

    public Task AddAsync(Customer customer, CancellationToken cancellationToken) => dbContext.Customers.AddAsync(customer, cancellationToken).AsTask();
    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
