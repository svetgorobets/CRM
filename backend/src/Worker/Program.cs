using NotificationManagement.Worker;
using NotificationManagement.BuildingBlocks.Infrastructure.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddPlatformPersistence(builder.Configuration);
builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
host.Run();
