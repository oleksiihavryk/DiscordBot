using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using Oleksii_Havryk.DiscordBot.Core.Options;

namespace Oleksii_Havryk.DiscordBot.Core.LanguageFilterServices;

public class LogImprovedLanguageFilterService : LanguageFilterService
{
    private readonly IBotLoggingService _loggingService;

    public LogImprovedLanguageFilterService(
        DiscordSocketClient client,
        ILanguageFilter languageFilter,
        IOptions<ExceptionalUsersOptions> options,
        IBotLoggingService loggingService)
        : base(client, languageFilter, options)
    {
        _loggingService = loggingService;
    }

    protected override async Task WordIsInappropriateAsync(SocketMessage socketMessage)
    {
        await base.WordIsInappropriateAsync(socketMessage);
        await _loggingService.LogBotMessage(new LogMessage(
            LogSeverity.Info,
            source: nameof(LogImprovedLanguageFilterService),
            message: $"User with nickname {socketMessage.Author.Username} " +
                     $"has been banned for message \"{socketMessage.Content}\""));
    }
}