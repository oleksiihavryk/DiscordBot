using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oleksii_Havryk.DiscordBot.Core.CommandHandlerServices;
using Oleksii_Havryk.DiscordBot.Core.Commands;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using Oleksii_Havryk.DiscordBot.Core.LanguageFilterServices;
using Oleksii_Havryk.DiscordBot.Core.Options;

namespace Oleksii_Havryk.DiscordBot.Core.Extensions;
/// <summary>
///     Application configuration.
/// </summary>
public static class ConfigurationExtensions
{
    public static IServiceCollection AddDiscordBot(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //Bot options.
        services.AddOptions<BotOptions>().Configure(bto =>
        {
            bto.TokenValue = configuration
                .GetSection("Bot")
                .GetValue<string>("Token") ?? string.Empty;
        });
        //Exceptional users.
        services.AddOptions<ExceptionalUsersOptions>().Configure(euo =>
        {
            euo.Identificators = configuration
                .GetSection("Bot:ExceptionalUserIds")
                .Get<string[]>() ?? Array.Empty<string>();
        });

        //Bot standard inner controller elements.
        var client = new DiscordSocketClient(config: new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        });
        var interactionService = new InteractionService(client);

        client.Ready += async () => await interactionService.RegisterCommandsGloballyAsync();

        services.AddSingleton<DiscordSocketClient>(client);
        services.AddSingleton<InteractionService>(interactionService);

        //Bot inner services helpers.
        services.AddSingleton<ILanguageFilter, UkrainianRussianLanguageFilter>(sp =>
        {
            var newServiceProvider = sp.CreateScope().ServiceProvider;
            var httpClientFactory = newServiceProvider.GetRequiredService<IHttpClientFactory>();

            return new UkrainianRussianLanguageFilter(
                httpClientFactory: httpClientFactory,
                key: configuration
                    .GetSection("Bot")
                    .GetValue<string>("RussianDictionaryApiKey") ?? string.Empty);
        });
        services.AddSingleton<ILoggerMessagesFolder, LoggerMessagesFolder>();

        //Bot inner services.
        services.AddSingleton<ILanguageFilterService, LogImprovedLanguageFilterService>();
        services.AddSingleton<IBotLoggingService, BotLoggingService>();
        services.AddSingleton<ICommandHandlerService, 
            BaseCommandHandlerService<RecheckMessageInteractionModule>>();
        
        return services.AddSingleton<Bot>();
    }
}