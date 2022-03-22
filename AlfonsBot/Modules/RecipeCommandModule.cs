using AlfonsBot.Common.Extensions;
using AlfonsBot.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace AlfonsBot.Modules;

public class RecipeCommandModule : BaseCommandModule
{
    public RecipeService RecipeService { get; set; }

    public RecipeCommandModule(RecipeService recipeService)
    {
        RecipeService = recipeService;
    }

    [Command("recipe")]
    public async Task RecipeCommand(CommandContext context, [RemainingText, Description("What kind of recipe")] string recipe)
    {
        var preference = await RecipeService.PreferenceFinder(recipe);

        if (string.IsNullOrEmpty(preference))
        {
            var personalMessage = await context.Member.CreateDmChannelAsync();

            await personalMessage.SendMessageAsync(EmbedExtensions.GetPersonalMessageHelpEmbed(
                "example: !recipe beef", "Meal types are: beef,pork,chicken,vegetarian,dessert or breakfast "));
        }

        var auth = await RecipeService.Authenticate();

        var mealType = preference switch
        {
            "Breakfast" => "Breakfast",
            "Dessert" => "Dessert",
            _ => "Lunch"
        };

        var response = await RecipeService.GetRandomRecipe(auth.Token, mealType, preference);

        await context.RespondAsync(EmbedExtensions.GetDiscordRecipeEmbed(response.Title, response.Image, response.SourceUrl)).ConfigureAwait(false);
    }

}
