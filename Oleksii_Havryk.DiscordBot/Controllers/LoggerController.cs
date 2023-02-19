using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using Oleksii_Havryk.DiscordBot.Dto;

namespace Oleksii_Havryk.DiscordBot.Controllers;

/// <summary>
///     Logger messages controller.
/// </summary>
[ApiController]
[Route("[controller]")]
public class LoggerController : ControllerBase
{
    private readonly ILoggerMessagesFolder _loggerMessagesFolder;
    private readonly IMapper _mapper;

    public LoggerController(
        ILoggerMessagesFolder loggerMessagesFolder,
        IMapper mapper)
    {
        _loggerMessagesFolder = loggerMessagesFolder;
        _mapper = mapper;
    }

    [HttpGet] public async Task<IActionResult> AllMessages()
        => await GetMessages(LoggerMessageFilter.None);
    [HttpGet("new")] public async Task<IActionResult> NewMessages()
        => await GetMessages(LoggerMessageFilter.New);
    [HttpGet("old")] public async Task<IActionResult> OldMessages()
        => await GetMessages(LoggerMessageFilter.Old);

    [NonAction]
    private async Task<IActionResult> GetMessages(LoggerMessageFilter filter)
    {
        var messages = filter switch
        {
            LoggerMessageFilter.None => _loggerMessagesFolder.LatestMessages
                .Concat(_loggerMessagesFolder.OtherMessages),
            LoggerMessageFilter.New => _loggerMessagesFolder.LatestMessages,
            LoggerMessageFilter.Old => _loggerMessagesFolder.OtherMessages,
            _ => throw new ArgumentOutOfRangeException(
                paramName: nameof(filter),
                message: "Logger message filter is unhandled by current endpoint.")
        };
        var result = _mapper.Map<IEnumerable<LoggerMessageDto>>(messages);

        if (filter != LoggerMessageFilter.Old)
            await _loggerMessagesFolder.UpdateMessagesAsync();

        return Ok(value: result);
    }

    /// <summary>
    ///     Logger message filter.
    ///     Simplify filtering of logger messages by single method.
    /// </summary>
    private enum LoggerMessageFilter
    {
        None,
        New,
        Old
    }
}