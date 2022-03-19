using DSharpPlus.Entities;

namespace AlfonsBot.Common.Extensions;


public static class EmbedExtensions
{
    public static DiscordEmbed GetDiscordEmbedWithThumbNail(string thumbnail, string message)
        => new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Magenta)
            .WithThumbnail(thumbnail)
            .WithDescription(message);

    public static DiscordEmbed GetDiscordEmbedWithSingleColor(string? message)
        => new DiscordEmbedBuilder()
            .WithColor(DiscordColor.DarkBlue)
            .WithDescription(message);

    public static DiscordEmbed GetRecipeEmbed(string title,string image,string url)
        => new DiscordEmbedBuilder()
            .WithColor(DiscordColor.IndianRed)
            .WithTitle(title)
            .WithUrl(url)
            .WithImageUrl(image)
            .Build();

}

