using MessageServer.Infrastructure.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MessageServer.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RabbitMqController : ControllerBase
{
    private readonly IRabbitMqService _rabbitMq;

    public RabbitMqController(IRabbitMqService rabbitMq)
    {
        _rabbitMq = rabbitMq;
    }

    [HttpGet]
    [Route("[action]/{message}")]
    public IActionResult SendMessage(string message)
    {
        _rabbitMq.SendMessage(message);
        return Ok("Message sent");
    }
}