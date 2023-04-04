using Discord.Interactions;
using Discord.WebSocket;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core.CommandHandlerServices;
/// <summary>
///     Command handler bot service.
/// </summary>
public class BaseCommandHandlerService<T> : ICommandHandlerService
    where T : class
{
    private readonly InteractionService _interactionService;
    private readonly DiscordSocketClient _client;
    private readonly IServiceProvider _services;

    public BaseCommandHandlerService(
        InteractionService interactionService,
        DiscordSocketClient client,
        IServiceProvider services)
    {
        _interactionService = interactionService;
        _client = client;
        _services = services;
    }

    public async Task BeginHandleAsync()
    {
        await _interactionService.AddModuleAsync<T>(_services);
        _client.InteractionCreated += ExecuteCommand;
    }
    public async Task EndHandleAsync()
    {
        _client.InteractionCreated -= ExecuteCommand;
        await _interactionService.RemoveModuleAsync<T>();
    }

    public virtual async Task ExecuteCommand(SocketInteraction arg)
    {
        var ctx = new SocketInteractionContext(_client, arg);
        await _interactionService.ExecuteCommandAsync(ctx, services: null);
    }
}