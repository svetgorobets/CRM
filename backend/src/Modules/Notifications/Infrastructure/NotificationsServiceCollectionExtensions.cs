using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationManagement.Notifications.Application;

namespace NotificationManagement.Notifications.Infrastructure;

public static class NotificationsServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationsModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("NotificationManagement")
            ?? throw new InvalidOperationException("Connection string 'NotificationManagement' is required.");

        services.AddDbContext<NotificationsDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationRequestService, NotificationRequestService>();
        return services;
    }
}
