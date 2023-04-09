using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core.LanguageFilterServices;
/// <summary>
///     Language filter default implementation.
/// </summary>
public class UkrainianRussianLanguageFilter : ILanguageFilter
{
    private readonly IUkrainianDictionaryWebService _ukrainianDictionaryWebService;
    private readonly IRussianDictionaryWebService _russianDictionaryWebService;

    public UkrainianRussianLanguageFilter(
        IUkrainianDictionaryWebService ukrainianDictionaryWebService, 
        IRussianDictionaryWebService russianDictionaryWebService)
    {
        _ukrainianDictionaryWebService = ukrainianDictionaryWebService;
        _russianDictionaryWebService = russianDictionaryWebService;
    }

    public virtual async Task<bool> FilterWordsAsync(string[] words)
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
    public virtual async Task<bool> FilterWordAsync(string word) =>
        await FilterWordAsync(word, CancellationToken.None);

    protected virtual async Task<bool> FilterWordAsync(string word, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(word.Trim()))
            return true;

        var findWordInRussianDictionary = await _russianDictionaryWebService
            .FindWordAsync(word, token);

        if (findWordInRussianDictionary)
        {
            var findWordInUkrainianDictionary = await _ukrainianDictionaryWebService
                .FindWordAsync(word, token);

            return findWordInUkrainianDictionary;
        }

        return true;
    }

    public void Dispose()
    {
        _ukrainianDictionaryWebService.Dispose();
        _russianDictionaryWebService.Dispose();
    }
}