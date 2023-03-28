namespace Oleksii_Havryk.DiscordBot.Core.Options;
/// <summary>
///     Exceptional users for system.
///     Using for defining a response on language filter.
/// </summary>
public class ExceptionalUsersOptions
{
    public string[] Identificators { get; set; }

    public ExceptionalUsersOptions(string[] identificators)
    {
        Identificators = identificators;
    }
    public ExceptionalUsersOptions()
        : this(Array.Empty<string>())
    {
    }
}