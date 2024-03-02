using System.Text;
using System.Text.Json;
using MessageServer.Application.Abstractions;
using RabbitMQ.Client;

namespace MessageServer.Application.Implementations;

public class RabbitMqService : IRabbitMqService
{
    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory(){HostName = "localhost"};
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: "OwnerQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments:null);

        var body = Encoding.UTF8.GetBytes(message);
        
        channel.BasicPublish(exchange: string.Empty,
            routingKey:"OwnerQueue",
            basicProperties:null,
            body: body);
    }
}