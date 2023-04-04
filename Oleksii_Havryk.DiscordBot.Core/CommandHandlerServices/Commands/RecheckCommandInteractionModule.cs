using System.Net.Sockets;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core.CommandHandlerServices.Commands;
/// <summary>
///     Basic interaction module but with recheck command.
/// </summary>
public class RecheckCommandInteractionModule : BasicInteractionModule
{
    private readonly ILanguageFilterService _languageFilterService;

    public RecheckCommandInteractionModule(ILanguageFilterService languageFilterService)
    {
        _languageFilterService = languageFilterService;
    }

    [SlashCommand(
        name: "recheck",
        description: "Команда для повторної перевірки повідомлень на наявність російських слів " +
                     "які з яких то причин не були перевірені автоматично. " +
                     "Переповіряє певну кількість (не більше 100) повідомлень " +
                     "і тихо видаляє якщо там є російські слова.",
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
                if (m.Content.Length > 0 && message is SocketMessage socketMessage)
                    asyncCheckingMessage.Add(
                        _languageFilterService.FilterDiscordMessage(socketMessage));
            }
        }

        await Task.WhenAll(asyncCheckingMessage);
    }
}