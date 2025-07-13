using Azure.Messaging.ServiceBus;
using NotificationService.Worker;
using NotificationService.Worker.Interfaces;
using NotificationService.Worker.Services;
using NotificationService.Worker.Workers;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        IConfiguration config = context.Configuration;
        string? connectionString = config["ConnectionStrings:AzureServiceBus"];

        services.AddSingleton(new ServiceBusClient(connectionString));
        services.AddSingleton<INotificationProcessor, NotificationProcessor>();
        services.AddHostedService<NotificationQueueWorker>();

    })
    .Build();


host.Run();
