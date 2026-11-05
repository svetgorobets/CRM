using NotificationManagement.Worker;
using NotificationManagement.BuildingBlocks.Infrastructure.DependencyInjection;
using NotificationManagement.Integrations.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddPlatformPersistence(builder.Configuration);
builder.Services.AddIntegrationsModule(builder.Configuration);
builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
host.Run();
