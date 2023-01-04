using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core;
/// <summary>
///     Internal service in bot configuration for configure logging mechanism.
/// </summary>
public class BotLoggingService : IBotLoggingService
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<BotLoggingService> _logger;
    private readonly ILoggerMessagesFolder _loggerMessagesFolder;

    public BotLoggingService(
        DiscordSocketClient client,
        ILogger<BotLoggingService> logger, 
        ILoggerMessagesFolder loggerMessagesFolder)
    {
        _client = client;
        _logger = logger;
        _loggerMessagesFolder = loggerMessagesFolder;
    }

    public async Task BeginHandleAsync()
    {
        _client.Log += HandleAsync;

        await Task.CompletedTask;
    }
    public async Task EndHandleAsync()
    {
        _client.Log -= HandleAsync;

        await Task.CompletedTask;
    }

    private async Task HandleAsync(LogMessage message)
    {
        if (message.Exception is not null)
        {
            var ex = message.Exception;
            switch (ex)
            {
                case CommandException commandException:
                {
                    _logger.LogError(message: $"[{message.Source}] {message.Message}\n" +
                                        "Command context:\n" +
                                        $"User: {commandException.Context.User.Username},\n" +
                                        $"Command: {commandException.Command.Name},\n" +
                                        $"Exception LogMessage: {commandException.Message},\n" +
                                        $"Exception stack trace: {commandException.StackTrace}");
                    break;
                }
                default:
                {
                    _logger.LogError(
                        message: $"[{message.Source}] {message.Message}\n", 
                        exception: message.Exception);
                    break;
                }
            }
        }
        else
        {
            _logger.Log(
                    logLevel: message.Severity switch
                    {
                        LogSeverity.Critical => LogLevel.Critical,
                        LogSeverity.Debug => LogLevel.Debug,
                        LogSeverity.Error => LogLevel.Error,
                        LogSeverity.Info => LogLevel.Information,
                        LogSeverity.Verbose => LogLevel.Trace,
                        LogSeverity.Warning => LogLevel.Warning,
                        _ => LogLevel.None
                    }, 
                message: $"[{message.Source}] {message.Message}");
        }

        await _loggerMessagesFolder.AddToLatestAsync(message);
    }
}