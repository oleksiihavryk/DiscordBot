using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core.LanguageFilterServices;
/// <summary>
///     Web service of ukrainian dictionary.
/// </summary>
public class UkrainianDictionaryWebService : DictionaryWebService, IUkrainianDictionaryWebService
{
    private const string KeyWord = "відсутнє в базі! Будь ласка, перевірте правильність написання.";

    public UkrainianDictionaryWebService(
        IHttpClientFactory httpClientFactory)
        : base(httpClientFactory.CreateClient())
    {
    }

    protected override async Task<HttpRequestMessage> CreateRequestAsync(string word)
        => await Task.FromResult(new HttpRequestMessage(
            method: HttpMethod.Get,
            requestUri: $"https://slovnyk.ua/?swrd={word}"));
    protected override async Task<bool> HandleResponseAsync(
        HttpResponseMessage response,
        CancellationToken token)
    {
        var ukrainianResponseContent = await response.Content
            .ReadAsStringAsync(token);

        return ukrainianResponseContent.IndexOf(
            value: KeyWord,
            comparisonType: StringComparison.Ordinal) == -1;
    }
}