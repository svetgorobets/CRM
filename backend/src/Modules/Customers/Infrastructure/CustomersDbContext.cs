using Microsoft.EntityFrameworkCore;
using NotificationManagement.Customers.Domain;

namespace NotificationManagement.Customers.Infrastructure;

public sealed class CustomersDbContext(DbContextOptions<CustomersDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerPreference> CustomerPreferences => Set<CustomerPreference>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers");
            entity.HasKey(customer => customer.Id);
            entity.Property(customer => customer.ExternalId).HasMaxLength(200).IsRequired();
            entity.HasIndex(customer => customer.ExternalId).IsUnique();
            entity.Property(customer => customer.Email).HasMaxLength(320);
            entity.Property(customer => customer.Phone).HasMaxLength(50);
            entity.Property(customer => customer.AttributesJson).HasColumnType("jsonb").IsRequired();
            entity.HasMany(customer => customer.Preferences).WithOne().HasForeignKey(preference => preference.CustomerId);
        });

        modelBuilder.Entity<CustomerPreference>(entity =>
        {
            entity.ToTable("customer_preferences");
            entity.HasKey(preference => new { preference.CustomerId, preference.Channel, preference.Category });
        });
    }
}
