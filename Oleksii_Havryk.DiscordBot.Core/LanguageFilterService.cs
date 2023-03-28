using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Oleksii_Havryk.DiscordBot.Core.Extensions;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using Oleksii_Havryk.DiscordBot.Core.Options;

namespace Oleksii_Havryk.DiscordBot.Core;
/// <summary>
///     Language filter bot service.
/// </summary>
public class LanguageFilterService : ILanguageFilterService
{
    private readonly DiscordSocketClient _client;
    private readonly ILanguageFilter _languageFilter;
    private readonly ExceptionalUsersOptions _exceptionalUsers;
    private readonly string[] _possibleTextAnswers = new []
    {
        "Dolboeb -> {0}",
        "Loh -> {0}",
        "Цієї людини єбали мати -> {0}",
        "Що, не пишеться, {0}, так?",
        "фууу {0} обісраний "
    };

    public LanguageFilterService(
        DiscordSocketClient client, 
        ILanguageFilter languageFilter,
        IOptions<ExceptionalUsersOptions> options)
    {
        _client = client;
        _languageFilter = languageFilter;
        _exceptionalUsers = options.Value;
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
    private string GetTextResponseOnInappropriateWord(IGuildUser user)
    {
        if (_exceptionalUsers.Identificators.Contains(user.Id.ToString()))
        {
            return "Сорі, помиливися, більше не повторится)))";
        }

        return string.Format(
            _possibleTextAnswers.GetRandom(), 
            MentionUtils.MentionUser(user.Id));
    }
    private async Task WordIsInappropriateAsync(SocketMessage socketMessage)
    {
        await socketMessage.DeleteAsync();
        if (socketMessage.Author is IGuildUser user)
        {
            var textResponse = GetTextResponseOnInappropriateWord(user);
            await socketMessage.Channel.SendMessageAsync(textResponse);
            if (user.GuildPermissions.Administrator == false)
                await user.SetTimeOutAsync(TimeSpan.FromSeconds(10));
        }
    }
}