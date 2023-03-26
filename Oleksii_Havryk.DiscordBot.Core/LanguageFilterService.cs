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

    public async Task FilterDiscordMessage(SocketMessage arg)
    {
        if (!arg.Author.IsBot && !arg.Author.IsWebhook && !string.IsNullOrWhiteSpace(arg.Content))
        {
            var words = ExtractWordsAsync(arg);
            var isAllowed = words.Length != 1
                ? await _languageFilter.FilterWordsAsync(words)
                : await _languageFilter.FilterWordAsync(words[0]);

            if (!isAllowed)
            {
                await WordIsInappropriateAsync(arg);
            }
        }
    }

    private string[] ExtractWordsAsync(SocketMessage arg)
        => arg.Content.Trim().ToLower().Split(' ');
    private async Task WordIsInappropriateAsync(SocketMessage socketMessage)
    {
        await socketMessage.DeleteAsync();
        await socketMessage.Channel.SendMessageAsync("!");
        if (socketMessage.Author is IGuildUser user)
        {
            await user.SetTimeOutAsync(TimeSpan.FromSeconds(10));
        }
    }
}