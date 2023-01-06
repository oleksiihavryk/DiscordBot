using System.Runtime.CompilerServices;
using Oleksii_Havryk.DiscordBot.Core;

namespace Oleksii_Havryk.DiscordBot.ViewModels;
/// <summary>
///     Logger messages view model.
/// </summary>
public class LoggerMessagesViewModel
{
    public IEnumerable<Core.LoggerMessage> OtherMessages { get; set; }
    public IEnumerable<Core.LoggerMessage> LatestMessages { get; set; }

    public LoggerMessagesViewModel(
        IEnumerable<Core.LoggerMessage> otherMessages, 
        IEnumerable<Core.LoggerMessage> latestMessages)
    {
        OtherMessages = otherMessages;
        LatestMessages = latestMessages;
    }
}