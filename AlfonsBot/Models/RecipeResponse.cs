using System.Text.Json.Serialization;

namespace AlfonsBot.Models;

public class RecipeResponse
{
    [JsonPropertyName("recipes")]
    public List<Recipe> Recipes { get; set; }
}
