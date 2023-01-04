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

    public BotController(Bot bot)
    {
        _bot = bot;
    }

    //endpoints
    /* 1. Run
     * 2. Stop
     * 3. Get logger messages */
    [Route("[action]")]
    public async Task<ViewResult> Run()
    {
        await _bot.StartAsync();
        return View();
    }
    [Route("[action]")]
    public async Task<ViewResult> Stop()
    {
        await _bot.StopAsync();
        return View();
    }
    public ViewResult LoggerMessages()
    {
        throw new NotImplementedException();
    }
}