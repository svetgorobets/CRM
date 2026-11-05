using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationManagement.Templates.Application;

namespace NotificationManagement.Templates.Infrastructure;

public static class TemplatesServiceCollectionExtensions
{
    public static IServiceCollection AddTemplatesModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("NotificationManagement")
            ?? throw new InvalidOperationException("Connection string 'NotificationManagement' is required.");

        services.AddDbContext<TemplatesDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddScoped<ITemplateService, TemplateService>();
        return services;
    }
}
