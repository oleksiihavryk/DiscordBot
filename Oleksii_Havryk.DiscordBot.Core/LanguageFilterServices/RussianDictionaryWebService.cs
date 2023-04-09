using Microsoft.Extensions.Options;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using Oleksii_Havryk.DiscordBot.Core.Options;
using System.Net.Http;

namespace Oleksii_Havryk.DiscordBot.Core.LanguageFilterServices;

/// <summary>
///     Web service of russian dictionary.
/// </summary>
public class RussianDictionaryWebService : DictionaryWebService, IRussianDictionaryWebService
{
    private const string Lang = "ru-ru";

    public string Key { get; set; }

    public RussianDictionaryWebService(
        IHttpClientFactory httpClientFactory, 
        IOptions<RussianDictionaryOptions> options)
        : base(httpClientFactory.CreateClient())
    {
        Key = options.Value.Key;
    }

    protected override HttpRequestMessage CreateRequest(string word)
        => new HttpRequestMessage(
            method: HttpMethod.Get,
            requestUri: "https://dictionary.yandex.net/api/v1/dicservice.json/lookup?" +
                        $"key={Key}" +
                        $"&lang={Lang}" +
                        $"&text={word}");
    protected override async Task<bool> HandleResponseAsync(
        HttpResponseMessage response,
        CancellationToken token)
    {
        var russianResponseContent = await response.Content
            .ReadAsStringAsync(token);

        return russianResponseContent.IndexOf(
            value: "\"def\":[]",
            comparisonType: StringComparison.Ordinal) == -1;
    } 
}