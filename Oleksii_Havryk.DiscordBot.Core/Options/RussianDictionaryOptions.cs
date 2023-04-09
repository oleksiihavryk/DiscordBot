namespace Oleksii_Havryk.DiscordBot.Core.Options;

/// <summary>
///     Options of russian dictionary service.
/// </summary>
public class RussianDictionaryOptions
{
    public string Key { get; set; }

    public RussianDictionaryOptions(string key)
    {
        Key = key;
    }
    public RussianDictionaryOptions()
        : this(string.Empty)
    {
    }

}