namespace Oleksii_Havryk.DiscordBot.Core.Interfaces;

/// <summary>
///     Interface of dictionary web service.
/// </summary>
public interface IDictionaryWebService : IDisposable
{
    Task<bool> FindWordAsync(string word);
    Task<bool> FindWordAsync(string word, CancellationToken token);
}