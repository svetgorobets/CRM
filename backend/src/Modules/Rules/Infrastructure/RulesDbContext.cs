using Microsoft.EntityFrameworkCore;
using NotificationManagement.Rules.Domain;

namespace NotificationManagement.Rules.Infrastructure;

public sealed class RulesDbContext(DbContextOptions<RulesDbContext> options) : DbContext(options)
{
    public DbSet<NotificationRule> Rules => Set<NotificationRule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationRule>(entity =>
        {
            entity.ToTable("notification_rules");
            entity.HasKey(rule => rule.Id);
            entity.Property(rule => rule.Name).HasMaxLength(200).IsRequired();
            entity.Property(rule => rule.EventType).HasMaxLength(100).IsRequired();
            entity.HasIndex(rule => new { rule.EventType, rule.IsActive });
        });
    }
}
