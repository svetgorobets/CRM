using NotificationManagement.Worker;
using NotificationManagement.BuildingBlocks.Infrastructure.DependencyInjection;
using NotificationManagement.Integrations.Infrastructure;
using NotificationManagement.Notifications.Infrastructure;
using NotificationManagement.Rules.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddPlatformPersistence(builder.Configuration);
builder.Services.AddIntegrationsModule(builder.Configuration);
builder.Services.AddNotificationsModule(builder.Configuration);
builder.Services.AddRulesModule(builder.Configuration);
builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
host.Run();
