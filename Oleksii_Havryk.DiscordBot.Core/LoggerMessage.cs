using Discord;

namespace Oleksii_Havryk.DiscordBot.Core;
/// <summary>
///     Containment logger LogMessage.
/// </summary>
public class LoggerMessage 
{
    public LogMessage LogMessage { get; set; }
    public DateTime AddTime { get; set; }
    public bool IsRead { get; set; } = false;

    public LoggerMessage(
        LogMessage logMessage, 
        DateTime addTime)
    {
        LogMessage = logMessage;
        AddTime = addTime;
    }

    public static IComparer<LoggerMessage> CompareByTime => 
        ContainmentLoggerMessageByTimeComparer.Default;

    public class ContainmentLoggerMessageByTimeComparer : IComparer<LoggerMessage>
    {
        public int Compare(LoggerMessage? x, LoggerMessage? y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null && y != null)
                return -1;
            if (x != null && y == null)
                return 1;

            return -(x.AddTime.CompareTo(y.AddTime));
        }

        private ContainmentLoggerMessageByTimeComparer()
        {
        }

        public static IComparer<LoggerMessage> Default => new ContainmentLoggerMessageByTimeComparer();
    }
}