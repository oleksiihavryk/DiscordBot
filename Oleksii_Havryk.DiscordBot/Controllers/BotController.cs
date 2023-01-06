using Microsoft.AspNetCore.Mvc;
using Oleksii_Havryk.DiscordBot.Core;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using Oleksii_Havryk.DiscordBot.ViewModels;

namespace Oleksii_Havryk.DiscordBot.Controllers;
/// <summary>
///     Bot controller.
/// </summary>
[Controller]
public class BotController : Controller
{
    private readonly Bot _bot;
    private readonly ILoggerMessagesFolder _loggerMessagesFolder;

    public BotController(
        Bot bot,
        ILoggerMessagesFolder loggerMessagesFolder)
    {
        _bot = bot;
        _loggerMessagesFolder = loggerMessagesFolder;
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
    [Route("[action]")]
    public async Task<ViewResult> Messages()
    {
        var latestMessages = _loggerMessagesFolder
            .LatestMessages
            .ToArray();
        var otherMessages = _loggerMessagesFolder
            .OtherMessages
            .ToArray();
        await _loggerMessagesFolder.UpdateMessagesAsync();

        return View(model: new LoggerMessagesViewModel(
            otherMessages,
            latestMessages));
    }
}