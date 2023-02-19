namespace Oleksii_Havryk.DiscordBot.Dto;
/// <summary>
///     Data transfer object of LoggerMessage.
/// </summary>
public class LoggerMessageDto
{
    public string Source { get; set; }
    public string Message { get; set; }
    public DateTime AddTime { get; set; }
    public bool IsRead { get; set; } = false;
}