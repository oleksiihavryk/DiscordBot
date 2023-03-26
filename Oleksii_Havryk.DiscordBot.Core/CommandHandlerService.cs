using Discord.Interactions;
using Discord.WebSocket;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core;
/// <summary>
///     Command handler bot service.
/// </summary>
public class CommandHandlerService : ICommandHandlerService
{
    private readonly InteractionService _interactionService;
    private readonly DiscordSocketClient _client;

    public CommandHandlerService(
        InteractionService interactionService, 
        DiscordSocketClient client)
    {
        _interactionService = interactionService;
        _client = client;
    }

    public async Task BeginHandleAsync()
    {
        await _interactionService.AddModuleAsync<BasicInteractionModule>(services: null);
        _client.InteractionCreated += ExecuteCommand;
    }
    public async Task EndHandleAsync()
    {
        _client.InteractionCreated -= ExecuteCommand;
        await _interactionService.RemoveModuleAsync<BasicInteractionModule>();
    }

    public virtual async Task ExecuteCommand(SocketInteraction arg)
    {
        var ctx = new SocketInteractionContext(_client, arg);
        await _interactionService.ExecuteCommandAsync(ctx, services: null);
    }
}