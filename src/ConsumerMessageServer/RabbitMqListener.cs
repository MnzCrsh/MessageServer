using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerMessageServer;

public class RabbitMqListener : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqListener()
    {
        var factory = new ConnectionFactory() {HostName = "localhost"};
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "OwnerQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            //TODO: Add logging and message handler
            //await HandleOwnerMessagesAsync();
            
            _channel.BasicAck(ea.DeliveryTag, false);
        };
        _channel.BasicConsume(queue:"OwnerQueue", consumer: consumer);
        
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}