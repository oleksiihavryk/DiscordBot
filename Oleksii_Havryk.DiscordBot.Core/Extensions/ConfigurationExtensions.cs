using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oleksii_Havryk.DiscordBot.Core.CommandHandlerServices;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;
using Oleksii_Havryk.DiscordBot.Core.LanguageFilterServices;
using Oleksii_Havryk.DiscordBot.Core.LoggingServices;
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
        //Russian dictionary api key
        services.AddOptions<RussianDictionaryOptions>().Configure(rdo =>
        {
            rdo.Key = configuration
                .GetSection("Bot")
                .GetValue<string>("RussianDictionaryApiKey") ?? string.Empty;
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

        //dictionary web services
        services.AddSingleton<IUkrainianDictionaryWebService, UkrainianDictionaryWebService>();
        services.AddSingleton<IRussianDictionaryWebService, RussianDictionaryWebService>();

        //Bot inner services helpers.
        services.AddSingleton<ILanguageFilter, UkrainianRussianLanguageFilter>();
        services.AddSingleton<ILoggerMessagesFolder, LoggerMessagesFolder>();

        //Bot inner services.
        services.AddSingleton<ILanguageFilterService, LanguageFilterService>();
        services.AddSingleton<IBotLoggingService, BotLoggingService>();
        services.AddSingleton<
            ICommandHandlerService,
            CommandHandlerService<BasicInteractionModule>>();
        
        return services.AddSingleton<Bot>();
    }
}