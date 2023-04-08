using Discord;

namespace Oleksii_Havryk.DiscordBot.Domain;
/// <summary>
///     Content information about logger message.
/// </summary>
public class LoggerMessage 
{
    public static IComparer<LoggerMessage> CompareByTime =>
        ContainmentLoggerMessageByTimeComparer.Default;

    public string Source { get; set; }
    public string Message { get; set; }
    public DateTime AddTime { get; set; }
    public bool IsRead { get; set; } = false;

    public LoggerMessage(
        DateTime addTime, 
        string source, 
        string message)
    {
        Source = source;
        Message = message;
        AddTime = addTime;
    }

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