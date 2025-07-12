# Todo App (Notification Microservice)

This is a microservice created for the todo app, this is a way for me to practice Microservices. It allows the main application to talk to the microservice, and then the microservice will use AMQP (Azure service bus), this will be caught by the background worker and send the email notification.

## Technology stack
* .NET 8 Web API
* Azure Service Bus
* Mail sender (Not yet implemented, not important for this scope)

#### Web API
To build the API routes, I created a .NET 8 Web API.
This API currently only implements one route, this is a route to send a message to the Azure Service Bus.

##### Api routes

| Methode | Route                     | Description                        | Request/Response Body                          |
|---------|---------------------------|-------------------------------------|----------------------------------------|
| POST    | `/api/notification`      | Send a message to azure service bus | `{"email": "string","message": "string"}` |
---

#### Azure Service Bus

We start out by creating a service bus queue on Azure. Upload the connection string to your secrets in appsettings.json

After that we create a service bus client that will publish to our service bus queue.

```c#
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
```

This publishes the message into the Queue
