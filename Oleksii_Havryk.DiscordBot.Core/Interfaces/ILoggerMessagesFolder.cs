using Discord;
using Oleksii_Havryk.DiscordBot.Domain;

namespace Oleksii_Havryk.DiscordBot.Core.Interfaces;

/// <summary>
///     Logger messages folder.
/// </summary>
public interface ILoggerMessagesFolder
{
    IEnumerable<LoggerMessage> OtherMessages { get; }
    IEnumerable<LoggerMessage> LatestMessages { get; }

    Task AddToLatestAsync(string source, string message);
    Task UpdateMessagesAsync();
}