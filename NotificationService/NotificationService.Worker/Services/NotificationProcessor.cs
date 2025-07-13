using NotificationService.Worker.Interfaces;
using NotificationService.Worker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationService.Worker.Services
{
    public class NotificationProcessor : INotificationProcessor
    {

        private readonly ILogger<NotificationProcessor> _logger;

        public NotificationProcessor(ILogger<NotificationProcessor> logger)
        {
            _logger = logger;
        }

        public Task ProcessAsync(string rawMessage)
        {
            NotificationPayload? payload = JsonSerializer.Deserialize<NotificationPayload>(rawMessage);
            _logger.LogInformation("📨 Notification: {Email} -> {Message}", payload?.Email, payload?.Message);

            //TODO: Add real email logic
            return Task.CompletedTask;
        }
    }
}
