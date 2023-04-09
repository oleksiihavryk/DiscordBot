using System.ComponentModel;
using Discord;
using Discord.Interactions;
using Oleksii_Havryk.DiscordBot.Core.Interfaces;

namespace Oleksii_Havryk.DiscordBot.Core.CommandHandlerServices;
/// <summary>
///     Bot commands module class.
/// </summary>
public class BasicInteractionModule : InteractionModuleBase
{
    protected ILanguageFilterService LanguageFilterService { get; set; }
    protected IBotLoggingService LoggingService { get; set; }

    public BasicInteractionModule(
        ILanguageFilterService languageFilterService,
        IBotLoggingService loggingService)
    {
        LanguageFilterService = languageFilterService;
        LoggingService = loggingService;
    }

    [SlashCommand(
        name: "votekick",
        description: "Команда для того щоб розпочати голосування за " +
                     "кік обраного користувача! ",
        runMode: RunMode.Async)]
    public virtual async Task Votekick(
        [Description("Користувач який висуваєтся на кік.")] IUser user)
    {
        if (user.IsBot)
        {
            await RespondAsync(
                text: "Ти не можеш просто так взяти і кікнути бота!",
                ephemeral: true);
            return;
        }

        if (user is IGuildUser guildUser)
        {
            var channel = Context.Channel;

            var messageId = (await channel.SendMessageAsync(
                text: $"@everyone Голосування за кік {MentionUtils.MentionUser(user.Id)}.\n" +
                      "Для того щоб проголосувати \"за\" " +
                      "поставте :white_check_mark: на це повідомлення\n" +
                      "Для того щоб проголосувати \"проти\" " +
                      "поставте :x: на це повідомлення\n" +
                      "Час для голосування: 1 година!\n" +
                      "Для кіку необхідно щоб проголосувало \"за\" " +
                      "в півтора рази більше ніж проголосувало проти!")).Id;

            await Task.Delay(TimeSpan.FromHours(1));

            var message = await channel.GetMessageAsync(id: messageId);

            message.Reactions
                .TryGetValue(new Emoji("❌"), out var againstData);
            message.Reactions
                .TryGetValue(new Emoji("✅"), out var forData);

            if (forData.ReactionCount > againstData.ReactionCount * 1.5)
            {
                await guildUser.KickAsync(
                    reason: "Кікнут шляхом демократії (хз за що, потім розберемося)!");
                await channel.SendMessageAsync(
                    text: $"@everyone Користувач з нікнеймом {MentionUtils.MentionUser(user.Id)} був кікнут з серверу.");
                return;
            }

            await channel.SendMessageAsync(
                text: $"@everyone Голосування за кік {MentionUtils.MentionUser(user.Id)} провалилося.");
        }
    }
    [SlashCommand(
        name: "voteban",
        description: "Команда для того щоб розпочати голосування за " +
                     "бан обраного користувача! ",
        runMode: RunMode.Async)]
    public virtual async Task Voteban(
        [Description("Користувач який висуваєтся на бан.")] IUser user)
    {
        if (user.IsBot)
        {
            await RespondAsync(
                text: "Ти не можеш просто так взяти і забанити бота!",
                ephemeral: true);
            return;
        }

        if (user is IGuildUser guildUser)
        {
            var channel = Context.Channel;

            var messageId = (await channel.SendMessageAsync(
                text: $"@everyone Голосування за бан {MentionUtils.MentionUser(user.Id)}.\n" +
                      "Для того щоб проголосувати \"за\" " +
                      "поставте :white_check_mark: на це повідомлення\n" +
                      "Для того щоб проголосувати \"проти\" " +
                      "поставте :x: на це повідомлення\n" +
                      "Час для голосування: 1 день!\n" +
                      "Для бану необхідно щоб проголосувало \"за\" " +
                      "в півтора рази більше ніж проголосувало проти!")).Id;

            await Task.Delay(TimeSpan.FromDays(1));

            var message = await channel.GetMessageAsync(id: messageId);

            message.Reactions
                .TryGetValue(new Emoji("❌"), out var againstData);
            message.Reactions
                .TryGetValue(new Emoji("✅"), out var forData);

            if (forData.ReactionCount > againstData.ReactionCount * 1.5)
            {
                await guildUser.BanAsync(
                    reason: "Забанен шляхом демократії (хз за що, потім розберемося)!");
                await channel.SendMessageAsync(
                    text: $"@everyone Користувач з нікнеймом {MentionUtils.MentionUser(user.Id)} був забанен з серверу.");
                return;
            }

            await channel.SendMessageAsync(
                text: $"@everyone Голосування за бан {MentionUtils.MentionUser(user.Id)} провалилося.");
        }
    }
    [SlashCommand(
        name: "recheck",
        description: "Ініціює повторну перевірку повідомлень на наявність російських слів. ",
        runMode: RunMode.Async)]
    public virtual async Task Recheck(
        [MinValue(0), MaxValue(100)] int count)
    {
        var message = await Context.Channel.SendMessageAsync("Бррр.");
        var messages = Context.Channel.GetMessagesAsync(
            fromMessage: message,
            dir: Direction.Before,
            limit: count,
            mode: CacheMode.AllowDownload);

        var asyncCheckingMessage = new List<Task>();
        await foreach (var collection in messages)
        {
            foreach (var m in collection)
            {
                if (m.Content.Length > 0)
                    asyncCheckingMessage.Add(
                        LanguageFilterService.FilterDiscordMessageAsync(m));
            }
        }

        await Task.WhenAll(asyncCheckingMessage);
        await LoggingService.LogBotMessageAsync(new LogMessage(
            severity: LogSeverity.Info,
            source: nameof(BasicInteractionModule),
            message: "Recheck operation was successfully ended."));
    }
}