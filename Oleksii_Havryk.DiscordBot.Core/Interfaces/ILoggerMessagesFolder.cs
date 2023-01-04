using Discord;

namespace Oleksii_Havryk.DiscordBot.Core.Interfaces;

/// <summary>
///     Logger messages folder.
/// </summary>
public interface ILoggerMessagesFolder
{
    IEnumerable<ContainmentLoggerMessage> OtherMessages { get; }
    IEnumerable<ContainmentLoggerMessage> LatestMessages { get; }

    Task AddToLatestAsync(LogMessage message);
    Task UpdateMessagesAsync();
}