using System.ComponentModel;
using Discord;
using Discord.Interactions;

namespace Oleksii_Havryk.DiscordBot.Core;
/// <summary>
///     Bot commands module class.
/// </summary>
public class BasicInteractionModule : InteractionModuleBase
{
    [SlashCommand(
        name: "votekick",
        description: "Команда для того щоб розпочати голосування за " +
                     "кік обраного користувача! ",
        runMode: RunMode.Async)]
    public async Task Votekick(
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
    public async Task Voteban(
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
}