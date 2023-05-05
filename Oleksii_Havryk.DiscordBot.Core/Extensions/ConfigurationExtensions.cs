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
        services.ConfigureBotOptions(configuration);
        
        services.AddCoreServices();
        
        services.AddLanguageFilterServices();
        services.AddLoggingServices();
        
        services.AddCommands();

        return services.AddSingleton<Bot>();
    }

    private static void AddLoggingServices(this IServiceCollection services)
    {
        //Bot inner services helpers.
        services.AddSingleton<ILoggerMessagesFolder, LoggerMessagesFolder>();
        //Bot inner services.
        services.AddSingleton<IBotLoggingService, BotLoggingService>();
    }
    private static void ConfigureBotOptions(
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
    }
    private static void AddLanguageFilterServices(this IServiceCollection services)
    {
        //dictionary web services
        services.AddSingleton<IUkrainianDictionaryWebService, UkrainianDictionaryWebService>();
        services.AddSingleton<IRussianDictionaryWebService, RussianDictionaryWebService>();
        //Bot inner services helpers.
        services.AddSingleton<ILanguageFilter, UkrainianRussianLanguageFilter>();
        //Bot inner services.
        services.AddSingleton<ILanguageFilterService, LanguageFilterService>();
    }
    private static void AddCommands(this IServiceCollection services)
    {
        //Bot inner services.
        services.AddSingleton<
            ICommandHandlerService,
            CommandHandlerService<BasicInteractionModule>>();
    }
    private static void AddCoreServices(this IServiceCollection services)
    {
        //Bot standard inner controller elements.
        var client = new DiscordSocketClient(config: new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        });
        var interactionService = new InteractionService(client);

        client.Ready += async () => await interactionService.RegisterCommandsGloballyAsync();

        services.AddSingleton<DiscordSocketClient>(client);
        services.AddSingleton<InteractionService>(interactionService);
    }
}