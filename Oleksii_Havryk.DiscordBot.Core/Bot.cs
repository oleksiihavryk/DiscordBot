using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using Oleksii_Havryk.DiscordBot.Core.Options;

namespace Oleksii_Havryk.DiscordBot.Core;
/// <summary>
///     Discord bot instance.
/// </summary>
public sealed class Bot
{
    private readonly DiscordSocketClient _client;
    private readonly ILanguageFilterService _languageFilterService;
    private readonly ICommandHandlerService _commandHandlerService;
    private readonly IBotLoggingService _botLoggingService;
    private readonly IOptions<BotOptions> _tokenOptions;

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
    {
        await _commandHandlerService.BeginHandleAsync();
        await _languageFilterService.BeginHandleAsync();
        await _botLoggingService.BeginHandleAsync();
        await _client.LoginAsync(
            tokenType: TokenType.Bot,
            token: _tokenOptions.Value.TokenValue);
        await _client.StartAsync();
    }
    public async Task StopAsync()
    {
        await _client.StopAsync();
        await _client.LogoutAsync();
        await _commandHandlerService.EndHandleAsync();
        await _languageFilterService.EndHandleAsync();
        await _botLoggingService.EndHandleAsync();
    }
}