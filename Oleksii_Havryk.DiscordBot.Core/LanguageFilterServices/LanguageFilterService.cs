using System.Text.RegularExpressions;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Oleksii_Havryk.DiscordBot.Core.Extensions;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using Oleksii_Havryk.DiscordBot.Core.Options;

namespace Oleksii_Havryk.DiscordBot.Core.LanguageFilterServices;
/// <summary>
///     Language filter bot service.
/// </summary>
public class LanguageFilterService : ILanguageFilterService
{
    protected DiscordSocketClient Client { get; set; }
    protected ILanguageFilter LanguageFilter { get; set; }
    protected ExceptionalUsersOptions ExceptionalUsers { get; set; }
    protected IBotLoggingService LoggingService { get; set; }
    protected string[] PossibleTextAnswers { get; set; } = new[]
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
        IOptions<ExceptionalUsersOptions> options, IBotLoggingService loggingService)
    {
        Client = client;
        LanguageFilter = languageFilter;
        LoggingService = loggingService;
        ExceptionalUsers = options.Value;
    }

    public async Task BeginHandleAsync()
    {
        Client.MessageReceived += FilterDiscordMessageAsync;

        await Task.CompletedTask;
    }
    public async Task EndHandleAsync()
    {
        Client.MessageReceived += FilterDiscordMessageAsync;

        await Task.CompletedTask;
    }

    public virtual async Task FilterDiscordMessageAsync(IMessage arg)
    {
        var content = arg.Content;

        if (!arg.Author.IsBot && !arg.Author.IsWebhook && !string.IsNullOrWhiteSpace(content))
        {
            var words = ExtractWords(content);
            var isAllowed = words.Length != 1
                ? await LanguageFilter.FilterWordsAsync(words)
                : await LanguageFilter.FilterWordAsync(words[0]);

            if (!isAllowed)
            {
                await WordIsInappropriateAsync(arg);
            }
        }
    }

    protected virtual string[] ExtractWords(string content)
        => content.Trim()
            .ToLower()
            .Split(' ')
            .Where(w => string.IsNullOrWhiteSpace(w) == false)
            .Select(w => new string(w.Trim().Where(s => char.IsLetter(s)).ToArray()))
            .ToArray();
    protected virtual string GetTextResponseOnInappropriateWord(ulong userId)
    {
        if (ExceptionalUsers.Identificators.Contains(userId.ToString()))
            return "Сорі, помиливися, більше не повторится)))";

        return string.Format(
            PossibleTextAnswers.GetRandom(),
            MentionUtils.MentionUser(userId));
    }
    protected virtual async Task WordIsInappropriateAsync(IMessage socketMessage)
    {
        await socketMessage.DeleteAsync();
        if (socketMessage.Author is IGuildUser user)
        {
            var textResponse = GetTextResponseOnInappropriateWord(user.Id);
            await socketMessage.Channel.SendMessageAsync(textResponse);
            if (user.GuildPermissions.Administrator == false)
                await user.SetTimeOutAsync(TimeSpan.FromSeconds(10));
        }
        await LoggingService.LogBotMessageAsync(new LogMessage(
            LogSeverity.Info,
            source: nameof(LanguageFilterService),
            message: $"User with nickname {socketMessage.Author.Username} " +
                     $"has been banned for message \"{socketMessage.Content}\""));
    }
}