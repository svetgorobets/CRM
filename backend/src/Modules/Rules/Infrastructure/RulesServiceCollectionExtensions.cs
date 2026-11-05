using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationManagement.BuildingBlocks.Application.Validation;
using NotificationManagement.Rules.Application;

namespace NotificationManagement.Rules.Infrastructure;

public static class RulesServiceCollectionExtensions
{
    public static IServiceCollection AddRulesModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("NotificationManagement")
            ?? throw new InvalidOperationException("Connection string 'NotificationManagement' is required.");

        services.AddDbContext<RulesDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IRuleRepository, RuleRepository>();
        services.AddScoped<IValidator<CreateNotificationRule>, CreateNotificationRuleValidator>();
        services.AddScoped<IRuleService, RuleService>();
        return services;
    }
}
