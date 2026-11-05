using NotificationManagement.Integrations.Domain;

namespace NotificationManagement.Integrations.Application;

public interface IIntegrationConfigurationService
{
    IReadOnlyList<IntegrationConfiguration> GetActiveConfigurations();
}
