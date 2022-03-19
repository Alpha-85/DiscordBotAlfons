using AlfonsBot.Common.Extensions;
using AlfonsBot.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace AlfonsBot.Modules;

public class ComedyCommandModule : BaseCommandModule
{
    public ComedyCommandModule(ComedyService comedyService)
    {
        ComedyService = comedyService;
    }

    public ComedyService ComedyService { get; set; }

    [Command("darkjoke")]
    public async Task DarkJokeCommand(CommandContext context)
    {
        var joke = await ComedyService.GetDarkHumor();

        await context.RespondAsync(EmbedExtensions.GetDiscordEmbedWithSingleColor(joke)).ConfigureAwait(false);
    }

    [Command("chuck")]
    public async Task ChuckNorrisCommand(CommandContext context)
    {
        var joke = await ComedyService.GetChuckNorrisJoke();

        await context.RespondAsync(EmbedExtensions.GetDiscordEmbedWithThumbNail(joke.IconUrl, joke.Value)).ConfigureAwait(false);
    }

    [Command("dadjoke")]
    public async Task DadJokeCommand(CommandContext context)
    {
        var joke = await ComedyService.GetDadJoke();

        await context.RespondAsync(EmbedExtensions.GetDiscordEmbedWithSingleColor(joke)).ConfigureAwait(false);
    }
}
