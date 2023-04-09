using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core.LanguageFilterServices;

/// <summary>
///     Abstract base class for dictionary web services.
/// </summary>
public abstract class DictionaryWebService : IDictionaryWebService
{
    protected HttpClient HttpClient { get; set; }

    protected DictionaryWebService(
        HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public virtual async Task<bool> FindWordAsync(string word)
        => await FindWordAsync(word, CancellationToken.None);
    public virtual async Task<bool> FindWordAsync(string word, CancellationToken token)
    {
        //create http request message
        var checkOnUkrainian = CreateRequest(word);

        //send request 
        var russianResponse = await HttpClient.SendAsync(checkOnUkrainian, token);

        //handle response from service
        return await HandleResponseAsync(russianResponse, token);
    }

    protected abstract HttpRequestMessage CreateRequest(string word);
    protected abstract Task<bool> HandleResponseAsync(
        HttpResponseMessage response,
        CancellationToken token);

    public virtual void Dispose()
    {
        HttpClient.Dispose();
    }
}