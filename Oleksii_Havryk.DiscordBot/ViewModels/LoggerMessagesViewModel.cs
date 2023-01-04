using System.Runtime.CompilerServices;
using Oleksii_Havryk.DiscordBot.Core;

namespace Oleksii_Havryk.DiscordBot.ViewModels;
/// <summary>
///     Logger messages view model.
/// </summary>
public class LoggerMessagesViewModel
{
    public IEnumerable<ContainmentLoggerMessage> OtherMessages { get; set; }
    public IEnumerable<ContainmentLoggerMessage> LatestMessages { get; set; }

    public LoggerMessagesViewModel(
        IEnumerable<ContainmentLoggerMessage> otherMessages, 
        IEnumerable<ContainmentLoggerMessage> latestMessages)
    {
        OtherMessages = otherMessages;
        LatestMessages = latestMessages;
    }
}