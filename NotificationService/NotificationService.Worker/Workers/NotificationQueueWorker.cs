using Azure.Messaging.ServiceBus;
using NotificationService.Worker.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Worker.Workers
{
    public class NotificationQueueWorker : BackgroundService
    {
        private readonly ServiceBusProcessor _processor;
        private readonly ILogger<NotificationQueueWorker> _logger;
        private readonly INotificationProcessor _processorService;

        public NotificationQueueWorker(ServiceBusClient client,
                                       ILogger<NotificationQueueWorker> logger,
                                       INotificationProcessor processorService)
        {
            _logger = logger;
            _processorService = processorService;
            _processor = client.CreateProcessor("notification-queue", new ServiceBusProcessorOptions());
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor.ProcessMessageAsync += async args =>
            {
                string body = args.Message.Body.ToString();
                await _processorService.ProcessAsync(body);
                await args.CompleteMessageAsync(args.Message);
            };

            _processor.ProcessErrorAsync += args =>
            {
                _logger.LogError(args.Exception, "Error while message handling.");
                return Task.CompletedTask;
            };

            await _processor.StartProcessingAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
