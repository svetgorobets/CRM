namespace NotificationManagement.Integrations.Application;

public interface IProviderResolver
{
    IEmailProvider GetEmailProvider();
    ISmsProvider GetSmsProvider();
    IPushProvider GetPushProvider();
}
