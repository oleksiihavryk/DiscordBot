using Discord;

namespace Oleksii_Havryk.DiscordBot.Core.Interfaces;

/// <summary>
///     Logging bot service abstraction.
/// </summary>
public interface IBotLoggingService : IDiscordBotService
{
    Task LogBotMessageAsync(LogMessage message);
}