namespace Oleksii_Havryk.DiscordBot.Core.Options;
/// <summary>
///     Bot token options.
/// </summary>
public class BotOptions
{
    public string TokenValue { get; set; }

    public BotOptions()
        : this(tokenValue: string.Empty)
    {
    }
    public BotOptions(string tokenValue)
    {
        TokenValue = tokenValue;
    }
}