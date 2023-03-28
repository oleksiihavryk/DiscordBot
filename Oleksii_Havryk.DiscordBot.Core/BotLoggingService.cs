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
        _client.Log += LogBotMessage;

        await Task.CompletedTask;
    }
    public async Task EndHandleAsync()
    {
        _client.Log -= LogBotMessage;

        await Task.CompletedTask;
    }
    
    public virtual async Task LogBotMessage(LogMessage message)
    {
        LogMessage newMessage = FormatMessage(message);

        if (newMessage.Exception is not null)
        {
            _logger.LogError(
                message: $"[{newMessage.Source}] {newMessage.Message}",
                exception: newMessage.Exception);
        }
        else
        {
            _logger.Log(
                    logLevel: newMessage.Severity switch
                    {
                        LogSeverity.Critical => LogLevel.Critical,
                        LogSeverity.Debug => LogLevel.Debug,
                        LogSeverity.Error => LogLevel.Error,
                        LogSeverity.Info => LogLevel.Information,
                        LogSeverity.Verbose => LogLevel.Trace,
                        LogSeverity.Warning => LogLevel.Warning,
                        _ => LogLevel.None
                    }, 
                message: $"[{newMessage.Source}] {newMessage.Message}");
        }

        await _loggerMessagesFolder.AddToLatestAsync(newMessage);
    }

    private LogMessage FormatMessage(LogMessage message)
    {
        return new LogMessage(
            severity: message.Severity,
            source: message.Source,
            message: message.Exception != null ? 
                $"{message.Message} ({message.Exception.Message})" : 
                message.Message,
            exception: message.Exception);
    }
}