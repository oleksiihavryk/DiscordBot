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
        _client.InteractionCreated += HandleInteraction;
    }
    public async Task EndHandleAsync()
    {
        _client.InteractionCreated -= HandleInteraction;
        await _interactionService.RemoveModuleAsync<BasicInteractionModule>();
    }

    private async Task HandleInteraction(SocketInteraction arg)
    {
        var ctx = new SocketInteractionContext(_client, arg);
        await _interactionService.ExecuteCommandAsync(ctx, services: null);
    }
}