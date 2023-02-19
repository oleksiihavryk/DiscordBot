using Discord;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using LoggerMessage = Oleksii_Havryk.DiscordBot.Domain.LoggerMessage;

namespace Oleksii_Havryk.DiscordBot.Core;

/// <summary>
///     Logger message folder interface implementation.
/// </summary>
public class LoggerMessagesFolder : ILoggerMessagesFolder
{
    private readonly List<LoggerMessage> _messages = 
        new List<LoggerMessage>();

    public IEnumerable<LoggerMessage> OtherMessages => 
        _messages.Where(
            m => m.IsRead)
            .ToList();
    public IEnumerable<LoggerMessage> LatestMessages => 
        _messages.Where(
            m => !m.IsRead)
            .ToList();

    public TimeSpan ExpireTime { get; set; } = TimeSpan.FromDays(1);

    public async Task AddToLatestAsync(LogMessage message)
    {
        _messages.Add(item: new LoggerMessage(
            message,
            addTime: DateTime.Now));

        await Task.CompletedTask;
    }
    public async Task UpdateMessagesAsync()
    {
        var latest = LatestMessages.ToArray();
        var all = _messages.ToArray();
        var now = DateTime.Now;

        if (latest.Length != 0)
            for (int i = latest.Length; i-- > 0;) latest[i].IsRead = true;

        if (all.Length != 0)
        {
            for (int i = all.Length; i-- > 0;)
            {
                var message = all[i];
                if (now - message.AddTime >= ExpireTime)
                    _messages.Remove(message);
            }
        }

        await Task.CompletedTask;
    }
}