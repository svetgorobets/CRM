using Microsoft.EntityFrameworkCore;
using NotificationManagement.Templates.Domain;

namespace NotificationManagement.Templates.Infrastructure;

public sealed class TemplatesDbContext(DbContextOptions<TemplatesDbContext> options) : DbContext(options)
{
    public DbSet<NotificationTemplate> Templates => Set<NotificationTemplate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationTemplate>(entity =>
        {
            entity.ToTable("notification_templates");
            entity.HasKey(template => template.Id);
            entity.Property(template => template.Name).HasMaxLength(200).IsRequired();
            entity.HasIndex(template => new { template.Name, template.Channel }).IsUnique();
            entity.Property(template => template.Subject).HasMaxLength(500);
            entity.Property(template => template.Content).IsRequired();
        });
    }
}
