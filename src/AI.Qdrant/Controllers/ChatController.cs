using Microsoft.AspNetCore.Mvc;

namespace AI.Qdrant.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly ILogger<ChatController> _logger;

    public ChatController(ILogger<ChatController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Chat")]
    public async Task<string> Post()
    {
        return "OK";
    }
}