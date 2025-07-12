using Azure.Messaging.ServiceBus;
using NotificationService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure
{
    public class AzureServiceBus : IMessageBus
    {

        private readonly ServiceBusClient _client;

        public AzureServiceBus(ServiceBusClient client)
        {
            _client = client;
        }

        public async Task PublishAsync<T>(T message, string queueName)
        {
            ServiceBusSender sender = _client.CreateSender(queueName);
            string json = JsonSerializer.Serialize(message);
            ServiceBusMessage busMessage = new ServiceBusMessage(json);
            await sender.SendMessageAsync(busMessage);
        }
    }
}
