namespace MessageServer.Application;

public interface IRabbitMqService
{
    void SendMessage(object obj);
    void SendMessage(string message);
}