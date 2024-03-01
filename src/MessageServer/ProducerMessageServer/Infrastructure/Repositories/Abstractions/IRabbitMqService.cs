namespace MessageServer.Infrastructure.Repositories.Abstractions;

public interface IRabbitMqService
{
    void SendMessage(object obj);
    void SendMessage(string message);
}