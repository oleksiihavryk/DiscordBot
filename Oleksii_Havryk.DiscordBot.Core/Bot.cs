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
    private readonly DiscordSocketClient _client;
    private readonly ILanguageFilterService _languageFilterService;
    private readonly ICommandHandlerService _commandHandlerService;
    private readonly IBotLoggingService _botLoggingService;
    private readonly IOptions<BotOptions> _tokenOptions;
    private bool _isWork = false;

    private IDiscordBotService[] BaseServices => new IDiscordBotService[]
    {
        _languageFilterService,
        _commandHandlerService,
        _botLoggingService,
    };

    public bool IsWork => _isWork;

    public Bot(
        DiscordSocketClient client,  
        ICommandHandlerService commandHandlerService, 
        ILanguageFilterService languageFilterService,
        IBotLoggingService botLoggingService,
        IOptions<BotOptions> tokenOptions)
    {
        _client = client;
        _commandHandlerService = commandHandlerService;
        _languageFilterService = languageFilterService;
        _botLoggingService = botLoggingService;
        _tokenOptions = tokenOptions;
    }

    public async Task StartAsync()
        => await StartAsync(BaseServices);
    public async Task StopAsync()
        => await StopAsync(BaseServices);

    private async Task StartAsync(params IDiscordBotService[] services)
    {
        if (_isWork == false)
        {
            await Task.WhenAll(services.Select(s => s.BeginHandleAsync()));

            await _client.LoginAsync(
                tokenType: TokenType.Bot,
                token: _tokenOptions.Value.TokenValue);
            await _client.StartAsync();

            _isWork = true;
        }
    }
    private async Task StopAsync(params IDiscordBotService[] services)
    {
        if (_isWork)
        {
            await _client.StopAsync();
            await _client.LogoutAsync();
            await Task.WhenAll(services.Select(s => s.EndHandleAsync()));

            _isWork = false;
        }
    }
}