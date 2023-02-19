using Microsoft.AspNetCore.Mvc;
using Oleksii_Havryk.DiscordBot.Core;

namespace Oleksii_Havryk.DiscordBot.Controllers;
/// <summary>
///     Bot controller.
/// </summary>
[Controller]
[Route("[controller]")]
public class BotController : Controller
{
    private readonly Bot _bot;

    public BotController(
        Bot bot)
    {
        _bot = bot;
    }

    //endpoints
    /* 1. Run
     * 2. Stop */
    [HttpPut("[action]")]
    public async Task<IActionResult> Run()
    {
        await _bot.StartAsync();
        return Ok();
    }
    [HttpPut("[action]")]
    public async Task<IActionResult> Stop()
    {
        await _bot.StopAsync();
        return Ok();
    }
}