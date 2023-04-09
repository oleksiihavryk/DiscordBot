using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core.LoggingServices;
/// <summary>
///     Internal service in bot configuration for configure logging mechanism.
/// </summary>
public class BotLoggingService : IBotLoggingService
{
    private readonly DiscordSocketClient _client;
    private readonly ILoggerMessagesFolder _loggerMessagesFolder;
    private readonly ILogger<BotLoggingService> _logger;

    public BotLoggingService(
        DiscordSocketClient client,
        ILogger<BotLoggingService> logger,
        ILoggerMessagesFolder loggerMessagesFolder)
    {
        _client = client;
        _logger = logger;
        _loggerMessagesFolder = loggerMessagesFolder;
    }

    public virtual async Task BeginHandleAsync()
    {
        _client.Log += LogBotMessageAsync;

        await Task.CompletedTask;
    }
    public virtual async Task EndHandleAsync()
    {
        _client.Log -= LogBotMessageAsync;

        await Task.CompletedTask;
    }

    public virtual async Task LogBotMessageAsync(LogMessage message)
    {
        var formattedMessage = GetFormatStringMessage(message);

        if (message.Exception is not null)
            LogCaughtException(message.Source, formattedMessage, message.Exception);
        else LogBySeverity(message.Severity, message.Source, formattedMessage);

        await _loggerMessagesFolder.AddToLatestAsync(message.Source, formattedMessage);
    }

    protected virtual string GetFormatStringMessage(LogMessage message)
        => message.Exception != null ? 
            $"{message.Message} ({message.Exception.Message})" :
            message.Message;
    protected virtual void LogCaughtException(string source, string message, Exception exception)
        => _logger.LogError(
            message: $"[{source}] {message}",
            exception: exception);
    protected virtual void LogBySeverity(LogSeverity severity, string source, string message)
        => _logger.Log(
            logLevel: severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Debug => LogLevel.Debug,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Trace,
                LogSeverity.Warning => LogLevel.Warning,
                _ => LogLevel.None
            },
            message: $"[{source}] {message}");
}