using Discord;
using Discord.WebSocket;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core;
/// <summary>
///     Language filter bot service.
/// </summary>
public class LanguageFilterService : ILanguageFilterService
{
    private readonly DiscordSocketClient _client;
    private readonly ILanguageFilter _languageFilter;

    public LanguageFilterService(
        DiscordSocketClient client, 
        ILanguageFilter languageFilter)
    {
        _client = client;
        _languageFilter = languageFilter;
    }

    public async Task BeginHandleAsync()
    {
        _client.MessageReceived += FilterDiscordMessage;

        await Task.CompletedTask;
    }
    public async Task EndHandleAsync()
    {
        _client.MessageReceived += FilterDiscordMessage;

        await Task.CompletedTask;
    }

    public virtual async Task FilterDiscordMessage(SocketMessage arg)
    {
        if (!arg.Author.IsBot && !arg.Author.IsWebhook && !string.IsNullOrWhiteSpace(arg.Content))
        {
            var words = arg.Content.Trim().ToLower().Split(' ');
            var isAllowed = words.Length != 1
                ? await _languageFilter.FilterWordsAsync(words)
                : await _languageFilter.FilterWordAsync(words[0]);

            if (!isAllowed)
            {
                await arg.DeleteAsync();
                await arg.Channel.SendMessageAsync(
                    text: $"@everyone Альорт!!! " +
                          $"Кацап в чаті!!! " +
                          $"Ось він -> {MentionUtils.MentionUser(arg.Author.Id)}, " +
                          $"гнобіть його!");
                if ((await arg.Channel.GetUserAsync(arg.Author.Id)) is IGuildUser user)
                {
                    await user.SetTimeOutAsync(TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}