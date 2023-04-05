using Discord.WebSocket;

namespace Oleksii_Havryk.DiscordBot.Core.Interfaces;

/// <summary>
///     Command handler bot service abstraction.
/// </summary>
public interface ICommandHandlerService : IDiscordBotService
{
    Task ExecuteCommandAsync(SocketInteraction interaction);
}