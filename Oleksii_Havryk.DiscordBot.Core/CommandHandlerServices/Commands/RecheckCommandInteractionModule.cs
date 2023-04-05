using Discord;
using Discord.Interactions;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core.CommandHandlerServices.Commands;
/// <summary>
///     Basic interaction module but with recheck command.
/// </summary>
public class RecheckCommandInteractionModule : BasicInteractionModule
{
    private readonly ILanguageFilterService _languageFilterService;
    private readonly IBotLoggingService _loggingService;

    public RecheckCommandInteractionModule(
        ILanguageFilterService languageFilterService, 
        IBotLoggingService loggingService)
    {
        _languageFilterService = languageFilterService;
        _loggingService = loggingService;
    }

    [SlashCommand(
        name: "recheck",
        description: "Ініціює повторну перевірку повідомлень на наявність російських слів. ",
        runMode: RunMode.Async)]
    public virtual async Task Recheck(
        [MinValue(0), MaxValue(100)] int count)
    {
        var message = await Context.Channel.SendMessageAsync("Бррр.");
        var messages = Context.Channel.GetMessagesAsync(
            fromMessage: message,
            dir: Direction.Before,
            limit: count,
            mode: CacheMode.AllowDownload);

        var asyncCheckingMessage = new List<Task>();
        await foreach (var collection in messages)
        {
            foreach (var m in collection)
            {
                if (m.Content.Length > 0)
                    asyncCheckingMessage.Add(
                        _languageFilterService.FilterDiscordMessageAsync(m));
            }
        }

        await Task.WhenAll(asyncCheckingMessage);
        await _loggingService.LogBotMessageAsync(new LogMessage(
            severity: LogSeverity.Info,
            source: nameof(RecheckCommandInteractionModule),
            message: "Recheck operation was successfully ended."));
    }
}