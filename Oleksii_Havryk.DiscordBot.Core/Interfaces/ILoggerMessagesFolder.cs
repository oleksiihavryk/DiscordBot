using Discord;

namespace Oleksii_Havryk.DiscordBot.Core.Interfaces;

/// <summary>
///     Logger messages folder.
/// </summary>
public interface ILoggerMessagesFolder
{
    IEnumerable<LoggerMessage> OtherMessages { get; }
    IEnumerable<LoggerMessage> LatestMessages { get; }

    Task AddToLatestAsync(LogMessage message);
    Task UpdateMessagesAsync();
}