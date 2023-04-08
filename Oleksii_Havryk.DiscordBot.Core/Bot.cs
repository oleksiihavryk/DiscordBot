using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using Oleksii_Havryk.DiscordBot.Core.Options;

namespace Oleksii_Havryk.DiscordBot.Core;
/// <summary>
///     Discord bot instance.
/// </summary>
public class Bot
{
    protected DiscordSocketClient Client { get; set; }
    protected ILanguageFilterService LanguageFilterService { get; set; }
    protected ICommandHandlerService CommandHandlerService { get; set; }
    protected IBotLoggingService BotLoggingService { get; set; }
    protected IOptions<BotOptions> TokenOptions { get; set; }

    private IDiscordBotService[] BaseServices => new IDiscordBotService[]
    {
        LanguageFilterService,
        CommandHandlerService,
        BotLoggingService,
    };

    public bool IsWork { get; protected set; } = false;

    public Bot(
        DiscordSocketClient client,  
        ICommandHandlerService commandHandlerService, 
        ILanguageFilterService languageFilterService,
        IBotLoggingService botLoggingService,
        IOptions<BotOptions> tokenOptions)
    {
        Client = client;
        CommandHandlerService = commandHandlerService;
        LanguageFilterService = languageFilterService;
        BotLoggingService = botLoggingService;
        TokenOptions = tokenOptions;
    }

    public virtual async Task StartAsync()
        => await StartAsync(BaseServices);
    public virtual async Task StopAsync()
        => await StopAsync(BaseServices);

    protected virtual async Task StartAsync(params IDiscordBotService[] services)
    {
        if (IsWork == false)
        {
            await Task.WhenAll(services.Select(s => s.BeginHandleAsync()));

            await Client.LoginAsync(
                tokenType: TokenType.Bot,
                token: TokenOptions.Value.TokenValue);
            await Client.StartAsync();

            IsWork = true;
        }
    }
    protected virtual async Task StopAsync(params IDiscordBotService[] services)
    {
        if (IsWork)
        {
            await Client.StopAsync();
            await Client.LogoutAsync();
            await Task.WhenAll(services.Select(s => s.EndHandleAsync()));

            IsWork = false;
        }
    }
}