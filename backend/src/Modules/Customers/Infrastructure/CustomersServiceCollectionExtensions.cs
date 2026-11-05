using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationManagement.Customers.Application;

namespace NotificationManagement.Customers.Infrastructure;

public static class CustomersServiceCollectionExtensions
{
    public static IServiceCollection AddCustomersModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("NotificationManagement")
            ?? throw new InvalidOperationException("Connection string 'NotificationManagement' is required.");

        services.AddDbContext<CustomersDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        return services;
    }
}
