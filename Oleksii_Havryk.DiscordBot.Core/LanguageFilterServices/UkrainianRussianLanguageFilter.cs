using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core.LanguageFilterServices;
/// <summary>
///     Language filter default implementation.
/// </summary>
public class UkrainianRussianLanguageFilter : ILanguageFilter
{
    private const string Lang = "ru-ru";

    private readonly HttpClient _httpClient;

    public string Key { get; set; }

    public UkrainianRussianLanguageFilter(
        IHttpClientFactory httpClientFactory,
        string key)
    {
        _httpClient = httpClientFactory.CreateClient();
        Key = key;
    }

    public async Task<bool> FilterWordsAsync(string[] words)
    {
        bool result = true;
        var source = new CancellationTokenSource();

        var tasks = new List<Task>();
        foreach (var word in words)
        {
            var task = FilterWordAsync(word, source.Token);
            var newTask = task.ContinueWith(r =>
            {
                if (r.Result == false)
                {
                    result = r.Result;
                    source.Cancel();
                }
            });
            tasks.Add(newTask);
        }
        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            if (ex.InnerException is not OperationCanceledException)
                throw;
            //ignore
        }

        return result;
    }
    public async Task<bool> FilterWordAsync(string word) =>
        await FilterWordAsync(word, CancellationToken.None);

    private async Task<bool> FilterWordAsync(string word, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(word))
            return true;

        var russianKeyWord = "\"def\":[]";
        var checkOnRussian = new HttpRequestMessage(
            method: HttpMethod.Get,
            requestUri:
            $"https://dictionary.yandex.net/api/v1/dicservice.json/lookup?" +
            $"key={Key}" +
            $"&lang={Lang}" +
            $"&text={word}");
        var russianResponse = await _httpClient.SendAsync(checkOnRussian, token);
        var russianResponseContent = await russianResponse.Content
            .ReadAsStringAsync(token);

        if (russianResponseContent.IndexOf(
                value: russianKeyWord,
                comparisonType: StringComparison.Ordinal) == -1)
        {
            var ukrainianKeyWord = $"Слова «{word}» не знайдено.";
            var checkUkrainian = new HttpRequestMessage(
                method: HttpMethod.Get,
                requestUri: $"http://sum.in.ua/?swrd={word}");
            var ukrainianResponse = await _httpClient.SendAsync(checkUkrainian, token);
            var ukrainianResponseContent = await ukrainianResponse.Content
                .ReadAsStringAsync(token);

            if (ukrainianResponseContent.IndexOf(
                    value: ukrainianKeyWord,
                    comparisonType: StringComparison.Ordinal) != -1)
            {
                return false;
            }
        }

        return true;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}