using Discord;

namespace Oleksii_Havryk.DiscordBot.Core;
/// <summary>
///     Containment logger LogMessage.
/// </summary>
public class ContainmentLoggerMessage 
{
    public LogMessage LogMessage { get; set; }
    public DateTime AddTime { get; set; }
    public bool IsRead { get; set; } = false;

    public ContainmentLoggerMessage(
        LogMessage logMessage, 
        DateTime addTime)
    {
        LogMessage = logMessage;
        AddTime = addTime;
    }

    public static IComparer<ContainmentLoggerMessage> CompareByTime => 
        ContainmentLoggerMessageByTimeComparer.Default;

    public class ContainmentLoggerMessageByTimeComparer : IComparer<ContainmentLoggerMessage>
    {
        public int Compare(ContainmentLoggerMessage? x, ContainmentLoggerMessage? y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null && y != null)
                return 1;
            if (x != null && y == null)
                return -1;

            return x.AddTime.CompareTo(y.AddTime);
        }

        private ContainmentLoggerMessageByTimeComparer()
        {
        }

        public static IComparer<ContainmentLoggerMessage> Default => new ContainmentLoggerMessageByTimeComparer();
    }
}