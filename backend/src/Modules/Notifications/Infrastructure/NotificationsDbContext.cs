using Microsoft.EntityFrameworkCore;
using NotificationManagement.Notifications.Domain;

namespace NotificationManagement.Notifications.Infrastructure;

public sealed class NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");
            entity.HasKey(notification => notification.Id);
            entity.Property(notification => notification.DataJson).HasColumnType("jsonb").IsRequired();
            entity.HasIndex(notification => new { notification.CustomerId, notification.RequestedAt });
            entity.HasIndex(notification => new { notification.Status, notification.RequestedAt });
        });
    }
}
