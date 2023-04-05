using Discord;
using Discord.WebSocket;

namespace Oleksii_Havryk.DiscordBot.Core.Interfaces;

/// <summary>
///     Language filter bot service interface.
/// </summary>
public interface ILanguageFilterService : IDiscordBotService
{
    Task FilterDiscordMessageAsync(IMessage message);
}