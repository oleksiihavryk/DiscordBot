namespace Oleksii_Havryk.DiscordBot.Core.Interfaces;
/// <summary>
///     Language filter.
/// </summary>
public interface ILanguageFilter : IDisposable
{
    Task<bool> FilterWordsAsync(string[] words);
    Task<bool> FilterWordAsync(string word);
}