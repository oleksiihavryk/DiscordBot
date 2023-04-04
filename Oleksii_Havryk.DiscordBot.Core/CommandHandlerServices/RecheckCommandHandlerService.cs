using Discord.Interactions;
using Discord.WebSocket;
using Oleksii_Havryk.DiscordBot.Core.CommandHandlerServices.Commands;

namespace Oleksii_Havryk.DiscordBot.Core.CommandHandlerServices;

/// <summary>
///     Command handler service with recheck command module.
/// </summary>
public class RecheckCommandHandlerService
    : BaseCommandHandlerService<RecheckCommandInteractionModule>
{
    public RecheckCommandHandlerService(
        DiscordSocketClient client,
        InteractionService interactionService)
        : base(interactionService, client)
    {
    }
}