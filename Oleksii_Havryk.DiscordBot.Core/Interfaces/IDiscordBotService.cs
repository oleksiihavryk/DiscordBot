namespace Oleksii_Havryk.DiscordBot.Core.Interfaces;
/// <summary>
///     Discord bot service. 
/// </summary>
public interface IDiscordBotService
{
    Task BeginHandleAsync();
    Task EndHandleAsync();
}