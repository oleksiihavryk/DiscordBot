using Discord.Interactions;
using Discord.WebSocket;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core.CommandHandlerServices;
/// <summary>
///     Command handler bot service.
/// </summary>
public class CommandHandlerService<T> : ICommandHandlerService
    where T : class
{
    private readonly InteractionService _interactionService;
    private readonly DiscordSocketClient _client;
    private readonly IServiceProvider _services;

    public CommandHandlerService(
        InteractionService interactionService,
        DiscordSocketClient client,
        IServiceProvider services)
    {
        _interactionService = interactionService;
        _client = client;
        _services = services;
    }

    public virtual async Task BeginHandleAsync()
    {
        await _interactionService.AddModuleAsync<T>(_services);
        _client.InteractionCreated += ExecuteCommandAsync;
    }
    public virtual async Task EndHandleAsync()
    {
        _client.InteractionCreated -= ExecuteCommandAsync;
        await _interactionService.RemoveModuleAsync<T>();
    }

    public virtual async Task ExecuteCommandAsync(SocketInteraction arg)
    {
        var ctx = await CreateSocketInteractionContextAsync(arg);
        await _interactionService.ExecuteCommandAsync(ctx, _services);
    }

    protected virtual async Task<SocketInteractionContext> CreateSocketInteractionContextAsync(
        SocketInteraction arg)
        => await Task.FromResult(new SocketInteractionContext(_client, arg));
}